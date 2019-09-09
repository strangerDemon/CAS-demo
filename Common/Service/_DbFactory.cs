using MySql.Data.MySqlClient;
using Oracle.DataAccess.Client;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using ZoneTop.Application.SSO.Common.Grobal;
using ZoneTop.Application.SSO.Common.Model;
using ZoneTop.Application.SSO.Common.Utils;

namespace ZoneTop.Application.SSO.Common.Service
{
    /// <summary>
    /// 数据库建立工厂
    /// </summary>
    public class _DbFactory
    {
        private static LogModel log = LogUtils.GetLogger("_DbFactory");

        private static readonly ConnectionStringSettings Connection = ConfigurationManager.ConnectionStrings[GrobalConfig.DataBaseType];
        private static readonly string ConnectionString = Connection.ConnectionString;

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetOpenConnection()
        {
            switch (GrobalConfig.DataBaseType)
            {
                case "Oracle":
                    return OracleConnection();
                case "MySQL":
                    return MySQLConnection();
                case "SQLServer":
                default:
                    return SQLConnection();
            }
        }

        /// <summary>
        /// SQL Server 的连接
        /// </summary>
        /// <returns></returns>
        private static SqlConnection SQLConnection()
        {
            try
            {
                var connection = new SqlConnection(ConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                throw ex;
            }
        }

        /// <summary>
        /// MySQL 的连接
        /// </summary>
        /// <returns></returns>
        private static MySqlConnection MySQLConnection()
        {
            try
            {
                var connection = new MySqlConnection(ConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Oracle 的连接
        /// </summary>
        /// <returns></returns>
        private static OracleConnection OracleConnection()
        {
            try
            {
                var connection = new OracleConnection(ConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                throw ex;
            }
        }
    }
}
