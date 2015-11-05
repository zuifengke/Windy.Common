// ***********************************************************
// 数据库访问基础方法类之数据库驱动类型创建工厂类
// Creator:YangMingkun  Date:2009-6-27
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Windy.Common.Libraries.DbAccess
{
    /// <summary>
    /// 数据库驱动类型创建工厂类
    /// </summary>
    internal class ProviderFactory
    {
        public static DatabaseType GetDbType(string szDbType)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDbType))
                return DatabaseType.ORACLE;
            szDbType = szDbType.Trim().ToUpper();
            if (szDbType == "SQLSERVER")
                return DatabaseType.SQLSERVER;
            else if (szDbType == "ACCESS")
                return DatabaseType.ACCESS;
            else if (szDbType == "MYSQL")
                return DatabaseType.MYSQL;
            else
                return DatabaseType.ORACLE;
        }

        public static DataProvider GetDataProvider(string szDataProvider)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDataProvider))
                return DataProvider.OleDb;
            szDataProvider = szDataProvider.Trim().ToUpper();
            if (szDataProvider == "ODBC")
                return DataProvider.Odbc;
            else if (szDataProvider == "SQLCLIENT")
                return DataProvider.SqlClient;
            else if (szDataProvider == "ORACLECLIENT")
                return DataProvider.OracleClient;
            else if (szDataProvider == "ODPNET")
                return DataProvider.ODPNET;
            else if (szDataProvider == "MYSQLCLIENT")
                return DataProvider.MySqlClient;
            else
                return DataProvider.OleDb;
        }

        public static IDbDataAdapter GetAdapter(DataProvider provider)
        {
            IDbDataAdapter adapter = null;
            switch (provider)
            {
                case DataProvider.Odbc:
                    return new OdbcDataAdapter();
                case DataProvider.SqlClient:
                    return new SqlDataAdapter();
                case DataProvider.OleDb:
                    return new OleDbDataAdapter();
                case DataProvider.OracleClient:
                    return new System.Data.OracleClient.OracleDataAdapter();
                case DataProvider.ODPNET:
                    return new Oracle.DataAccess.Client.OracleDataAdapter();
                case DataProvider.MySqlClient:
                    return new MySql.Data.MySqlClient.MySqlDataAdapter();

            }
            return adapter;
        }

        public static IDbCommand GetCommand(DataProvider provider)
        {
            IDbCommand command = null;
            switch (provider)
            {
                case DataProvider.Odbc:
                    return new OdbcCommand();
                case DataProvider.SqlClient:
                    return new SqlCommand();
                case DataProvider.OleDb:
                    return new OleDbCommand();
                case DataProvider.OracleClient:
                    return new System.Data.OracleClient.OracleCommand();
                case DataProvider.ODPNET:
                    return new Oracle.DataAccess.Client.OracleCommand();
                case DataProvider.MySqlClient:
                    return new MySql.Data.MySqlClient.MySqlCommand();
            }
            return command;
        }

        public static IDbCommand GetCommand(string sql, CommandType cmdType, int timeout
                , DbParameter[] parameters, DataProvider provider)
        {
            IDbCommand command = GetCommand(provider);
            if (parameters != null)
            {
                for (int index = 0; index < parameters.Length; index++)
                {
                    DbParameter parameter = parameters[index];
                    if (parameter == null)
                        continue;
                    IDbDataParameter param = GetParameter(parameter.Name
                        , parameter.Direction
                        , parameter.Value
                        , parameter.DataType
                        , parameter.SourceColumn
                        , (short)parameter.Size, provider);
                    command.Parameters.Add(param);
                }
            }
            command.CommandType = cmdType;
            command.CommandText = sql;
            command.CommandTimeout = timeout;
            return command;
        }

        public static IDbConnection GetConnection(DataProvider provider)
        {
            IDbConnection connection = null;
            switch (provider)
            {
                case DataProvider.Odbc:
                    return new OdbcConnection();
                case DataProvider.SqlClient:
                    return new SqlConnection();
                case DataProvider.OleDb:
                    return new OleDbConnection();
                case DataProvider.OracleClient:
                    return new System.Data.OracleClient.OracleConnection();
                case DataProvider.ODPNET:
                    return new Oracle.DataAccess.Client.OracleConnection();
                case DataProvider.MySqlClient:
                    return new MySql.Data.MySqlClient.MySqlConnection();
            }
            return connection;
        }

        public static IDbConnection GetConnection(string connString, DataProvider provider)
        {
            IDbConnection connection = GetConnection(provider);
            connection.ConnectionString = connString;
            return connection;
        }

        public static IDbTransaction GetTransaction(ref IDbConnection conn, IsolationLevel isolationLevel)
        {
            return conn.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// 获取数据库数据字段类型
        /// </summary>
        /// <param name="provider">数据提供者</param>
        /// <param name="dbType">基本数据类型</param>
        /// <returns>CommonConst.ReturnValue</returns>
        public static short GetParameterType(DataProvider provider, DbType dbType)
        {
            if (provider == DataProvider.SqlClient)
            {
                if (dbType == DbType.String)
                    return (short)System.Data.SqlDbType.Text;
                else if (dbType == DbType.Binary)
                    return (short)System.Data.SqlDbType.Image;
                else if (dbType == DbType.DateTime)
                    return (short)System.Data.SqlDbType.DateTime;
                else if (dbType == DbType.Int32)
                    return (short)System.Data.SqlDbType.Int;
                else if (dbType == DbType.Single)
                    return (short)System.Data.SqlDbType.Float;
            }
            else if (provider == DataProvider.ODPNET)
            {
                if (dbType == DbType.String)
                    return (short)Oracle.DataAccess.Client.OracleDbType.Varchar2;
                else if (dbType == DbType.Binary)
                    return (short)Oracle.DataAccess.Client.OracleDbType.Blob;
                else if (dbType == DbType.DateTime)
                    return (short)Oracle.DataAccess.Client.OracleDbType.TimeStamp;
                else if (dbType == DbType.Int32)
                    return (short)Oracle.DataAccess.Client.OracleDbType.Int32;
                else if (dbType == DbType.Single)
                    return (short)Oracle.DataAccess.Client.OracleDbType.Single;
            }
            else if (provider == DataProvider.MySqlClient)
            {
                if (dbType == DbType.String)
                    return (short)MySql.Data.MySqlClient.MySqlDbType.VarString;
                else if (dbType == DbType.Binary)
                    return (short)MySql.Data.MySqlClient.MySqlDbType.LongBlob;
                else if (dbType == DbType.DateTime)
                    return (short)MySql.Data.MySqlClient.MySqlDbType.Time;
                else if (dbType == DbType.Int32)
                    return (short)MySql.Data.MySqlClient.MySqlDbType.Int32;
                else if (dbType == DbType.Single)
                    return (short)MySql.Data.MySqlClient.MySqlDbType.Float;
            }
            else if (provider == DataProvider.OracleClient)
            {
                if (dbType == DbType.String)
                    return (short)System.Data.OracleClient.OracleType.LongVarChar;
                else if (dbType == DbType.Binary)
                    return (short)System.Data.OracleClient.OracleType.Blob;
                else if (dbType == DbType.DateTime)
                    return (short)System.Data.OracleClient.OracleType.DateTime;
                else if (dbType == DbType.Int32)
                    return (short)System.Data.OracleClient.OracleType.Int32;
                else if (dbType == DbType.Single)
                    return (short)System.Data.OracleClient.OracleType.Float;
            }
            
            else if (provider == DataProvider.Odbc)
            {
                if (dbType == DbType.String)
                    return (short)System.Data.Odbc.OdbcType.Text;
                else if (dbType == DbType.Binary)
                    return (short)System.Data.Odbc.OdbcType.Image;
                else if (dbType == DbType.DateTime)
                    return (short)System.Data.Odbc.OdbcType.DateTime;
                else if (dbType == DbType.Int32)
                    return (short)System.Data.Odbc.OdbcType.Int;
                else if (dbType == DbType.Single)
                    return (short)System.Data.Odbc.OdbcType.Real;
            }
            else
            {
                if (dbType == DbType.String)
                    return (short)System.Data.OleDb.OleDbType.LongVarChar;
                else if (dbType == DbType.Binary)
                    return (short)System.Data.OleDb.OleDbType.LongVarBinary;
                else if (dbType == DbType.DateTime)
                    return (short)System.Data.OleDb.OleDbType.DBTimeStamp;
                else if (dbType == DbType.Int32)
                    return (short)System.Data.OleDb.OleDbType.Integer;
                else if (dbType == DbType.Single)
                    return (short)System.Data.OleDb.OleDbType.Single;
            }
            throw new Exception(string.Format("非法的数据类型:{0}", dbType.ToString()));
        }

        public static IDbDataParameter GetParameter(DataProvider provider)
        {
            IDbDataParameter parameter = null;
            switch (provider)
            {
                case DataProvider.Odbc:
                    return new OdbcParameter();
                case DataProvider.SqlClient:
                    return new SqlParameter();
                case DataProvider.OleDb:
                    return new OleDbParameter();
                case DataProvider.OracleClient:
                    return new System.Data.OracleClient.OracleParameter();
                case DataProvider.ODPNET:
                    return new Oracle.DataAccess.Client.OracleParameter();
                case DataProvider.MySqlClient:
                    return new MySql.Data.MySqlClient.MySqlParameter();
            }
            return parameter;
        }

        public static IDbDataParameter GetParameter(string name, ParameterDirection direction, object value
                , DbType dbType, string sourceColumn, short size, DataProvider provider)
        {
            IDbDataParameter parameter = GetParameter(provider);
            parameter.ParameterName = name;

            //设置参数数据类型
            short shType = GetParameterType(provider, dbType);
            if (provider == DataProvider.SqlClient)
                (parameter as SqlParameter).SqlDbType = (SqlDbType)shType;
            else if (provider == DataProvider.OracleClient)
                (parameter as System.Data.OracleClient.OracleParameter).OracleType = (System.Data.OracleClient.OracleType)shType;
            else if (provider == DataProvider.Odbc)
                (parameter as OdbcParameter).OdbcType = (OdbcType)shType;
            else if (provider == DataProvider.OleDb)
                (parameter as OleDbParameter).OleDbType = (OleDbType)shType;
            else if (provider == DataProvider.ODPNET)
                (parameter as Oracle.DataAccess.Client.OracleParameter).OracleDbType = (Oracle.DataAccess.Client.OracleDbType)shType;
            else if (provider == DataProvider.MySqlClient)
                (parameter as MySql.Data.MySqlClient.MySqlParameter).MySqlDbType = (MySql.Data.MySqlClient.MySqlDbType)shType;
            else
                parameter.DbType = (DbType)shType;

            if (size > 0) parameter.Size = size;
            if (value != null) parameter.Value = value;
            parameter.Direction = direction;
            if (!GlobalMethods.Misc.IsEmptyString(sourceColumn)) parameter.SourceColumn = sourceColumn;
            return parameter;
        }
    }
}
