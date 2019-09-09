using System.Web.Mvc;
using ZoneTop.Application.SSO.Common.Enums;

namespace ZoneTop.Application.SSO
{
    /// <summary>
    /// 拦截组件
    /// </summary>
    public class HandlerAuthorizeAttribute : ActionFilterAttribute
    {
        private PermissionMode _customMode;
        /// <summary>默认构造</summary>
        /// <param name="Mode">认证模式</param>
        public HandlerAuthorizeAttribute(PermissionMode Mode)
        {
            _customMode = Mode;
        }
        /// <summary>
        /// 权限认证
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //是否忽略
            if (_customMode == PermissionMode.Ignore)
            {
                return;
            }
        }
    }
}
