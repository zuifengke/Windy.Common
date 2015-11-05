// ***********************************************************
// ��װһЩ�޷�����ķ�������
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Windows.Forms;

namespace Windy.Common.Libraries
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// ��װһЩ�޷�����ķ���
        /// </summary>
        public struct Misc
        {
            private static Random m_random = null;
            /// <summary>
            /// ����һ�������
            /// </summary>
            /// <param name="min">��Сֵ</param>
            /// <param name="max">���ֵ</param>
            /// <returns>ֵ</returns>
            public static int Random(int min, int max)
            {
                if (m_random == null)
                    m_random = new Random();
                return m_random.Next(min, max);
            }

            /// <summary>
            /// �ж��ַ��������Ƿ�Ϊ�մ�
            /// </summary>
            /// <param name="value">Ŀ���ַ���</param>
            /// <returns>true:Ϊ�մ�;false:�ǿմ�</returns>
            public static bool IsEmptyString(object value)
            {
                if (value == null)
                    return true;
                return IsEmptyString(value.ToString());
            }

            /// <summary>
            /// �ж��ַ����Ƿ�Ϊ�մ�
            /// </summary>
            /// <param name="value">Ŀ���ַ���</param>
            /// <returns>true:Ϊ�մ�;false:�ǿմ�</returns>
            public static bool IsEmptyString(string value)
            {
                if (value == null)
                    return true;
                if (value.Trim() == string.Empty)
                    return true;
                return false;
            }

            private static string m_workPath = string.Empty;

            /// <summary>
            /// �õ�Libraries��̬������Ŀ¼
            /// </summary>
            /// <returns>string</returns>
            public static string GetWorkingPath()
            {
                return GlobalMethods.Misc.GetWorkingPath(typeof(GlobalMethods));
            }

            /// <summary>
            /// �õ�ָ���������ڵĶ�̬�������Ŀ¼
            /// </summary>
            /// <param name="type">����</param>
            /// <returns>string</returns>
            public static string GetWorkingPath(Type type)
            {
                if (type == null || m_workPath.Trim() != string.Empty)
                    return m_workPath;
                try
                {
                    string szDllPath = type.Assembly.Location;
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(szDllPath);
                    m_workPath = fileInfo.DirectoryName;
                    return m_workPath;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetWorkingPath", new string[] { "type" }
                        , new object[] { type }, "����·����ȡʧ��!", ex);
                    return Application.StartupPath;
                }
            }
        }
    }
}
