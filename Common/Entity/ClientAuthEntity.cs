using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoneTop.Application.SSO.Common.Utils;

namespace ZoneTop.Application.SSO.Common.Entity
{
    /// <summary>
    /// 数据库 客户端 权限类
    /// </summary>
    [Table("T_AppAuth")]
    public partial class ClientAuthEntity
    {
        #region 基本属性
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 用户主键
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 应用主键
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 创建时间
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

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = UserUtils.Provider.Current().UserId;
            this.CreateUserName = UserUtils.Provider.Current().UserName;
        }
        #endregion
    }
}