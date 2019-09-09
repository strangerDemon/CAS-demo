using System;
namespace ZoneTop.Application.SSO.Common.Model
{
    /// <summary>
    /// 日志消息
    /// </summary>
    public class LogMessageModel
    {
        /// <summary>
        /// 
        /// </summary>
        public LogMessageModel()
        {
            this.OperationTime = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">位置</param>
        /// <param name="method">方法名</param>
        /// <param name="message">内容</param>
        public LogMessageModel(string url, string method, string message)
        {
            this.OperationTime = DateTime.Now;
            this.Url = url;
            this.Class = method;
            this.Content = message;
        }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }
        /// <summary>
        /// Url地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string Class { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionInfo { get; set; }
        /// <summary>
        /// 异常来源
        /// </summary>
        public string ExceptionSource { get; set; }
        /// <summary>
        /// 异常信息备注
        /// </summary>
        public string ExceptionRemark { get; set; }
    }
}
