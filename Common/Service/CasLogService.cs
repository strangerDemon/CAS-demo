using System;
using System.Text;
using System.Linq;
using System.Data.Common;
using System.Collections.Generic;
using ZoneTop.Application.SSO.Common.Entity;
using Dapper;

namespace ZoneTop.Application.SSO.Common.Service
{
    /// <summary>
    /// 单点登录日志
    /// </summary>
    public class CasLogService
    {
        private DbConnection _connection;

        #region 查询

        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CasLogEntity GetCasLog(string keyValue)
        {
            using (_connection = _DbFactory.GetOpenConnection())
            {
                return _connection.Get<CasLogEntity>(keyValue);
            }
        }

        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CasLogEntity> GetCasLogList()
        {
            throw new Exception("未实现");
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void Remove(string keyValue)
        {
            throw new Exception("未实现");
        }

        /// <summary>
        /// 保存cas登录日志
        /// </summary>
        /// <param name="casLogEntity">实体对象</param>
        /// <returns></returns>
        public bool Save(CasLogEntity casLogEntity)
        {
            using (_connection = _DbFactory.GetOpenConnection())
            {
                string query = "insert into [T_SsoCasSession] ([CasLogId], [UserId], [UserAccount], [UserName], [IPAddress], [LoginTime], [LogoutTime], [LogStatus], [BrowserType], [SESSIONID], [Description]) values (@CasLogId, @UserId, @UserAccount, @UserName, @IPAddress, @LoginTime, @LogoutTime, @LogStatus, @BrowserType, @SESSIONID, @Description)";
                return _connection.Execute(query, casLogEntity) == 1;

                //return _connection.Insert<CasLogEntity>(casLogEntity) == 1;
            }
        }

        /// <summary>
        /// 修改cas登录日志
        /// </summary>
        /// <param name="casLogEntity">实体对象</param>
        public bool Update(CasLogEntity casLogEntity)
        {
            using (_connection = _DbFactory.GetOpenConnection())
            {
                return _connection.Update<CasLogEntity>(casLogEntity) == 1;
            }
        }
        #endregion
    }
}
