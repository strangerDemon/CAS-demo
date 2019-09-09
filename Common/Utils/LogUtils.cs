using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Web;
using ZoneTop.Application.SSO.Common.Grobal;
using ZoneTop.Application.SSO.Common.Model;

namespace ZoneTop.Application.SSO.Common.Utils
{
    /// <summary>
    /// 日志初始化
    /// </summary>
    public class LogUtils
    {
        #region 构建
        static LogUtils()
        {
            FileInfo configFile = new FileInfo(HttpContext.Current.Server.MapPath("/Configs/Xml/log4net.config"));
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static LogModel GetLogger(Type type)
        {
            return new LogModel(LogManager.GetLogger(type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static LogModel GetLogger(string str)
        {
            return new LogModel(LogManager.GetLogger(str));
        }

        #endregion

        #region 写日志

        #region Error错误日志
        /// <summary>
        /// log4net 自定义错误日志格式
        /// </summary>
        /// <param name="log"></param>
        /// <param name="exception"></param>
        public static void myError(LogModel log, Exception exception)
        {
            if (GrobalConfig.IsLog)
            {
                Exception Error = exception;
                LogMessageModel logMessage = new LogMessageModel();
                logMessage.OperationTime = DateTime.Now;
                logMessage.Url = HttpContext.Current.Request.RawUrl;
                logMessage.Class = log.GetType().ToString();
                logMessage.Ip = WebHelperUtils.Ip;
                logMessage.Host = WebHelperUtils.Host;
                logMessage.Browser = WebHelperUtils.Browser;
                try
                {
                    logMessage.UserName = UserUtils.Provider.Current().Account + "（" + UserUtils.Provider.Current().UserName + "）";
                }
                catch (Exception excep)
                {
                    logMessage.UserName = excep.ToString();
                }

                if (Error.InnerException == null)
                {
                    logMessage.ExceptionInfo = Error.Message;
                }
                else
                {
                    logMessage.ExceptionInfo = Error.InnerException.Message;
                }
                logMessage.ExceptionSource = Error.Source;
                logMessage.ExceptionRemark = Error.StackTrace;
                string strMessage = LogFormatModel.ExceptionFormat(logMessage);
                log.Error(strMessage);
            }
        }
        #endregion

        #region Debug 调试日志
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="debug"></param>
        public static void myDebug(LogModel log, JObject debug)
        {
            if (GrobalConfig.IsLog)
            {
                LogMessageModel logMessage = new LogMessageModel();
                logMessage.OperationTime = DateTime.Now;
                logMessage.Url = HttpContext.Current.Request.RawUrl;
                logMessage.Class = log.GetType().ToString();
                logMessage.Ip = WebHelperUtils.Ip;
                logMessage.Host = WebHelperUtils.Host;
                logMessage.Browser = WebHelperUtils.Browser;
                try
                {
                    logMessage.UserName = UserUtils.Provider.Current().Account + "（" + UserUtils.Provider.Current().UserName + "）";
                }
                catch (Exception excep)
                {
                    logMessage.UserName = excep.ToString();
                }
                if (debug != null)
                {
                    foreach (JProperty jp in debug.Properties())
                    {
                        logMessage.Content += "\t" + jp.Name + " : " + jp.Value + "\r\n";
                    }
                }
                string strMessage = LogFormatModel.DebugFormat(logMessage);
                log.Debug(strMessage);
            }
        }
        #endregion

        #region Info 信息日志
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="info"></param>
        public static void myInfo(LogModel log, JObject info)
        {
            if (GrobalConfig.IsLog)
            {
                LogMessageModel logMessage = new LogMessageModel();
                logMessage.OperationTime = DateTime.Now;
                logMessage.Url = HttpContext.Current.Request.RawUrl;
                logMessage.Class = log.GetType().ToString();
                logMessage.Ip = WebHelperUtils.Ip;
                logMessage.Host = WebHelperUtils.Host;
                logMessage.Browser = WebHelperUtils.Browser;
                try
                {
                    logMessage.UserName = UserUtils.Provider.Current().Account + "（" + UserUtils.Provider.Current().UserName + "）";
                }
                catch (Exception excep)
                {
                    logMessage.UserName = excep.ToString();
                }

                if (info != null)
                {
                    foreach (JProperty jp in info.Properties())
                    {
                        logMessage.Content += "\t" + jp.Name + " : " + jp.Value + "\r\n";
                    }
                }
                string strMessage = LogFormatModel.InfoFormat(logMessage);
                log.Info(strMessage);
            }
        }
        #endregion

        #region Warn 警告日志
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="warn"></param>
        public static void myWarn(LogModel log, JObject warn)
        {
            if (GrobalConfig.IsLog)
            {
                LogMessageModel logMessage = new LogMessageModel();
                logMessage.OperationTime = DateTime.Now;
                logMessage.Url = HttpContext.Current.Request.RawUrl;
                logMessage.Class = log.GetType().ToString();
                logMessage.Ip = WebHelperUtils.Ip;
                logMessage.Host = WebHelperUtils.Host;
                logMessage.Browser = WebHelperUtils.Browser;
                try
                {
                    logMessage.UserName = UserUtils.Provider.Current().Account + "（" + UserUtils.Provider.Current().UserName + "）";
                }
                catch (Exception excep)
                {
                    logMessage.UserName = excep.ToString();
                }

                if (warn != null)
                {
                    foreach (JProperty jp in warn.Properties())
                    {
                        logMessage.Content += "\t" + jp.Name + " : " + jp.Value + "\r\n";
                    }
                }
                string strMessage = LogFormatModel.WarnFormat(logMessage);
                log.Warn(strMessage);
            }
        }
        #endregion

        #endregion

    }
}
