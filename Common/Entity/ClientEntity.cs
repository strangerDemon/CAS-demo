using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoneTop.Application.SSO.Common.Utils;

namespace ZoneTop.Application.SSO.Common.Entity
{
    /// <summary>
    /// 数据库 客户端类
    /// </summary>
    [Table("T_App")]
    public class ClientEntity
    {
        #region 实体成员
        /// <summary>
        /// 应用主键
        /// </summary>  
        [Key]
        public string AppId { get; set; }
        /// <summary>
        /// 应用配置ID
        /// </summary>      
        public string AppSetId { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>      
        public string AppName { get; set; }
        /// <summary>
        /// 应用地址
        /// </summary>      
        public string AppSvcUrl { get; set; }
        /// <summary>
        /// 单点登出，0/1
        /// </summary>      
        public int SingleLogin { get; set; }
        /// <summary>
        /// 登出URL 必须是前端(前后端分离也必须是前端页面，否则客户端无法获取session）
        /// </summary>      
        public string LogoutUrl { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>      
        public string Manager { get; set; }
        /// <summary>
        /// 负责人联系电话
        /// </summary>      
        public string ManagerTel { get; set; }
        /// <summary>
        /// 应用平台：
        /// </summary>      
        public string AppPlatform { get; set; }
        /// <summary>
        /// 应用缩略图
        /// </summary>      
        public string AppThumb { get; set; }
        /// <summary>
        /// 应用图标
        /// </summary>      
        public string AppIcon { get; set; }
        /// <summary>
        ///应用服务器操作系统：Windows、Linux
        /// </summary>      
        public string AppServerSystem { get; set; }
        /// <summary>
        /// 应用服务器IP
        /// </summary>      
        public string AppServerIP { get; set; }
        /// <summary>
        /// 是否接入单点登录:0否1是
        /// </summary>      
        public int IsJoinSso { get; set; }
        /// <summary>
        /// 是否默认展示的客户端:0否1是
        /// </summary>      
        public int IsDefaultApp { get; set; }
        /// <summary>
        /// Token
        /// </summary>      
        public string Token { get; set; }
        /// <summary>
        /// 接口权限:0否1是
        /// </summary>      
        public int ApiAuth { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>      
        public int SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>      
        public int DeleteMark { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>      
        public int EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>      
        public string Description { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>      
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>      
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>      
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>      
        public DateTime ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>      
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>      
        public string ModifyUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.AppId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = UserUtils.Provider.Current().UserId;
            this.CreateUserName = UserUtils.Provider.Current().UserName;
            this.DeleteMark = 0;
            this.EnabledMark = 1;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.AppId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = UserUtils.Provider.Current().UserId;
            this.ModifyUserName = UserUtils.Provider.Current().UserName;
        }
        #endregion
    }
}
