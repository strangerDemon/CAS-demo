using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZoneTop.Application.SSO.Common.Entity;
using ZoneTop.Application.SSO.Common.Grobal;
using ZoneTop.Application.SSO.Common.Model;
using ZoneTop.Application.SSO.Common.Utils;


namespace ZoneTop.Application.SSO.Controllers
{
    /// <summary>
    /// 视图controller、接口
    /// </summary>
    public class casController : MvcControllerBase
    {

        private LogModel log = LogUtils.GetLogger("casController");

        #region CAS

        #region 视图功能
        /// <summary>
        /// 系统登陆页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult login()
        {
            try
            {
                //账户名登录
                string service = Request["service"] == null ? "" : Request["service"].ToString();
                //客户端来的防伪标识
                string code = Request["code"] == null ? "" : Request["code"].ToString();
                UserModel user = UserUtils.Provider.Current();
                if (!service.Equals("") && !ClientUtils.Provider.ValideClient(service))//无效service
                {
                    MyRedirect("/cas/error", false, new { header = GrobalConfig.InvalidErrorHeader, context = GrobalConfig.InvalidErrorText });
                }
                else if (user != null)//用户已登陆
                {
                    if (service.Equals(""))
                    {
                        MyRedirect("/cas/index", false, null);//跳转到集成页面
                    }
                    else if (ClientUtils.Provider.UserValideClient(service))
                    {
                        string ST = ClientUtils.Provider.GetTicket(service, code);
                        MyRedirect(service + "?ST=" + ST, false, null);
                    }
                    else//用户无service权限
                    {
                        MyRedirect("/cas/error", false, new { header = GrobalConfig.UnauthorizedErrorHeader, context = GrobalConfig.UnauthorizedErrorText });
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                //跳转系统未授权页面
                MyRedirect("/cas/error", false, new { header = GrobalConfig.ErrorHeader, context = GrobalConfig.ErrorText });
                return View();
            }
        }

        /// <summary>
        /// 系统集成登出，客户端登出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult logout()
        {
            try
            {
                var service = Request["service"];
                UserModel user = UserUtils.Provider.Current();

                List<ClientEntity> clients = ClientUtils.Provider.getAllAuthClient(user.UserId).ToList();
                ClientEntity client = null;

                bool isLogoutAll = true;
                if (service != null)//客户端请求登出页面
                {
                    client = clients.Find(t => t.AppSvcUrl == service);
                    isLogoutAll = client.SingleLogout == 1;
                }

                if (isLogoutAll)
                {
                    ClientUtils.Provider.LogoutAllAction(user, clients, "");
                    //清理session 和redis
                    UserUtils.Provider.EmptyCurrent();
                }
                else if (client != null && user.Clients != null)//未接入单点登录，只退出自己
                {
                    ClientModel _client = user.Clients.Find(t => t.ClientId == client.AppId);
                    if (_client != null)
                    {
                        ClientUtils.Provider.LogoutOne(UserUtils.Provider.getCurrentSession(), user, _client, client);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
            }

            return View();
        }

        /// <summary>
        /// 系统集成页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult index()
        {
            UserModel user = UserUtils.Provider.Current();
            //redis 登录超时 或 未登录 跳转登出页面 清除数据
            if (user == null)
            {
                Response.Redirect("login");
            }
            return View();
        }

        /// <summary>
        /// 错误页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult error()
        {
            return View();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VerifyCode()
        {
            return File(new VerifyCodeUtils().GetVerifyCode(), @"image/Gif");
        }

        /// <summary>
        /// 自定义重定向
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isOver"></param>
        /// <param name="param"></param>
        private void MyRedirect(string url, bool isOver, object param)
        {
            Response.Clear();
            //todo：隐式传参
            string connect = url.IndexOf("?") > 0 ? "&" : "?";
            if (param != null)
            {
                url += "?para=" + Server.UrlEncode(JsonUtils.ToJson(param));
                connect = "&";
            }
            //加时间戳
            url += connect + "datetime=" + DateTime.Now.Ticks;
            Response.Redirect(url, isOver);
            Response.End();
        }
        #endregion

        #region 获取数据

        #region 获取用户权限内的客户端
        /// <summary>
        /// 获取用户权限内的客户端
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInfos()
        {
            UserModel user = UserUtils.Provider.Current();
            IEnumerable<ClientEntity> clientList = ClientUtils.Provider.getAllAuthClient(user.UserId);
            JObject systemInfo = JsonUtils.ReadJsonConfig("SysConfig.json");
            var objResult = new
            {
                userInfo = user,
                systemInfo = systemInfo,
                clientList = clientList
            };

            return ToJsonResult(objResult);
        }
        #endregion

        #endregion 

        #region 提交数据

        #region 登录操作
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="verifycode">验证码</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public void CheckLogin(string username, string password, string verifycode)
        {
            JObject joResult = new JObject();
            try
            {
                #region 验证码验证                
                verifycode = EncryptUtils.doEncrypt(verifycode.ToLower());
                if (Session[GrobalConfig.VerifyCode].IsEmpty() || verifycode != Session[GrobalConfig.VerifyCode].ToString())
                {
                    throw new Exception("验证码错误，请重新输入");
                }
                #endregion

                #region 内部账户验证
                UserModel userModel = UserUtils.Provider.CheckLogin(username, password);
                #endregion

                #region 多地登录
                //不支持多地登录处理
                if (!GrobalConfig.IsMultiplePlace)
                {
                    string otherSessionId = UserUtils.Provider.UserSession(userModel.UserId);
                    if (otherSessionId != null && !otherSessionId.Equals("") && !otherSessionId.Equals(UserUtils.Provider.getCurrentSession()))//redis 中存在（已在别地登录）
                    {
                        UserModel user = UserUtils.Provider.GetUser(otherSessionId);
                        List<ClientEntity> clients = ClientUtils.Provider.getAllAuthClient(user.UserId).ToList();
                        //post 请求清理客户端用户登录信息
                        ClientUtils.Provider.LogoutAllAction(user, clients, "");

                        //清理session 和redis
                        UserUtils.Provider.EmptyUser(user.UserId);
                    }
                }
                #endregion

                UserUtils.Provider.AddCurrent(userModel);

                #region 登录返回
                joResult["type"] = 1;
                joResult["message"] = "登陆成功";
                joResult["st"] = "";
                string service = Request["service"] == null ? "" : Request["service"].ToString();
                string code = Request["code"] == null ? "" : Request["code"].ToString();
                joResult["service"] = service;
                if (!service.Equals(""))
                {
                    joResult["st"] = ClientUtils.Provider.GetTicket(service, code);
                }
                Response.Write(joResult.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                joResult["type"] = 0;
                joResult["message"] = ex.Message;
                Response.Write(joResult.ToJson());
            }
            finally
            {
                Response.End();
            }
        }
        #endregion

        #endregion

        #endregion

        #region 接口

        #region 客户端 票据验证
        /// <summary>
        /// 验证票据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void VerifyTicket()
        {
            JObject joResult = new JObject();
            try
            {
                string ticket = Request["ST"] == null ? "" : Request["ST"].ToString();
                string service = Request["service"] == null ? "" : Request["service"].ToString();
                string code = Request["code"] == null ? "" : Request["code"].ToString();
                string sessionIdKey = Request["sessionIdKey"] == null ? "" : Request["sessionIdKey"].ToString();
                string sessionIdValue = Request["sessionIdValue"] == null ? "" : Request["sessionIdValue"].ToString();

                if (ticket.Equals("") || service.Equals("") || code.Equals(""))
                {
                    joResult["code"] = 0;
                    joResult["message"] = "参数不正确";
                }

                if (service != "")
                {
                    //验证系统是否有效：即是否接入SSO
                    //ADD CODE
                    UserModel objUserInfo = ClientUtils.Provider.VerifyTicket(ticket, service, code, sessionIdKey, sessionIdValue);

                    joResult["code"] = 1;
                    joResult["message"] = "验证通过";
                    joResult["account"] = objUserInfo.Account;
                    joResult["username"] = objUserInfo.UserName;
                }

                Response.Write(joResult.ToJson());
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                joResult["code"] = 0;
                joResult["message"] = ex.Message;
                Response.Write(joResult.ToJson());
            }
            finally
            {
                Response.End();
            }
        }
        #endregion

        #region 获取在线用户列表
        /// <summary>
        /// 获取在线用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void GetOnLines()
        {
            JObject joResult = new JObject();
            try
            {
                #region 接口权限
                string clientSessionId = Request["sessionId"] == null ? "" : Request["sessionId"].ToString();//客户端sessionid
                string code = Request["code"] == null ? "" : Request["code"].ToString();//客户端防伪标识

                if (!ClientUtils.Provider.ApiIdentifyCheck(clientSessionId, code))
                {
                    throw new Exception("请求接口仅已客户端可调用！");
                }

                string service = Request["service"] == null ? "" : Request["service"].ToString();//调用的客户端
                ClientEntity clientEntity = ClientUtils.Provider.GetClient(service);

                ClientUtils.Provider.isApiAuth(clientEntity);
                #endregion

                string client = Request["client"] == null ? "" : Request["client"].ToString();//客户端
                if (!client.Equals(""))
                {
                    List<ClientEntity> clientList = ClientUtils.Provider.getAllClient().ToList();
                    ClientEntity _client = clientList.Find(t => t.AppSvcUrl == client);
                    if (_client == null)
                    {
                        throw new Exception("客户端不存在！");
                    }
                }
                IDictionary<string, UserModel> users = UserUtils.Provider.GetAllOnLines();
                JArray list = new JArray();
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        if (client.Equals("") || (user.Value.Clients.Find(t => t.ClientUrl == client) != null))
                        {
                            //屏蔽隐秘数据
                            user.Value.Password = "";
                            user.Value.Clients.ForEach(t =>
                            {
                                t.SessionIdKey = "";
                                t.SessionIdValue = "";
                                t.Code = "";
                                t.Ticket = "";
                            });
                            list.Add(JsonUtils.ModelToJObject(user.Value));
                        }
                    }
                }

                joResult["code"] = 1;
                joResult["message"] = "成功";
                joResult["data"] = list;
                Response.Write(joResult.ToJson());
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                joResult["code"] = 0;
                joResult["message"] = ex.Message;
                joResult["data"] = new JArray();
                Response.Write(joResult.ToJson());
            }
            finally
            {
                Response.End();
            }
        }
        #endregion

        #region 登出接口
        /// <summary>
        /// 登出接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void DoLogout()
        {
            JObject joResult = new JObject();
            try
            {
                #region 验证调用权限               
                string clientSessionId = Request["sessionId"] == null ? "" : Request["sessionId"].ToString();//客户端sessionid
                string code = Request["code"] == null ? "" : Request["code"].ToString();//客户端防伪标识

                if (!ClientUtils.Provider.ApiIdentifyCheck(clientSessionId, code))
                {
                    throw new Exception("请求接口仅已登录客户端可调用！");
                }

                string service = Request["service"] == null ? "" : Request["service"].ToString();//客户端
                ClientEntity clientEntity = ClientUtils.Provider.GetClient(service);

                ClientUtils.Provider.isApiAuth(clientEntity);

                ClientUtils.Provider.LogoutClient(clientEntity);
                #endregion

                #region 账户
                string account = Request["account"] == null ? "" : Request["account"].ToString();//用户账号
                if (account.Equals(""))
                {
                    throw new Exception("请指定需要登出的用户！");
                }
                BaseUserEntity userEntity = UserUtils.Provider.GetUserEntity(account);

                string sessionid = UserUtils.Provider.UserSession(userEntity.UserId);

                if (sessionid == null || sessionid.Equals(""))
                {
                    throw new Exception("未查询到用户登录信息！");
                }

                UserModel user = UserUtils.Provider.GetUser(sessionid);
                if (user == null)
                {
                    throw new Exception("未查询到用户登录信息！");
                }

                if (user.Clients == null || user.Clients.Count == 0)
                {
                    throw new Exception("用户未登录客户端！");
                }
                #endregion 

                List<ClientEntity> clientList = ClientUtils.Provider.getAllAuthClient(user.UserId).ToList();

                #region 只登出具体客户端 废弃
                /*string client = Request["client"] == null ? "" : Request["client"].ToString();//需要登出的客户端 
                //bool isRelation = Request["isRelation"] == null ? true : bool.Parse(Request["isRelation"].ToString());//是否关联登出

                ClientEntity clientEntity = null;
                ClientModel clientModel = null;

                if (client != "")
                {
                    clientEntity = clientList.Find(t => t.AppSvcUrl == client);
                    if (clientEntity == null)
                    {
                        throw new Exception("未授权客户端！");
                    }
                    isRelation = isRelation && (clientEntity.IsJoinSso == 1);

                    clientModel = user.Clients.Find(t => t.ClientUrl == client);
                    if (clientModel == null)
                    {
                        throw new Exception("客户端未登录！");
                    }
                }

                if (!isRelation && clientModel != null && clientEntity != null)//只登本客户端
                {
                    ClientUtils.Provider.LogoutOne(sessionid, user, clientModel, clientEntity);
                }
                else//登出用户登录的所有客户端
                {
                    ClientUtils.Provider.LogoutAllAction(user, clientList);
                    //清理session 和redis
                    UserUtils.Provider.EmptyUser(user.UserId);
                }*/
                #endregion

                ClientUtils.Provider.LogoutAllAction(user, clientList, clientSessionId);
                //清理session 和redis
                UserUtils.Provider.EmptyUser(user.UserId);

                joResult["code"] = 1;
                joResult["message"] = "成功";
                Response.Write(joResult.ToJson());
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                joResult["code"] = 0;
                joResult["message"] = ex.Message;
                Response.Write(joResult.ToJson());
            }
            finally
            {
                Response.End();
            }
        }
        #endregion

        #region 获取变更用户信息
        /// <summary>
        /// 获取变更用户信息
        /// </summary>
        [HttpPost]
        public void GetUserInfo()
        {
            JObject joResult = new JObject();
            try
            {
                #region 接口权限               
                string clientSessionId = Request["sessionId"] == null ? "" : Request["sessionId"].ToString();//客户端sessionid
                string code = Request["code"] == null ? "" : Request["code"].ToString();//客户端防伪标识

                if (!ClientUtils.Provider.ApiIdentifyCheck(clientSessionId, code))
                {
                    throw new Exception("请求接口仅已登录客户端可调用！");
                }

                string service = Request["service"] == null ? "" : Request["service"].ToString();//客户端
                ClientEntity clientEntity = ClientUtils.Provider.GetClient(service);

                ClientUtils.Provider.isApiAuth(clientEntity);
                #endregion

                string account = Request["account"] == null ? "" : Request["account"].ToString();//用户账号
                string updateTime = Request["updateTime"] == null ? "" : Request["updateTime"].ToString();//时间戳

                List<HttpModel> paras = new List<HttpModel>();
                //paras.Add(new HttpModel("account", account));
                paras.Add(new HttpModel("updateTime", updateTime));
                string url = GrobalConfig.ManageApi + "/api/GetUserInfo/";
                string userInfo = HttpUtils.RestRequest(url, Method.GET, paras, null);
                JObject joUserInfo = JObject.Parse(userInfo);
                if (joUserInfo["message"] != null)
                {
                    throw new Exception(joUserInfo["message"].ToString());
                }
                joResult["code"] = 1;
                joResult["message"] = "成功";
                joResult["data"] = joUserInfo;
                Response.Write(joResult.ToJson());
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                joResult["code"] = 0;
                joResult["message"] = ex.Message;
                joResult["data"] = new JArray();
                Response.Write(joResult.ToJson());
            }
            finally
            {
                Response.End();
            }
        }
        #endregion

        #endregion
    }
}
