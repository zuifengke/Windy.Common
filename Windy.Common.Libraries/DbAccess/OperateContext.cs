// ***********************************************************
// 数据库访问基础方法类之数据库操作上下文对象
// 主要为支持多线程访问同一个DataAccess类实例而设置
// Creator:YangMingkun  Date:2011-12-7
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.SqlClient;

namespace Windy.Common.Libraries.DbAccess
{
    /// <summary>
    /// 数据库操作上下文对象
    /// </summary>
    internal class OperateContext : IDisposable
    {
        private DatabaseType m_dbType = DatabaseType.ORACLE;
        private DataProvider m_provider = DataProvider.OleDb;
        private string m_connString = null;

        private string m_identifier = null;

        private IDbTransaction m_transaction = null;
        private IDbConnection m_connection = null;
        private IDataReader m_dataReader = null;
        private bool m_bNeedCommit = false;
        private short m_timeOut = 120;

        public string Identifier
        {
            get { return this.m_identifier; }
        }

        public short TimeOut
        {
            get { return this.m_timeOut; }
            set { this.m_timeOut = value; }
        }

        private bool m_bClearPoolEnabled = true;
        /// <summary>
        /// 获取或设置当出现ORA-12571错误时,
        /// 是否允许执行清空缓存连接操作
        /// </summary>
        public bool ClearPoolEnabled
        {
            get { return this.m_bClearPoolEnabled; }
            set { this.m_bClearPoolEnabled = value; }
        }

        public event EventHandler ConnectionClosed;
        protected virtual void OnConnectionClosed(EventArgs e)
        {
            if (this.ConnectionClosed != null)
                this.ConnectionClosed(this, e);
        }

        public OperateContext(string identifier
            , DatabaseType dbType, DataProvider provider, string connString)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("identifier");
            this.m_identifier = identifier;
            this.m_dbType = dbType;
            this.m_provider = provider;
            this.m_connString = connString;
        }

        public override string ToString()
        {
            return this.m_identifier;
        }

        public void Dispose()
        {
            this.CloseConnnection(false);
            this.DisposeTransaction(true);
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void OpenConnnection()
        {
            if (this.m_connection == null)
                this.m_connection = ProviderFactory.GetConnection(this.m_connString, this.m_provider);
            if (this.m_connection.State == ConnectionState.Open)
                return;
            try
            {
                this.m_bNeedCommit = false;
                this.m_connection.Open();
            }
            catch (Exception ex)
            {
                this.CleanConnnectionPool();
                throw ex;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="bCheckTransaction">是否检查事务正在执行</param>
        public void CloseConnnection(bool bCheckTransaction)
        {
            // 如果处在事务处理过程中，则取消关闭
            if (bCheckTransaction && this.IsInTransaction())
                return;

            // 关闭DataReader
            if (this.m_dataReader != null)
            {
                if (!this.m_dataReader.IsClosed)
                    this.m_dataReader.Close();
                this.m_dataReader.Dispose();
                this.m_dataReader = null;
            }

            // 关闭Connection
            if (this.m_connection == null)
                return;
            if (this.m_connection.State != ConnectionState.Closed)
                this.m_connection.Close();
            this.m_connection.Dispose();
            this.m_connection = null;

            // 未更新过数据库
            this.m_bNeedCommit = false;
            this.OnConnectionClosed(EventArgs.Empty);
        }

        /// <summary>
        /// 清理连接池中的各种对象
        /// </summary>
        public void CleanConnnectionPool()
        {
            if (this.m_connection == null)
                return;
            if (this.m_connection is OleDbConnection)
            {
                OleDbConnection.ReleaseObjectPool();
            }
            else if (this.m_connection is OdbcConnection)
            {
                OdbcConnection.ReleaseObjectPool();
            }
            else if (this.m_connection is SqlConnection)
            {
                SqlConnection.ClearPool((SqlConnection)this.m_connection);
            }
            if (this.m_connection is Oracle.DataAccess.Client.OracleConnection)
            {
                Oracle.DataAccess.Client.OracleConnection.ClearPool(
                    (Oracle.DataAccess.Client.OracleConnection)this.m_connection);
            }
            if (this.m_connection is System.Data.OracleClient.OracleConnection)
            {
                System.Data.OracleClient.OracleConnection.ClearPool(
                    (System.Data.OracleClient.OracleConnection)this.m_connection);
            }
            if (this.m_connection is MySql.Data.MySqlClient.MySqlConnection)
            {
                MySql.Data.MySqlClient.MySqlConnection.ClearPool(
                                   (MySql.Data.MySqlClient.MySqlConnection)this.m_connection);
            }
               
        }

        /// <summary>
        /// 当前的数据库访问是否处于一个事务中
        /// </summary>
        /// <returns>bool</returns>
        private bool IsInTransaction()
        {
            return this.m_transaction == null ? false : true;
        }

        /// <summary>
        /// 开始数据库事务
        /// </summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        /// <returns>bool</returns>
        public bool BeginTransaction(IsolationLevel isolationLevel)
        {
            try
            {
                this.OpenConnnection();
                this.m_transaction = ProviderFactory.GetTransaction(ref this.m_connection, isolationLevel);
                this.m_bNeedCommit = false;
                return true;
            }
            catch (Exception ex)
            {
                this.DisposeTransaction(true);
                LogManager.Instance.WriteLog("OperateContext.BeginTransaction", ex);
                return false;
            }
        }

        /// <summary>
        /// 提交数据库事务
        /// </summary>
        /// <returns>bool</returns>
        public bool CommitTransaction()
        {
            return this.CommitTransaction(true);
        }

        /// <summary>
        /// 提交数据库事务
        /// </summary>
        /// <param name="bCloseConnection">是否关闭连接</param>
        /// <returns>bool</returns>
        public bool CommitTransaction(bool bCloseConnection)
        {
            try
            {
                if (this.m_bNeedCommit)
                    this.m_transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("OperateContext.CommitTransaction", ex);
                return false;
            }
            finally
            {
                this.DisposeTransaction(bCloseConnection);
            }
        }

        /// <summary>
        /// 中止数据库事务
        /// </summary>
        public void AbortTransaction()
        {
            if (this.IsInTransaction())
            {
                try
                {
                    if (this.m_bNeedCommit)//如果更新过数据
                        this.m_transaction.Rollback();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("OperateContext.AbortTransaction", ex);
                }
                this.DisposeTransaction(true);
            }
        }

        /// <summary>
        /// 释放数据库事务有关对象
        /// </summary>
        /// <param name="bCloseConnection">是否关闭连接</param>
        private void DisposeTransaction(bool bCloseConnection)
        {
            if (this.m_transaction != null)
                this.m_transaction.Dispose();
            this.m_transaction = null;
            if (bCloseConnection)
                this.CloseConnnection(false);
            this.m_bNeedCommit = false;
        }

        /// <summary>
        /// 准备数据库连接和命令
        /// </summary>
        /// <param name="sql">SQL字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameterArray">命令参数</param>
        /// <param name="command">返回的数据库命令</param>
        public IDbCommand PrepareAll(string sql, CommandType type, DbParameter[] parameters)
        {
            IDbCommand command = ProviderFactory.GetCommand(sql
                , type, this.m_timeOut, parameters, this.m_provider);
            if (this.IsInTransaction())
            {
                command.Connection = this.m_connection;
                command.Transaction = this.m_transaction;
            }
            else
            {
                this.OpenConnnection();
                command.Connection = this.m_connection;
            }
            return command;
        }

        public int ExecuteNonQuery(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = null;
            return this.ExecuteNonQuery(sql, cmdType, ref parameters);
        }

        public int ExecuteNonQuery(string sql, CommandType cmdType, ref DbParameter[] parameters)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = this.PrepareAll(sql, cmdType, parameters);
                int nResult = cmd.ExecuteNonQuery();
                int nCount = cmd.Parameters.Count;
                for (int index = 0; index < nCount; index++)
                {
                    IDbDataParameter param = (IDbDataParameter)cmd.Parameters[index];
                    if (param.Direction == ParameterDirection.Output
                        || param.Direction == ParameterDirection.InputOutput)
                    {
                        parameters[index].Value = param.Value;
                    }
                }
                this.m_bNeedCommit = true;
                return nResult;
            }
            catch (Exception ex)
            {
                this.GenericExceptionHandler(ex);
                return -1;
            }
            finally
            {
                this.CloseConnnection(true);
                if (cmd != null) cmd.Dispose();
            }
        }

        public IDataReader ExecuteReader(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = null;
            return this.ExecuteReader(sql, cmdType, ref parameters);
        }

        public IDataReader ExecuteReader(string sql, CommandType cmdType, ref DbParameter[] parameters)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = this.PrepareAll(sql, cmdType, parameters);
                this.m_dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return this.m_dataReader;
            }
            catch (Exception ex)
            {
                this.GenericExceptionHandler(ex);
                return null;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }
        }

        public object ExecuteScalar(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = null;
            return this.ExecuteScalar(sql, cmdType, ref parameters);
        }

        public object ExecuteScalar(string sql, CommandType cmdType, ref DbParameter[] parameters)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = this.PrepareAll(sql, cmdType, parameters);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                this.GenericExceptionHandler(ex);
                return null;
            }
            finally
            {
                this.CloseConnnection(true);
                if (cmd != null) cmd.Dispose();
            }
        }

        public DataSet ExecuteDataSet(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = null;
            return this.ExecuteDataSet(sql, cmdType, ref parameters);
        }

        public DataSet ExecuteDataSet(string sql, CommandType cmdType, ref DbParameter[] parameters)
        {
            IDbDataAdapter adapter = null;
            DataSet dataSet = new DataSet();
            try
            {
                adapter = ProviderFactory.GetAdapter(this.m_provider);
                adapter.SelectCommand = this.PrepareAll(sql, cmdType, parameters);
                dataSet.Clear();
                adapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception ex)
            {
                this.GenericExceptionHandler(ex);
                return null;
            }
            finally
            {
                this.CloseConnnection(true);
                if (adapter != null)
                {
                    if (adapter.SelectCommand != null) adapter.SelectCommand.Dispose();
                    ((IDisposable)adapter).Dispose();
                }
            }
        }

        private void GenericExceptionHandler(Exception ex)
        {
            if (this.m_provider != DataProvider.ODPNET)
                throw ex;
            Oracle.DataAccess.Client.OracleException oracleException = ex as Oracle.DataAccess.Client.OracleException;
            if (oracleException == null)
                throw ex;

            int CONNECTION_INVALID = 12571;

            //捕捉到网络连接断掉导致连接池中的连接失效异常后,进行清空缓存连接操作
            if (oracleException.Number == CONNECTION_INVALID && this.m_bClearPoolEnabled)
                this.CleanConnnectionPool();
            throw oracleException;
        }
    }
}
