// ***********************************************************
// ��װ��DataSet��DataTable��������Ĳ�����������
// Creator:YangMingkun  Date:2013-8-5
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Drawing;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;

namespace Windy.Common.Libraries
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// ��װ����ת������
        /// </summary>
        public struct Table
        {
            /// <summary>
            /// ��ָ����DataTable����һ��ͬ���ṹ�Ķ���.
            /// ������������
            /// </summary>
            /// <param name="source">ԴDataTable</param>
            /// <returns>DataTable</returns>
            public static DataTable CloneTable(DataTable source)
            {
                DataTable table = new DataTable();
                if (source != null)
                {
                    foreach (DataColumn column in source.Columns)
                        table.Columns.Add(column.ColumnName, column.DataType);
                }
                return table;
            }

            /// <summary>
            /// ��ָ�������ݼ���,��ȡָ���к�������������
            /// </summary>
            /// <param name="data">���ݼ�</param>
            /// <param name="row">������</param>
            /// <param name="column">������</param>
            /// <returns>��Ԫ������</returns>
            public static object GetFieldValue(DataSet data, int row, int column)
            {
                if (data == null || data.Tables.Count <= 0)
                    return string.Empty;
                return GlobalMethods.Table.GetFieldValue(data.Tables[0], row, column);
            }

            /// <summary>
            /// ��ָ�������ݼ���,��ȡָ���к������Ƶ�����
            /// </summary>
            /// <param name="data">���ݼ�</param>
            /// <param name="row">������</param>
            /// <param name="column">������</param>
            /// <returns>��Ԫ������</returns>
            public static object GetFieldValue(DataSet data, int row, string column)
            {
                if (data == null || data.Tables.Count <= 0)
                    return string.Empty;
                return GlobalMethods.Table.GetFieldValue(data.Tables[0], row, column);
            }

            /// <summary>
            /// ��ָ���������к�������������
            /// </summary>
            /// <param name="row">������</param>
            /// <param name="column">������</param>
            /// <returns>��Ԫ������</returns>
            public static object GetFieldValue(DataRow row, int column)
            {
                if (row == null)
                    return string.Empty;
                object value = null;
                try
                {
                    if (!row.IsNull(column))
                        value = row[column];
                }
                catch { return string.Empty; }
                if (value == DBNull.Value || GlobalMethods.Misc.IsEmptyString(value))
                    return string.Empty;
                return value;
            }

            /// <summary>
            /// ��ָ���������к������Ƶ�����
            /// </summary>
            /// <param name="row">������</param>
            /// <param name="column">������</param>
            /// <returns>��Ԫ������</returns>
            public static object GetFieldValue(DataRow row, string column)
            {
                if (row == null)
                    return string.Empty;
                object value = null;
                try
                {
                    if (!row.IsNull(column))
                        value = row[column];
                }
                catch { return string.Empty; }
                if (value == DBNull.Value || GlobalMethods.Misc.IsEmptyString(value))
                    return string.Empty;
                return value;
            }

            /// <summary>
            /// ��ָ�������ݼ���,��ȡָ���к�������������
            /// </summary>
            /// <param name="table">���ݼ�</param>
            /// <param name="row">������</param>
            /// <param name="column">������</param>
            /// <returns>��Ԫ������</returns>
            public static object GetFieldValue(DataTable table, int row, int column)
            {
                if (table == null)
                    return string.Empty;
                if (row < 0 || row >= table.Rows.Count)
                    return string.Empty;
                if (column < 0 || column >= table.Columns.Count)
                    return string.Empty;

                object value = null;
                try
                {
                    DataRow dataRow = table.Rows[row];
                    if (!dataRow.IsNull(column))
                        value = dataRow[column];
                }
                catch { return string.Empty; }
                if (value == DBNull.Value || GlobalMethods.Misc.IsEmptyString(value))
                    return string.Empty;
                return value;
            }

            /// <summary>
            /// ��ָ�������ݼ���,��ȡָ���к������Ƶ�����
            /// </summary>
            /// <param name="table">���ݼ�</param>
            /// <param name="row">������</param>
            /// <param name="column">������</param>
            /// <returns>��Ԫ������</returns>
            public static object GetFieldValue(DataTable table, int row, string column)
            {
                if (table == null)
                    return string.Empty;
                if (column == null)
                    column = "";
                if (row < 0 || row >= table.Rows.Count)
                    return string.Empty;

                object value = null;
                try
                {
                    DataRow dataRow = table.Rows[row];
                    if (!dataRow.IsNull(column))
                        value = dataRow[column];
                }
                catch { return string.Empty; }
                if (value == DBNull.Value || GlobalMethods.Misc.IsEmptyString(value))
                    return string.Empty;
                return value;
            }

            /// <summary>
            /// ��ָ����DataGridView�ؼ�����ת��ΪDataTable���󷵻�
            /// </summary>
            /// <param name="gridView">DataGridView�ؼ�</param>
            /// <param name="start">��ʼ��</param>
            /// <param name="end">������</param>
            /// <param name="columnNameMode">
            ///  ��������ģʽ��
            ///  0 -         ʹ���еı�����Ϊ����;
            ///  1 -         ʹ���е�Tag������Ϊ�б�;
            ///  2������ֵ - ʹ���е�������Ϊ����;
            /// </param>
            /// <returns>DataTable</returns>
            public static DataTable GetDataTable(DataGridView gridView
                , int start, int end, short columnNameMode)
            {
                DataTable table = new DataTable();
                if (gridView == null || gridView.IsDisposed)
                    return table;
                foreach (DataGridViewColumn gridColumn in gridView.Columns)
                {
                    string columnName = string.Empty;
                    if (columnNameMode == 0)
                    {
                        columnName = gridColumn.HeaderText;
                    }
                    else if (columnNameMode == 1)
                    {
                        if (gridColumn.Tag == null)
                            columnName = string.Empty;
                        else
                            columnName = gridColumn.Tag.ToString();
                    }
                    else
                    {
                        columnName = gridColumn.Name;
                    }
                    table.Columns.Add(columnName, typeof(object));
                }

                if (start < 0)
                    start = 0;
                if (end >= gridView.Rows.Count)
                    end = gridView.Rows.Count - 1;

                for (int index = start; index <= end; index++)
                {
                    DataGridViewRow gridRow = gridView.Rows[index];
                    if (gridRow.IsNewRow)
                        continue;
                    object[] values = new object[gridRow.Cells.Count];
                    for (int cellIndex = 0; cellIndex < gridRow.Cells.Count; cellIndex++)
                        values[cellIndex] = gridRow.Cells[cellIndex].Value;
                    table.Rows.Add(values);
                }
                return table;
            }

            /// <summary>
            /// ��ָ����DataGridView�ؼ�����ת��ΪDataTable���󷵻�
            /// </summary>
            /// <param name="gridView">DataGridView�ؼ�</param>
            /// <param name="onlySelected">�Ƿ�����ѡ����</param>
            /// <param name="columnNameMode">
            ///  ��������ģʽ��
            ///  0 -         ʹ���еı�����Ϊ����;
            ///  1 -         ʹ���е�Tag������Ϊ�б�;
            ///  2������ֵ - ʹ���е�������Ϊ����;
            /// </param>
            /// <returns>DataTable</returns>
            public static DataTable GetDataTable(DataGridView gridView
                , bool onlySelected, short columnNameMode)
            {
                DataTable table = new DataTable();
                if (gridView == null || gridView.IsDisposed)
                    return table;
                foreach (DataGridViewColumn gridColumn in gridView.Columns)
                {
                    string columnName = string.Empty;
                    if (columnNameMode == 0)
                    {
                        columnName = gridColumn.HeaderText;
                    }
                    else if (columnNameMode == 1)
                    {
                        if (gridColumn.Tag == null)
                            columnName = string.Empty;
                        else
                            columnName = gridColumn.Tag.ToString();
                    }
                    else
                    {
                        columnName = gridColumn.Name;
                    }
                    table.Columns.Add(columnName, typeof(object));
                }
                for (int index = 0; index < gridView.Rows.Count; index++)
                {
                    DataGridViewRow gridRow = gridView.Rows[index];
                    if (gridRow.IsNewRow)
                        continue;
                    if (onlySelected && !gridRow.Selected)
                        continue;
                    object[] values = new object[gridRow.Cells.Count];
                    for (int cellIndex = 0; cellIndex < gridRow.Cells.Count; cellIndex++)
                        values[cellIndex] = gridRow.Cells[cellIndex].Value;
                    table.Rows.Add(values);
                }
                return table;
            }

            /// <summary>
            /// ��ָ���Ķ���ʵ���������б�ת��ΪDataTable����
            /// </summary>
            /// <param name="instance">����ʵ��</param>
            /// <returns>DataTable����</returns>
            public static DataTable GetDataTable(object instance)
            {
                if (instance == null)
                    return null;

                IEnumerable enumerableObject = instance as IEnumerable;
                if (enumerableObject != null)
                    return GetDataTableFromList(enumerableObject);

                DataTable table = new DataTable();
                PropertyInfo[] properties = instance.GetType().GetProperties();
                List<object> values = new List<object>();
                foreach (PropertyInfo property in properties)
                {
                    if (!property.CanRead)
                        continue;
                    if (table.Columns.Contains(property.Name))
                        continue;
                    table.Columns.Add(property.Name, property.PropertyType);
                    values.Add(GlobalMethods.Reflect.GetPropertyValue(instance, property));
                }
                table.Rows.Add(values.ToArray());
                return table;
            }

            /// <summary>
            /// ��ָ���Ķ���ʵ���������б�ת��ΪDataTable����
            /// </summary>
            /// <param name="list">����ʵ��</param>
            /// <returns>DataTable����</returns>
            private static DataTable GetDataTableFromList(IEnumerable list)
            {
                if (list == null)
                    return null;

                IEnumerator enumerator = list.GetEnumerator();
                if (enumerator == null)
                    return null;

                DataTable table = new DataTable();
                while (enumerator.MoveNext())
                {
                    object instance = enumerator.Current;
                    PropertyInfo[] properties = instance.GetType().GetProperties();
                    List<object> values = new List<object>();
                    foreach (PropertyInfo property in properties)
                    {
                        if (!property.CanRead)
                            continue;
                        if (table.Rows.Count <= 0 && !table.Columns.Contains(property.Name))
                            table.Columns.Add(property.Name, property.PropertyType);
                        values.Add(GlobalMethods.Reflect.GetPropertyValue(instance, property));
                    }
                    table.Rows.Add(values.ToArray());
                }
                return table;
            }

            /// <summary>
            /// ��ָ����DataRow�����ﱣ�������ת��������ʵ����
            /// </summary>
            /// <param name="row">DataRow����</param>
            /// <param name="instance">����ʵ��</param>
            /// <returns>�Ƿ�ɹ�</returns>
            public static bool DataRowToObject(DataRow row, object instance)
            {
                if (row == null || row.Table == null || instance == null)
                    return false;
                DataColumnCollection columns = row.Table.Columns;
                if (columns == null || columns.Count <= 0)
                    return false;
                PropertyInfo[] properties = instance.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (!property.CanWrite)
                        continue;

                    foreach (DataColumn column in columns)
                    {
                        if (string.Compare(column.ColumnName, property.Name, false) != 0)
                            continue;
                        object propertyValue = null;
                        if (!row.IsNull(column))
                            propertyValue = row[column];
                        GlobalMethods.Reflect.SetPropertyValue(instance, property, propertyValue);
                        break;
                    }
                }
                return true;
            }

            /// <summary>
            /// ��DataTable�����ݵ�����ΪCSV�ļ����
            /// </summary>
            /// <param name="table">��Ҫת����Table����</param>
            /// <param name="fileName">CSV�ļ����ȫ·��</param>
            /// <returns>�Ƿ�ת���ɹ�</returns>
            public static bool ExportCsvFile(DataTable table, string fileName)
            {
                if (table == null || GlobalMethods.Misc.IsEmptyString(fileName))
                    return false;

                if (!GlobalMethods.IO.DeleteFile(fileName))
                    return false;

                StringBuilder sbRowColumnText = new StringBuilder();

                //�����м��� 
                for (int index = 0; index < table.Columns.Count; index++)
                {
                    if (index > 0)
                        sbRowColumnText.Append(",");
                    string columnName = table.Columns[index].ColumnName;
                    columnName = GlobalMethods.Convert.ReplaceText(columnName, new string[] { "\"" }, new string[] { "\"\"" });
                    sbRowColumnText.AppendFormat("\"{0}\"", columnName);
                }
                sbRowColumnText.AppendLine();

                //����������   
                foreach (DataRow row in table.Rows)
                {
                    for (int index = 0; index < table.Columns.Count; index++)
                    {
                        if (index > 0)
                            sbRowColumnText.Append(",");
                        string cellValue = GlobalMethods.Table.GetFieldValue(row, index).ToString();
                        cellValue = GlobalMethods.Convert.ReplaceText(cellValue, new string[] { "\"" }, new string[] { "\"\"" });
                        sbRowColumnText.AppendFormat("\"{0}\"", cellValue);
                    }
                    sbRowColumnText.AppendLine();
                }
                return GlobalMethods.IO.WriteFileText(fileName, sbRowColumnText.ToString());
            }
        }
    }
}
