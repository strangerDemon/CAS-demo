using System.Web.Mvc;
using ZoneTop.Application.SSO.Common.Enums;
using ZoneTop.Application.SSO.Common.Model;
using ZoneTop.Application.SSO.Common.Utils;

namespace ZoneTop.Application.SSO
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [HandlerLogin(LoginMode.Ignore)]
    public abstract class MvcControllerBase : Controller
    {
        private LogModel _logger;
        /// <summary>
        /// 日志操作
        /// </summary>
        public LogModel Logger
        {
            get { return _logger ?? (_logger = LogUtils.GetLogger(this.GetType().ToString())); }
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual ActionResult ToJsonResult(object data)
        {
            return Content(data.ToJson());
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { type = ResultType.success, message = message }.ToJson());
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { type = ResultType.success, message = message, resultdata = data }.ToJson());
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { type = ResultType.error, message = message }.ToJson());
        }
    }
}
