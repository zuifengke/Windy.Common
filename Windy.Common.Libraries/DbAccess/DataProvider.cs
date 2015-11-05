// ***********************************************************
// ���ݿ���ʻ���������֮�����ṩ������ö��
// Creator:YangMingkun  Date:2009-6-27
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;

namespace Windy.Common.Libraries.DbAccess
{
    /// <summary>
    /// �����ṩ������ö��
    /// </summary>
    public enum DataProvider
    {
        /// <summary>
        /// ODBC(0)
        /// </summary>
        Odbc = 0,
        /// <summary>
        /// SQLClient(1)
        /// </summary>
        SqlClient = 1,
        /// <summary>
        /// OLEDB(2)
        /// </summary>
        OleDb = 2,
        /// <summary>
        /// .NET�Դ���ORACLE����(3)
        /// </summary>
        OracleClient = 3,
        /// <summary>
        /// ORACLE�ṩ��ODPNET(4)
        /// </summary>
        ODPNET = 4,
        /// <summary>
        /// .NET�Դ���MySql����(5)
        /// </summary>
        MySqlClient = 5
    }
}
