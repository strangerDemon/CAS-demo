using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ZoneTop.Application.SSO.Common.Entity;

namespace ZoneTop.Application.SSO.Common.Service
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class ClientService
    {
        private DbConnection _connection;

        #region 查询

        /// <summary>
        /// 客户端实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ClientEntity GetEntity(string keyValue)
        {
            using (_connection = _DbFactory.GetOpenConnection())
            {
                return _connection.Get<ClientEntity>(keyValue);
            }
        }

        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientEntity> GetClientList()
        {
            //var user = connection.GetList<User>("where age = 10 or Name like '%Smith%'");  
            //var user = connection.GetList<User>(new { Age = 10 })
            var strSql = new StringBuilder();
            strSql.Append(" where 1 = 1 and DeleteMark = 0 and EnabledMark = 1 ");
            using (_connection = _DbFactory.GetOpenConnection())
            {
                return _connection.GetList<ClientEntity>(strSql.ToString());
            }
        }

        /// <summary>
        /// 获取有权限的客户端列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<ClientEntity> GetAuthClientList(string userId)
        {
            //var user = connection.GetList<User>("where age = 10 or Name like '%Smith%'");  
            //var user = connection.GetList<User>(new { Age = 10 })
            var strSql = new StringBuilder();
            strSql.Append(" select t.* from T_App t ");
            strSql.Append(" where 1 = 1 and t.DeleteMark = 0 and t.EnabledMark = 1 ");//有效
            strSql.Append(" and  t.IsJoinSso = 1 ");//接入单点
            if (!String.IsNullOrEmpty(userId))//用户权限
            {
                strSql.Append(" and t.AppId in (select AppId from T_AppAuth where UserId = '" + userId + "')");
            }
            using (_connection = _DbFactory.GetOpenConnection())
            {
                return _connection.GetList<ClientEntity>(strSql.ToString());
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
        /// <param name="clientEntity">实体对象</param>
        /// <returns></returns>
        public string Save(string keyValue, ClientEntity clientEntity)
        {
            throw new Exception("未实现");
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="clientEntity">实体对象</param>
        public void Update(ClientEntity clientEntity)
        {
            throw new Exception("未实现");
        }
        #endregion
    }
}
