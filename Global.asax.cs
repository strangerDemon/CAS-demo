using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZoneTop.Application.SSO.Common.Entity;
using ZoneTop.Application.SSO.Common.Grobal;
using ZoneTop.Application.SSO.Common.Model;
using ZoneTop.Application.SSO.Common.Utils;

namespace ZoneTop.Application.SSO
{
    /// <summary>
    /// 应用程序全局设置
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        private LogModel log = LogUtils.GetLogger("MvcApplication");
        /// <summary>
        /// 启动应用程序
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleTable.EnableOptimizations = true;
        }

        #region 异常退出或者正常退出都会触发该事件，主要是清除缓存
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                var a = sender.ToJson();
                string LoginProvider = GrobalConfig.LoginProvider;
                string LoginUserKey = GrobalConfig.LoginUserKey;

                if (Session[LoginUserKey] != null)
                {
                    //清理redis中的用户数据
                    UserModel user = UserUtils.Provider.Current();
                    if (user != null)
                    {
                        List<ClientEntity> clients = ClientUtils.Provider.getAllAuthClient(user.UserId).ToJson().ToList<ClientEntity>();
                        ClientUtils.Provider.LogoutAllAction(user, clients, "");
                        //清理redis
                        UserUtils.Provider.EmptyCurrentRedis();
                    }
                    //清理session中的用户数据
                    user = EncryptUtils.doDecrypt(Session[LoginUserKey].ToString()).ToObject<UserModel>();
                    if (user != null)
                    {
                        UserUtils.Provider.EmptyCurrentSession();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
            }

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string strRequestSid = "aspnetsession";
            string strSidKey = GrobalConfig.LoginUserKey;
            var strSidValue = HttpContext.Current.Request.QueryString[strRequestSid];

            if (strSidValue != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(strSidKey);

                if (null == cookie)
                {
                    cookie = new HttpCookie(strSidKey);
                }
                cookie.Value = strSidValue;
                HttpContext.Current.Request.Cookies.Set(cookie);
            }
        }

        /// <summary>
        /// todo:应用程序错误处理
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
        }
    }
}
