using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZoneTop.Application.SSO.Common.Entity
{
    /// <summary>
    /// 数据库 用户登录类
    /// </summary>
    [Table("T_SsoCasSession")]
    public class CasLogEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Key]
        public string CasLogId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        public string UserId { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        /// <returns></returns>
        public string UserAccount { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        /// <returns></returns>
        public string UserName { get; set; }
        /// <summary>
        /// 登录IP
        /// </summary>
        /// <returns></returns>
        public string IPAddress { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        /// <returns></returns>
        public DateTime? LoginTime { get; set; }
        /// <summary>
        /// 登出时间
        /// </summary>
        /// <returns></returns>
        public DateTime? LogoutTime { get; set; }
        /// <summary>
        /// 登录状态 0 登出、1登录
        /// </summary>
        /// <returns></returns>
        public int? LogStatus { get; set; }
        /// <summary>
        /// 浏览器类型
        /// </summary>
        /// <returns></returns>
        public string BrowserType { get; set; }
        /// <summary>
        /// SESSIONID
        /// </summary>
        /// <returns></returns>
        public string SESSIONID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.CasLogId = Guid.NewGuid().ToString();
            this.LoginTime = DateTime.Now;
            this.LogStatus = 1;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.CasLogId = keyValue;
        }
        #endregion
    }
}