using System;
using ZoneTop.Application.SSO.Common.Utils;

namespace ZoneTop.Application.SSO.Common.Grobal
{
    /// <summary>
    /// 全局静态变量
    /// </summary>
    public static class GrobalConfig
    {
        #region 验证码
        /// <summary>
        /// 验证码
        /// </summary>
        public static string VerifyCode = "session_verifycode";
        #endregion

        #region 加密
        /// <summary>
        ///  加密方式
        /// </summary>
        public static string MD5 = "MD5";

        /// <summary>
        /// 加密长度 16
        /// </summary>
        public static int Encryption16 = 16;

        /// <summary>
        /// 加密长度 32
        /// </summary>
        public static int Encryption32 = 32;
        #endregion

        #region 单点登录

        /// <summary>
        /// 用户登录标识
        /// </summary>
        public static string LoginUserKey = "SSOLoginUserKey";

        /// <summary>
        /// 票据头
        /// </summary>
        public static string TicketTitle = "ST_";

        /// <summary>
        /// redis 用户头
        /// </summary>
        public static string RedisUserTitle = "UserId_";

        #endregion

        #region xmlConfig 的配置

        /// <summary>
        /// 是否支持本地log4日志
        /// </summary>
        public static bool IsLog = ConfigUtils.GetValue("IsLog") == "" ? true : bool.Parse(ConfigUtils.GetValue("IsLog"));

        /// <summary>
        /// 是否支持多地点登录
        /// </summary>
        public static bool IsMultiplePlace = ConfigUtils.GetValue("IsMultiplePlace") == "" ? false : bool.Parse(ConfigUtils.GetValue("IsMultiplePlace"));

        /// <summary>
        /// 是否接口身份校验
        /// </summary>
        public static bool IsApiCheck = ConfigUtils.GetValue("IsApiCheck") == "" ? true : bool.Parse(ConfigUtils.GetValue("IsApiCheck"));

        /// <summary>
        /// 关系系统接口地址
        /// </summary>
        public static string DataBaseType = ConfigUtils.GetValue("DataBaseType") == "" ? "SQLServer" : ConfigUtils.GetValue("DataBaseType");

        /// <summary>
        /// 关系系统接口地址
        /// </summary>
        public static string ManageApi = ConfigUtils.GetValue("ManageApi") == "" ? "" : ConfigUtils.GetValue("ManageApi");

        /// <summary>
        /// 能调用登出接口的客户端
        /// </summary>
        public static string LogoutClient = ConfigUtils.GetValue("LogoutClient") == "" ? "" : ConfigUtils.GetValue("LogoutClient");

        /// <summary>
        /// ticket 有效时间
        /// </summary>
        public static int TicketTimeOut = ConfigUtils.GetValue("TicketTimeOut") == "" ? 10 : int.Parse(ConfigUtils.GetValue("TicketTimeOut"));

        /// <summary>
        /// redis 超时时间
        /// </summary>
        public static long RedisTimeOut = ConfigUtils.GetValue("RedisTimeOut").Equals("") ? 10 * TimeSpan.TicksPerMinute : long.Parse(ConfigUtils.GetValue("RedisTimeOut"));

        /// <summary>
        /// 加密token
        /// </summary>
        public static string EncryptToken = ConfigUtils.GetValue("EncryptToken") == "" ? "xmtz2018" : ConfigUtils.GetValue("EncryptToken");

        /// <summary>
        /// 登录标识
        /// </summary>
        public static string LoginProvider = ConfigUtils.GetValue("LoginProvider") == "" ? "" : ConfigUtils.GetValue("LoginProvider");

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SystemName = ConfigUtils.GetValue("SystemName") == "" ? "单点登录系统" : ConfigUtils.GetValue("SystemName");

        /// <summary>
        /// 登录页面title
        /// </summary>
        public static string LoginTitle = ConfigUtils.GetValue("LoginTitle") == "" ? "单点登录系统" : ConfigUtils.GetValue("LoginTitle");

        /// <summary>
        /// 登录页面描述
        /// </summary>
        public static string LoginDesc = ConfigUtils.GetValue("LoginDesc") == "" ? "厦门市土地开发总公司" : ConfigUtils.GetValue("LoginDesc");
        #endregion
    }
}
