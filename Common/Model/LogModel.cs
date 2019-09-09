using log4net;

namespace ZoneTop.Application.SSO.Common.Model
{
    /// <summary>
    /// 日志
    /// </summary>
    public class LogModel
    {
        private ILog logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public LogModel(ILog log)
        {
            this.logger = log;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Debug(object message)
        {
            this.logger.Debug(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Error(object message)
        {
            this.logger.Error(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            this.logger.Info(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Warn(object message)
        {
            this.logger.Warn(message);
        }
    }
}
