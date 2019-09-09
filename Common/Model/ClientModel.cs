using System;

namespace ZoneTop.Application.SSO.Common.Model
{
    /// <summary>
    /// 客户端工具
    /// </summary>
    public class ClientModel
    {
        /// <summary>
        /// 系统ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 系统URL
        /// </summary>
        public string ClientUrl { get; set; }

        /// <summary>
        /// 系统平台
        /// .NET JAVA PHP
        /// </summary>
        public string ClientPlatfrom { get; set; }

        /// <summary>
        /// 私有回话主键
        /// </summary>
        public string SessionIdKey { get; set; }

        /// <summary>
        /// 私有回话主键
        /// </summary>
        public string SessionIdValue { get; set; }

        /// <summary>
        /// 客户端防伪标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 票据值
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// 票据颁发时间
        /// </summary>
        public DateTime? TicketCreateTime { get; set; }

        /// <summary>
        /// 票据有效时间 以秒为单位，默认为10秒
        /// </summary>
        public int TicketEFTime { get; set; }

        /// <summary>
        /// 票据是否验证
        /// -1 未验证 0失败 1成功
        /// </summary>
        public int TicketValidated { get; set; }

        /// <summary>
        /// 票据验证时间
        /// </summary>
        public DateTime? TicketValidateTime { get; set; }

        /// <summary>
        /// 客户端日志id
        /// </summary>
        public string ClientLogId { get; set; }
    }
}
