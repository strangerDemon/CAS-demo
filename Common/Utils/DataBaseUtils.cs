using System;
using System.Collections.Generic;
using ZoneTop.Application.SSO.Common.Entity;
using ZoneTop.Application.SSO.Common.Model;
using ZoneTop.Application.SSO.Common.Service;

namespace ZoneTop.Application.SSO.Common.Utils
{
    /// <summary>
    /// 数据库 工具类
    /// </summary>
    public class DataBaseUtils
    {
        private static LogModel log = LogUtils.GetLogger("DataBaseUtils");

        #region userService
        /// <summary>
        /// 数据库获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password">为null时条件无效</param>
        /// <returns></returns>
        public static BaseUserEntity GetUserEntity(string account, string password)
        {
            UserService service = new UserService();
            IEnumerable<BaseUserEntity> userList = service.GetUserList(account, "");

            BaseUserEntity _entity = new BaseUserEntity();
            int accountEqual = 0;
            foreach (BaseUserEntity item in userList)
            {
                if (item.Account == account)
                {
                    _entity = item;
                    accountEqual++;
                }
            }
            if (accountEqual > 1)
            {
                throw new Exception("账号重名，请联系管理员");
            }
            else if (accountEqual == 1)
            {
                if (password == null || password.IsEmpty())
                {
                    return _entity;
                }
                string dbPassword = EncryptUtils.doEncrypt(password);//Md5Helper.MD5(DESEncrypt.Encrypt(password.ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                if (dbPassword == _entity.Password)
                {
                    return _entity;
                }
                else
                {
                    throw new Exception("密码和账户名不匹配");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }
        }
        #endregion

        #region clientService
        /// <summary>
        /// 获取用户权限下的客户端
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<ClientEntity> getAuthClient(string userId)
        {
            ClientService clientService = new ClientService();

            return clientService.GetAuthClientList(userId);
        }

        /// <summary>
        /// 获取所有客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ClientEntity> getAllClient()
        {
            ClientService clientService = new ClientService();

            return clientService.GetClientList();
        }
        #endregion

        #region casLogService
        /// <summary>
        /// 新增cas登录日志
        /// </summary>
        /// <returns></returns>
        public static CasLogEntity casLoginLog(BaseUserEntity user)
        {
            CasLogService casLogService = new CasLogService();

            CasLogEntity log = new CasLogEntity();
            log.Create();
            log.UserId = user.UserId;
            log.UserAccount = user.Account;
            log.UserName = user.RealName;

            log.BrowserType = WebHelperUtils.Browser;
            log.IPAddress = WebHelperUtils.Ip;

            log.SESSIONID = UserUtils.Provider.getCurrentSession();


            return casLogService.Save(log) ? log : null;
        }

        /// <summary>
        /// 退出时修改cas日志
        /// </summary>
        /// <returns></returns>
        public static bool casLogoutLog(UserModel user)
        {
            CasLogService casLogService = new CasLogService();
            CasLogEntity log = casLogService.GetCasLog(user.CasLogId);

            log.LogStatus = 0;
            log.LogoutTime = DateTime.Now;

            return casLogService.Update(log);
        }
        #endregion

        #region clientLogService
        /// <summary>
        /// 新增client日志
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="clientModel"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static ClientLogEntity clientLoginLog(string sessionId, ClientModel clientModel, string description)
        {
            UserModel user = UserUtils.Provider.GetUser(sessionId);

            ClientLogService clientLogService = new ClientLogService();
            ClientLogEntity log = new ClientLogEntity();

            log.Create();
            log.CasLogId = user.CasLogId;

            log.AppId = clientModel.ClientId;
            log.AppName = clientModel.ClientName;
            log.AppUrl = clientModel.ClientUrl;

            log.ST = clientModel.Ticket;
            log.STValidated = clientModel.TicketValidated;
            log.STCreateTime = clientModel.TicketCreateTime;
            log.STEFTime = clientModel.TicketEFTime;
            log.STValidateTime = clientModel.TicketValidateTime;

            log.SESSIONID = UserUtils.Provider.getCurrentSession();
            log.Description = description;

            return clientLogService.Save(log) ? log : null;
        }

        /// <summary>
        /// 退出时修改client日志
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static bool clientLogoutLog(ClientModel client)
        {
            try
            {
                ClientLogService clientLogService = new ClientLogService();
                ClientLogEntity log = clientLogService.GetClientLog(client.ClientLogId);

                log.LogStatus = 0;
                log.LogoutTime = DateTime.Now;

                return clientLogService.Update(log);
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                return false;
            }
        }
        #endregion
    }
}
