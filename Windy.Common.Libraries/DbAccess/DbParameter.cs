// ***********************************************************
// ���ݿ���ʻ���������֮��̬SQL�����ṹ��
// Creator:YangMingkun  Date:2009-6-27
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;

namespace Windy.Common.Libraries.DbAccess
{
    /// <summary>
    /// ��̬SQL�����ṹ��
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class DbParameter
    {
        private string m_name = null;
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public string Name
        {
            get { return this.m_name; }
        }

        private object m_value = null;
        /// <summary>
        /// ��ȡ�����ò���ֵ
        /// </summary>
        public object Value
        {
            get { return this.m_value; }
            set { this.SetValue(value); }
        }

        private DbType m_dataType = 0;
        /// <summary>
        /// ��ȡ������������
        /// </summary>
        public DbType DataType
        {
            get { return this.m_dataType; }
        }

        private int m_size = 0;
        /// <summary>
        /// ��ȡ�������ݴ�С
        /// </summary>
        public int Size
        {
            get { return this.m_size; }
            set { this.m_size = value; }
        }

        private string m_sourceColumn = null;
        /// <summary>
        /// ��ȡ������Դ����
        /// </summary>
        public string SourceColumn
        {
            get { return this.m_sourceColumn; }
            set { this.m_sourceColumn = value; }
        }

        private ParameterDirection m_direction = ParameterDirection.Input;
        /// <summary>
        /// ��ȡ�����ò������ݷ���
        /// </summary>
        public ParameterDirection Direction
        {
            get { return this.m_direction; }
        }

        /// <summary>
        /// ���ò���ֵ.
        /// �����ݲ���ֵ�Զ��������ݿ�ֵ����
        /// </summary>
        /// <param name="value">����ֵ</param>
        private void SetValue(object value)
        {
            this.m_value = value;
            if (value is int)
                this.m_dataType = DbType.Int32;
            else if (value is float)
                this.m_dataType = DbType.Single;
            else if (value is DateTime)
                this.m_dataType = DbType.DateTime;
            else if (value is byte[])
                this.m_dataType = DbType.Binary;
            else
                this.m_dataType = DbType.String;
        }

        /// <summary>
        /// ʵ�������ݿ��ַ������Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">�����ַ�������ֵ</param>
        public DbParameter(string name, string value)
        {
            this.m_name = name;
            this.Value = value;
        }

        /// <summary>
        /// ʵ�������ݿ��������Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">������������ֵ</param>
        public DbParameter(string name, int value)
        {
            this.m_name = name;
            this.Value = value;
        }

        /// <summary>
        /// ʵ�������ݿ⸡�����Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">������������ֵ</param>
        public DbParameter(string name, float value)
        {
            this.m_name = name;
            this.Value = value;
        }

        /// <summary>
        /// ʵ�������ݿ��ֽ����Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">�����ֽ�����ֵ</param>
        public DbParameter(string name, byte[] value)
        {
            this.m_name = name;
            this.Value = value;
        }

        /// <summary>
        /// ʵ�������ݿ�����ʱ�����Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">��������ʱ������ֵ</param>
        public DbParameter(string name, DateTime value)
        {
            this.m_name = name;
            this.Value = value;
        }

        /// <summary>
        /// ʵ�������ݿ�δ��ȷָ�����Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">δ��ȷָ������ֵ</param>
        public DbParameter(string name, object value)
        {
            this.m_name = name;
            this.Value = value;
        }

        /// <summary>
        /// ʵ�������ݿ��ַ������Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">�����ַ�������ֵ</param>
        /// <param name="direction">��������</param>
        public DbParameter(string name, string value, ParameterDirection direction)
        {
            this.m_name = name;
            this.Value = value;
            this.m_direction = direction;
        }

        /// <summary>
        /// ʵ�������ݿ��������Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">������������ֵ</param>
        /// <param name="direction">��������</param>
        public DbParameter(string name, int value, ParameterDirection direction)
        {
            this.m_name = name;
            this.Value = value;
            this.m_direction = direction;
        }

        /// <summary>
        /// ʵ�������ݿ⸡�����Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">������������ֵ</param>
        /// <param name="direction">��������</param>
        public DbParameter(string name, float value, ParameterDirection direction)
        {
            this.m_name = name;
            this.Value = value;
            this.m_direction = direction;
        }

        /// <summary>
        /// ʵ�������ݿ��ֽ����Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">�����ֽ�����ֵ</param>
        /// <param name="direction">��������</param>
        public DbParameter(string name, byte[] value, ParameterDirection direction)
        {
            this.m_name = name;
            this.Value = value;
            this.m_direction = direction;
        }

        /// <summary>
        /// ʵ�������ݿ�����ʱ�����Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">��������ʱ������ֵ</param>
        /// <param name="direction">��������</param>
        public DbParameter(string name, DateTime value, ParameterDirection direction)
        {
            this.m_name = name;
            this.Value = value;
            this.m_direction = direction;
        }

        /// <summary>
        /// ʵ�������ݿ�δ��ȷָ�����Ͳ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">δ��ȷָ������ֵ</param>
        /// <param name="direction">��������</param>
        public DbParameter(string name, object value, ParameterDirection direction)
        {
            this.m_name = name;
            this.Value = value;
            this.m_direction = direction;
        }
    }
}
