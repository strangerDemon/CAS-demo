using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using ZoneTop.Application.SSO.Common.Entity;

namespace ZoneTop.Application.SSO.Common.Service
{
    /// <summary>
    /// 客户端日志
    /// </summary>
    public class ClientLogService
    {
        private DbConnection _connection;
        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ClientLogEntity GetClientLog(string keyValue)
        {
            using (_connection = _DbFactory.GetOpenConnection())
            {
                return _connection.Get<ClientLogEntity>(keyValue);
            }
        }

        /// <summary>
        /// 获取客户端列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientLogEntity> GetClientLogList()
        {
            throw new Exception("未实现");
        }


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
        /// 新增 客户端日志
        /// </summary>
        /// <param name="clientLogEntity">实体对象</param>
        /// <returns></returns>
        public bool Save(ClientLogEntity clientLogEntity)
        {
            using (_connection = _DbFactory.GetOpenConnection())
            {
                string query = " insert into [T_SsoAppSession] ([AppLogId],[CasLogId], [AppId], [AppName], [AppUrl], [ST], [STCreateTime], [STEFTime], [STValidated], [STValidateTime], [LoginTime], [LogoutTime], [LogStatus], [Description], [SESSIONID]) values (@AppLogId, @CasLogId, @AppId, @AppName, @AppUrl, @ST, @STCreateTime, @STEFTime, @STValidated, @STValidateTime, @LoginTime, @LogoutTime, @LogStatus, @Description, @SESSIONID)";
                return _connection.Execute(query, clientLogEntity) == 1;

                //return _connection.Insert<ClientLogEntity>(clientLogEntity) == 1;
            }
        }

        /// <summary>
        /// 修改 客户端日志
        /// </summary>
        /// <param name="clientLogEntity">实体对象</param>
        public bool Update(ClientLogEntity clientLogEntity)
        {
            using (_connection = _DbFactory.GetOpenConnection())
            {
                return _connection.Update<ClientLogEntity>(clientLogEntity) == 1;
            }
        }
        #endregion
    }
}
