using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZoneTop.Application.SSO.Common.Entity
{
    /// <summary>
    /// 数据库 客户端登录日志类
    /// </summary>
    [Table("T_SsoAppSession")]
    public class ClientLogEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Key]
        public string AppLogId { get; set; }
        /// <summary>
        /// CAS会话ID
        /// </summary>
        /// <returns></returns>
        public string CasLogId { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        /// <returns></returns>
        public string AppId { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        /// <returns></returns>
        public string AppName { get; set; }
        /// <summary>
        /// 应用地址
        /// </summary>
        /// <returns></returns>
        public string AppUrl { get; set; }
        /// <summary>
        /// 票据
        /// </summary>
        /// <returns></returns>
        public string ST { get; set; }
        /// <summary>
        /// 票据创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? STCreateTime { get; set; }
        /// <summary>
        /// 票据有效时长，以秒为单位
        /// </summary>
        /// <returns></returns>
        public int? STEFTime { get; set; }
        /// <summary>
        /// 票据验证 -1 未验证 0 失败 1 成功 
        /// </summary>
        /// <returns></returns>
        public int? STValidated { get; set; }
        /// <summary>
        /// 票据验证时间
        /// </summary>
        /// <returns></returns>
        public DateTime? STValidateTime { get; set; }
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
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        /// <summary>
        /// SESSIONID
        /// </summary>
        /// <returns></returns>
        public string SESSIONID { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.AppLogId = Guid.NewGuid().ToString();
            this.LogStatus = 1;
            this.LoginTime = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.AppLogId = keyValue;
        }
        #endregion
    }
}