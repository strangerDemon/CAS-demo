using System;
using System.Collections.Generic;
using System.Net;
using ZoneTop.Application.SSO.Common.Entity;
using ZoneTop.Application.SSO.Common.Grobal;
using ZoneTop.Application.SSO.Common.Model;

namespace ZoneTop.Application.SSO.Common.Utils
{
    /// <summary>
    /// 客户端工具
    /// </summary>
    public class ClientUtils
    {
        private LogModel log = LogUtils.GetLogger("ClientUtils");

        #region 静态实例
        /// <summary>
        /// 当前提供者
        /// </summary>
        public static ClientUtils Provider
        {
            get { return new ClientUtils(); }
        }
        #endregion

        #region 获取所有客户端
        /// <summary>
        /// 获取所有客户端
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ClientEntity> getAllClient()
        {
            return DataBaseUtils.getAllClient();
        }
        #endregion

        #region 判断当前客户端 是否有效
        /// <summary>
        /// 获取所有客户端
        /// </summary>
        /// <returns></returns>
        public virtual bool ValideClient(string clientUrl)
        {
            IEnumerable<ClientEntity> clientList = DataBaseUtils.getAllClient();
            foreach (ClientEntity client in clientList)
            {
                if (client.AppSvcUrl.Equals(clientUrl))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 获取用户有权限的客户端
        /// <summary>
        /// 获取用户有权限的客户端
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual IEnumerable<ClientEntity> getAllAuthClient(string userId)
        {
            return DataBaseUtils.getAuthClient(userId);
        }
        #endregion

        #region 判断当前用户是否有权限客户端权限
        /// <summary>
        /// 验证APP
        /// </summary>
        /// <param name="clientUrl"></param>
        /// <returns></returns>
        public virtual bool UserValideClient(string clientUrl)
        {
            UserModel user = UserUtils.Provider.Current();
            IEnumerable<ClientEntity> clientList = getAllAuthClient(user.UserId);
            foreach (ClientEntity client in clientList)
            {
                if (client.AppSvcUrl.Equals(clientUrl))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 判断当前用户是否有权限
        /// <summary>
        /// 验证APP
        /// </summary>
        /// <param name="clientUrl"></param>
        /// <returns></returns>
        public virtual ClientEntity GetClientByService(string clientUrl)
        {
            UserModel user = UserUtils.Provider.Current();
            IEnumerable<ClientEntity> clientList = getAllAuthClient(user.UserId);
            foreach (ClientEntity client in clientList)
            {
                if (client.AppSvcUrl.Equals(clientUrl))
                {
                    return client;
                }
            }
            return null;
        }
        #endregion

        #region cas全部客户端登出操作
        /// <summary>
        /// 退出一个客户端
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="user"></param>
        /// <param name="client"></param>
        /// <param name="entity"></param>
        public virtual void LogoutOne(string sessionId, UserModel user, ClientModel client, ClientEntity entity)
        {
            //更新redis
            user.Clients.Remove(client);
            UserUtils.Provider.UpdateUser(sessionId, user);

            postLogout(client, entity.LogoutUrl);
        }

        /// <summary>
        /// 退出所有的客户端
        /// </summary>
        /// <param name="user">redis中存的用户</param>
        /// <param name="clients">用户的权限客户端</param>
        /// <param name="UnClearSessionId">不清理的sessionid,防止用户DoLogout登出本用户</param>
        public virtual void LogoutAllAction(UserModel user, List<ClientEntity> clients, string UnClearSessionId)
        {
            DataBaseUtils.casLogoutLog(user);

            if (clients != null && user.Clients != null)
            {
                foreach (ClientEntity _client in clients)
                {
                    ClientModel clientModel = user.Clients.Find(t => t.ClientId == _client.AppId && t.SessionIdValue != UnClearSessionId);
                    if (clientModel != null)
                    {
                        postLogout(clientModel, _client.LogoutUrl);
                    }
                }
            }
        }

        /// <summary>
        /// 退出客户端post
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logoutUrl"></param>
        private void postLogout(ClientModel client, string logoutUrl)
        {
            try
            {
                //数据库日志
                DataBaseUtils.clientLogoutLog(client);
                //清除客户端与cas的绑定
                if (GrobalConfig.IsApiCheck)
                {
                    RedisDelClient(client.SessionIdValue);
                }
                //请求客户端的退出 客户端根据session退出
                CookieContainer container = new CookieContainer();
                Cookie cookie = new Cookie(client.SessionIdKey, client.SessionIdValue);
                cookie.Domain = GetDomain(client.ClientUrl);
                container.Add(cookie);
                HttpUtils.HttpPost(logoutUrl, "", container);
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
            }
        }
        #endregion

        #region ticket

        #region 获取ticket
        /// <summary>
        /// 写入登录信息
        /// </summary>
        /// <param name="clientUrl">客户端地址</param>
        /// <param name="code">客户端防伪标识</param>
        public virtual string GetTicket(string clientUrl, string code)
        {
            try
            {
                UserModel user = UserUtils.Provider.Current();
                user.Clients = user.Clients == null ? new List<ClientModel>() : user.Clients;

                ClientModel client = null;
                foreach (var item in user.Clients)
                {
                    if (clientUrl.Contains(item.ClientUrl))
                    {
                        client = item;
                    }
                }
                //扩展ticket使用
                string ticket = createTicket();//创建ticket
                string encTicket = doEncryptTicket(ticket, clientUrl);//redis加密的ticket
                RedisAddTicket(encTicket, user.UserId);
                if (client == null)
                {
                    ClientEntity clientEntity = GetClientByService(clientUrl);
                    //注册系统
                    client = new ClientModel();
                    client.ClientId = clientEntity.AppId;
                    client.ClientName = clientEntity.AppName;
                    client.ClientPlatfrom = clientEntity.AppPlatform;

                    client.ClientUrl = clientUrl;  //后续需要修改为从系统应用实体类获取
                    client.Ticket = ticket;
                    client.TicketCreateTime = DateTime.Now;
                    client.TicketEFTime = GrobalConfig.TicketTimeOut;
                    client.TicketValidated = -1;
                    client.ClientLogId = "";
                    client.Code = code;
                    user.Clients.Add(client);
                }
                else
                {
                    user.Clients.Remove(client);//移除旧的
                    client.Ticket = ticket;
                    client.TicketCreateTime = DateTime.Now;
                    client.TicketEFTime = GrobalConfig.TicketTimeOut;
                    client.TicketValidated = -1;
                    client.Code = code;
                    user.Clients.Add(client);//添加编辑后的
                }
                //更新redis用户信息
                UserUtils.Provider.UpdateCurrent(user);
                return encTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 校验ticket
        /// <summary>
        /// 验证ST
        /// todo:ticket 应该与浏览器，ip绑定
        /// </summary>
        /// <param name="ticket">验证ST</param>
        /// <param name="service">应用地址</param>
        /// <param name="code">客户端防伪标识</param>
        /// <param name="sessionIdKey">客户端 sessionIdKey</param>
        /// <param name="sessionIdValue">客户端 sessionIdValue</param>
        public virtual UserModel VerifyTicket(string ticket, string service, string code, string sessionIdKey, string sessionIdValue)
        {
            try
            {
                string userId = RedisGetTicket(ticket);

                if (userId == null)
                {
                    throw new Exception("无效票据！");
                }

                string sessionId = UserUtils.Provider.UserSession(userId);

                if (sessionId == null)
                {
                    throw new Exception("用户未登录！");
                }

                UserModel user = UserUtils.Provider.GetUser(sessionId);

                if (user == null)
                {
                    throw new Exception("用户未登录！");
                }

                if (user.Clients == null)
                {
                    throw new Exception("用户未登录客户端！");
                }

                ClientModel clientModel = user.Clients.Find(t => t.ClientUrl == service);

                if (clientModel == null)
                {
                    throw new Exception("客户端未登录！");
                }
                else
                {
                    RedisDelTicket(ticket);
                    if (clientModel.TicketValidated == -1)
                    {
                        user.Clients.Remove(clientModel);//移除旧的

                        clientModel.TicketValidateTime = DateTime.Now;
                        clientModel.SessionIdKey = sessionIdKey;
                        clientModel.TicketValidated = 1;
                        clientModel.SessionIdValue = sessionIdValue;
                        string description = "";
                        if (!doDecryptTicket(service, ticket))
                        {
                            clientModel.TicketValidated = 0;
                            description = "无效票据！";
                        }
                        if (!clientModel.ClientUrl.Equals(service) || !clientModel.Code.Equals(code))
                        {
                            clientModel.TicketValidated = 0;
                            description = "客户端不一致！";
                        }

                        ClientLogEntity log = DataBaseUtils.clientLoginLog(sessionId, clientModel, description);
                        clientModel.ClientLogId = log.AppLogId;
                        user.Clients.Add(clientModel);
                        UserUtils.Provider.UpdateUser(sessionId, user);//添加编辑后的

                        if (clientModel.TicketValidated == 1)
                        {
                            if (GrobalConfig.IsApiCheck)
                            {
                                user.Clients.ForEach(t =>
                                {
                                    RedisAddClient(t.SessionIdValue, sessionId);
                                });
                            }
                            return user;
                        }
                        else
                        {
                            throw new Exception(description);
                        }
                    }
                    else
                    {
                        throw new Exception("客户端已登录校验！");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ticket 创建、加密、解密  todo:可扩展ticket加解密
        /// <summary>
        /// 创建ticket
        /// </summary>
        /// <returns></returns>
        private static string createTicket()
        {
            return GrobalConfig.TicketTitle + Guid.NewGuid().ToString();
        }

        /// <summary>
        /// ticket 加密 未实现
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="clientUrl"></param>
        /// <returns></returns>
        private static string doEncryptTicket(string ticket, string clientUrl)
        {
            return ticket;
        }

        /// <summary>
        /// ticket 解密 未实现
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="clientUrl"></param>
        /// <returns></returns>
        private static bool doDecryptTicket(string ticket, string clientUrl)
        {
            return true;
        }

        #endregion

        #region redis ticket 增删查用户信息
        private void RedisAddTicket(string ticket, string userId)
        {
            RedisCacheUtils.Select(3);
            TimeSpan timespan = new TimeSpan(GrobalConfig.TicketTimeOut * TimeSpan.TicksPerSecond);
            RedisCacheUtils.Set(ticket, userId, timespan);
        }

        private void RedisDelTicket(string ticket)
        {
            RedisCacheUtils.Select(3);
            RedisCacheUtils.Remove(ticket);
        }

        private string RedisGetTicket(string ticket)
        {
            RedisCacheUtils.Select(3);
            return RedisCacheUtils.Get<string>(ticket);
        }
        #endregion

        #endregion

        #region  条用接口身份校验
        /// <summary>
        /// 条用接口身份校验
        /// </summary>
        /// <param name="clientSessionId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool ApiIdentifyCheck(string clientSessionId, string code)
        {
            if (GrobalConfig.IsApiCheck)
            {
                if (clientSessionId.Equals(""))
                {
                    throw new Exception("仅已登录客户端可调用！");
                }
                string casSessionId = RedisGetClient(clientSessionId);
                if (casSessionId == null || casSessionId.Equals(""))
                {
                    throw new Exception("无效客户端！");
                }
                UserModel user = UserUtils.Provider.GetUser(casSessionId);
                if (user.Clients == null || user.Clients.Count == 0) { return false; }
                ClientModel client = user.Clients.Find(t => t.SessionIdValue == clientSessionId && t.Code == code);
                return client != null;
            }
            return true;
        }

        /// <summary>
        /// 客户端 是否能调用接口
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public bool isApiAuth(string service)
        {
            return true;
        }

        /// <summary>
        /// 判断客户端是否能调用登出接口
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public bool LogoutClient(string service)
        {
            string[] clients = GrobalConfig.LogoutClient.Split(',');
            foreach (string client in clients)
            {
                if (client.Equals(service))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion 

        #region redis 客户端sessionid 绑定服务端sessionid 查询方便
        /// <summary>
        /// client sessionId => cas sessionId
        /// </summary>
        /// <param name="clientSessionId"></param>
        /// <param name="casSessionId"></param>
        private void RedisAddClient(string clientSessionId, string casSessionId)
        {
            RedisCacheUtils.Select(2);
            RedisCacheUtils.Set(clientSessionId, casSessionId, null);
        }

        private void RedisDelClient(string clientSessionId)
        {
            RedisCacheUtils.Select(2);
            RedisCacheUtils.Remove(clientSessionId);
        }

        private string RedisGetClient(string clientSessionId)
        {
            RedisCacheUtils.Select(2);
            return RedisCacheUtils.Get<string>(clientSessionId);
        }
        #endregion 

        #region domain
        /// <summary>
        /// 处理clientUrl 获取cookie需要的domain
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual string GetDomain(string url)
        {
            url = url.IndexOf("://") > 0 ? url.Substring(url.IndexOf("://") + 3) : url;//去掉协议的地址
            url = url.IndexOf("/") > 0 ? url.Substring(0, url.IndexOf("/")) : url;//去掉参数
            url = url.IndexOf(":") > 0 ? url.Substring(0, url.IndexOf(":")) : url;//去掉端口

            return url;
        }
        #endregion 
    }
}
