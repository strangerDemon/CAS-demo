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
    /// 客户端权限
    /// </summary>
    public class ClientAuthService
    {
        private DbConnection _connection;

        #region 查询

        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ClientAuthEntity GetClientAuth(string keyValue)
        {
            using (_connection = _DbFactory.GetOpenConnection())
            {
                return _connection.Get<ClientAuthEntity>(keyValue);
            }
        }

        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="account"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public IEnumerable<ClientAuthEntity> GetClientAuthList(string account, string userName)
        {
            //var user = connection.GetList<User>("where age = 10 or Name like '%Smith%'");  
            //var user = connection.GetList<User>(new { Age = 10 })
            var strSql = new StringBuilder();
            strSql.Append(" where 1=1 ");
            if (!String.IsNullOrEmpty(account))
            {
                strSql.Append(" and Account like '%" + account + "%' ");
            }
            if (!String.IsNullOrEmpty(userName))
            {
                strSql.Append(" and RealName like '%" + userName + "%' ");
            }
            using (_connection = _DbFactory.GetOpenConnection())
            {
                return _connection.GetList<ClientAuthEntity>(strSql.ToString());
            }
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
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="clientAuthEntity">实体对象</param>
        /// <returns></returns>
        public string Save(string keyValue, ClientAuthEntity clientAuthEntity)
        {
            throw new Exception("未实现");
        }


        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="clientAuthEntity">实体对象</param>
        public void Update(ClientAuthEntity clientAuthEntity)
        {
            throw new Exception("未实现");
        }
        #endregion
    }
}
