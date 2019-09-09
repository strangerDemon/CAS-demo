using System.Web.Mvc;
using ZoneTop.Application.SSO.Common.Enums;
using ZoneTop.Application.SSO.Common.Model;
using ZoneTop.Application.SSO.Common.Utils;

namespace ZoneTop.Application.SSO
{
    /// <summary>
    /// 错误日志
    /// </summary>
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        private LogModel log = LogUtils.GetLogger("HandlerErrorAttribute");
        /// <summary>
        /// 控制器方法中出现异常，会调用该方法捕获异常
        /// </summary>
        /// <param name="context">提供使用</param>
        public override void OnException(ExceptionContext context)
        {
            WriteLog(context);
            base.OnException(context);
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            context.Result = new ContentResult { Content = new AjaxResult { type = ResultType.error, message = context.Exception.Message }.ToJson() };
        }
        /// <summary>
        /// 写入日志（log4net）
        /// </summary>
        /// <param name="context">提供使用</param>
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            //if (UserUtils.Provider.IsOverdue())
            //    return;
            LogUtils.myError(log, context.Exception);
        }
    }
}
