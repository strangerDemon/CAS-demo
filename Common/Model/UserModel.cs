using System;
using System.Collections.Generic;

namespace ZoneTop.Application.SSO.Common.Model
{
    /// <summary>
    /// 当前操作者信息类
    /// </summary>
    [Serializable]
    public class UserModel
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登陆账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// cas 日志
        /// </summary>
        public string CasLogId { get; set; }

        /// <summary>
        /// 登陆的应用系统列表
        /// </summary>
        public List<ClientModel> Clients { get; set; }
    }
}
