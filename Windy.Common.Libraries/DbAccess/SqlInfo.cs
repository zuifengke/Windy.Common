// ***********************************************************
// �������ݿ����ʹ�õ�SQL��Ϣ��.
// Creator:YangMingkun  Date:2013-11-15
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Windy.Common.Libraries.DbAccess
{
    public class SqlInfo
    {
        private string m_sql = string.Empty;
        /// <summary>
        /// ��ȡ������SQL���
        /// </summary>
        public string SQL
        {
            get { return this.m_sql; }
            set { this.m_sql = value; }
        }

        private bool m_isProc = false;
        /// <summary>
        /// ��ȡ�������Ƿ��ǹ���
        /// </summary>
        public bool IsProc
        {
            get { return this.m_isProc; }
            set { this.m_isProc = value; }
        }

        private object[] m_args = null;
        /// <summary>
        /// ��ȡ������sql������
        /// </summary>
        public object[] Args
        {
            get { return this.m_args; }
            set { this.m_args = value; }
        }

        public SqlInfo()
        {
        }

        public SqlInfo(string sql)
        {
            this.m_sql = sql;
        }

        public SqlInfo(string sql, bool isProc)
        {
            this.m_sql = sql;
            this.m_isProc = isProc;
        }

        public SqlInfo(string sql, object[] args)
        {
            this.m_sql = sql;
            this.m_args = args;
        }

        public SqlInfo(string sql, bool isProc, object[] args)
        {
            this.m_sql = sql;
            this.m_isProc = isProc;
            this.m_args = args;
        }
    }
}
