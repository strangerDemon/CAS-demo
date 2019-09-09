using System;
using System.Collections.Generic;
using System.Web;
using ZoneTop.Application.SSO.Common.Entity;
using ZoneTop.Application.SSO.Common.Grobal;
using ZoneTop.Application.SSO.Common.Model;

namespace ZoneTop.Application.SSO.Common.Utils
{
    /// <summary>
    /// 用户工具类
    /// </summary>
    public class UserUtils
    {
        //log4
        private LogModel log = LogUtils.GetLogger("UserUtils");

        /// <summary>
        /// 秘钥
        /// </summary>
        private string LoginUserKey = GrobalConfig.LoginUserKey;

        #region 静态实例
        /// <summary>
        /// 当前提供者
        /// </summary>
        public static UserUtils Provider
        {
            get { return new UserUtils(); }
        }
        #endregion

        #region 用户

        #region 公开用户，通过sessionid，userid获取用户
        /// <summary>
        /// 通过sessionid获取用户
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public virtual UserModel GetUser(string sessionId)
        {
            try
            {
                RedisCacheUtils.Select(1);
                return RedisCacheUtils.Get<UserModel>(sessionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过用户id获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual UserModel GetUserByUserId(string userId)
        {
            string sessionid = UserSession(userId);
            if (sessionid == null || sessionid.Equals(""))
            {
                throw new Exception("未查询到用户登录信息！");
            }
            return GetUser(sessionid);
        }

        /// <summary>
        /// 修改user
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="user"></param>
        public virtual void UpdateUser(string sessionId, UserModel user)
        {
            try
            {
                //修改redis
                RedisCacheUtils.Select(0);
                RedisCacheUtils.Set(GrobalConfig.RedisUserTitle + user.UserId, sessionId, null);
                RedisCacheUtils.Select(1);
                RedisCacheUtils.Set(sessionId, user, null);
                //todo:修改session 暂时无法修改其他的session
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除登录信息
        /// </summary>
        /// <param name="userId"></param>
        public virtual void EmptyUser(string userId)
        {
            try
            {
                string userIdKey = GrobalConfig.RedisUserTitle + userId;

                RedisCacheUtils.Select(0);
                string otherSessionId = RedisCacheUtils.Get<string>(userIdKey);
                RedisCacheUtils.Remove(userIdKey);

                RedisCacheUtils.Select(1);
                RedisCacheUtils.Remove(otherSessionId);
                //todo:clear other session

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 当前账号在他地登录session 是否已经登录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual string UserSession(string userId)
        {
            try
            {
                RedisCacheUtils.Select(0);
                return RedisCacheUtils.Get<string>(GrobalConfig.RedisUserTitle + userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 当前用户

        #region session
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string getCurrentSession()
        {
            return HttpContext.Current.Session.SessionID;
        }
        #endregion

        #region current 
        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public virtual UserModel Current()
        {
            try
            {
                string sessionId = getCurrentSession();
                RedisCacheUtils.Select(1);
                return RedisCacheUtils.Get<UserModel>(sessionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 写入登录信息
        /// </summary>
        /// <param name="user">成员信息</param>
        public virtual void AddCurrent(UserModel user)
        {
            try
            {
                string sessionId = getCurrentSession();

                RedisCacheUtils.Select(0);
                RedisCacheUtils.Set(GrobalConfig.RedisUserTitle + user.UserId, sessionId, null);

                RedisCacheUtils.Select(1);
                RedisCacheUtils.Set(sessionId, user, null);

                WebHelperUtils.WriteSession(LoginUserKey, user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新登录信息
        /// </summary>
        /// <param name="user">成员信息</param>
        public virtual void UpdateCurrent(UserModel user)
        {
            try
            {
                string sessionId = getCurrentSession();
                RedisCacheUtils.Select(0);
                RedisCacheUtils.Set(GrobalConfig.RedisUserTitle + user.UserId, sessionId, null);

                RedisCacheUtils.Select(1);
                RedisCacheUtils.Set(sessionId, user, null);

                //session 中移除旧的 添加新的
                WebHelperUtils.RemoveSession(LoginUserKey);
                WebHelperUtils.WriteSession(LoginUserKey, user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 清理用户
        /// <summary>
        /// 删除登录信息
        /// </summary>
        public virtual void EmptyCurrent()
        {
            try
            {
                EmptyCurrentRedis();
                EmptyCurrentSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 只清理redis的用户
        /// </summary>
        public virtual void EmptyCurrentRedis()
        {
            try
            {
                UserModel user = Current();
                RedisCacheUtils.Select(0);
                RedisCacheUtils.Remove(GrobalConfig.RedisUserTitle + user.UserId);
                RedisCacheUtils.Select(1);
                RedisCacheUtils.Remove(HttpContext.Current.Session.SessionID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 只清理session的客户
        /// </summary>
        public virtual void EmptyCurrentSession()
        {
            try
            {
                HttpContext.Current.Session.Contents.Remove(LoginUserKey);
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region 是否在线过期
        /// <summary>
        /// session中是否过期
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOverdue()
        {
            try
            {
                object str = HttpContext.Current.Session[LoginUserKey];

                if (str != null && str.ToString() != "")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                return true;
            }
        }

        /// <summary>
        /// redis是否已登录
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOnLine()
        {
            try
            {
                UserModel user = Current();

                return user != null;
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                return false;
            }
        }
        #endregion

        #endregion

        #region 所有用户
        /// <summary>
        /// 获取所有在线用户
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, UserModel> GetAllOnLines()
        {
            try
            {
                RedisCacheUtils.Select(1);
                IEnumerable<string> keys = RedisCacheUtils.GetAllKey();
                if (keys.ToJson().Equals("[]"))
                {
                    return null;
                }
                IDictionary<string, UserModel> users = RedisCacheUtils.GetAll<UserModel>(keys);
                return users;
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                throw ex;
            }
        }

        #endregion

        #region 数据库用户

        #region 账号密码登录        
        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual UserModel CheckLogin(string account, string password)
        {
            BaseUserEntity _entity = DataBaseUtils.GetUserEntity(account, password);
            CasLogEntity log = DataBaseUtils.casLoginLog(_entity);
            UserModel user = new UserModel();
            user.UserId = _entity.UserId;
            user.Account = _entity.Account;
            user.Password = _entity.Password;
            user.UserName = _entity.RealName;
            user.Code = _entity.EnCode;
            user.LogTime = DateTime.Now;
            user.CasLogId = log == null ? "" : log.CasLogId;
            return user;
        }
        #endregion

        #region 根据账号获取用户
        /// <summary>
        /// 根据账号获取用户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public virtual BaseUserEntity GetUserEntity(string account)
        {
            return DataBaseUtils.GetUserEntity(account, null);
        }
        #endregion

        #endregion

        #endregion
    }
}
