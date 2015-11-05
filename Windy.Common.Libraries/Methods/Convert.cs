// ***********************************************************
// ��װһЩ����ת����������
// Creator:YangMingkun  Date:2009-6-22
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
using System.Runtime.Serialization.Formatters.Binary;

namespace Windy.Common.Libraries
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// ��װ����ת������
        /// </summary>
        public struct Convert
        {
            /// <summary>
            /// ����ת��Ϊ�ٷ�֮һӢ��
            /// </summary>
            /// <param name="mm">����ֵ</param>
            /// <returns>�ٷ�֮һӢ��ֵ</returns>
            public static float MMToHundredInch(float mm)
            {
                return (float)(mm * 3.937f);
            }

            /// <summary>
            /// �ٷ�֮һӢ��ת��Ϊ����
            /// </summary>
            /// <param name="inch">�ٷ�֮һӢ��ֵ</param>
            /// <returns>����ֵ</returns>
            public static float HunderedInchToMM(float inch)
            {
                return (float)(((float)inch) / 3.937f);
            }

            /// <summary>
            /// ������ת��Ϊ����ֵ
            /// </summary>
            /// <param name="fMMValue">����ֵ</param>
            /// <param name="bVertical">�������</param>
            public static int MMToPixel(float fMMValue, bool bVertical)
            {
                float fDpiX = 0f;
                float fDpiY = 0f;

                IntPtr hScreenDC = NativeMethods.User32.GetDC(0);
                if (hScreenDC == IntPtr.Zero)
                {
                    fDpiX = fDpiY = 96.0f;
                }
                else
                {
                    using (Graphics g = Graphics.FromHdc(hScreenDC))
                    {
                        fDpiX = g.DpiX;
                        fDpiY = g.DpiY;
                    }
                }
                NativeMethods.User32.ReleaseDC(0, hScreenDC);
                if (bVertical)
                    return (int)Math.Round(fMMValue * (fDpiY / 25.4));
                else
                    return (int)Math.Round(fMMValue * (fDpiX / 25.4));
            }

            /// <summary>
            /// ������ֵת��Ϊ����ֵ
            /// </summary>
            /// <param name="fPixelValue">����ֵ</param>
            /// <param name="bVertical">�Ƿ���Y����ת��</param>
            /// <returns>����ֵ</returns>
            public static float Pixel2MM(float fPixelValue, bool bVertical)
            {
                float fDpiX = 0f;
                float fDpiY = 0f;

                IntPtr hScreenDC = NativeMethods.User32.GetDC(0);
                if (hScreenDC == IntPtr.Zero)
                {
                    fDpiX = fDpiY = 96.0f;
                }
                else
                {
                    using (Graphics g = Graphics.FromHdc(hScreenDC))
                    {
                        fDpiX = g.DpiX;
                        fDpiY = g.DpiY;
                    }
                }
                NativeMethods.User32.ReleaseDC(0, hScreenDC);
                if (bVertical)
                    return fPixelValue * 25.4f / fDpiY;
                else
                    return fPixelValue * 25.4f / fDpiX;
            }

            /// <summary>
            /// ����ָ����ֵ����ӽ�����������
            /// </summary>
            /// <param name="dData">��ֵ</param>
            /// <returns>���Ͳ�������</returns>
            public static double Fix(double dData)
            {
                if (dData >= 0.0)
                {
                    return Math.Floor(dData);
                }
                return -Math.Floor(-dData);
            }

            /// <summary>
            /// ����ContentAlignmentö�ٻ�ȡStringFormat�ַ�����ʽ������
            /// </summary>
            /// <param name="alignment">ָ����ContentAlignmentö��</param>
            /// <returns>StringFormat�ַ�����ʽ������</returns>
            public static StringFormat GetStringFormat(ContentAlignment alignment)
            {
                StringFormat format = new StringFormat();
                switch (alignment)
                {
                    case ContentAlignment.BottomCenter:
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.BottomLeft:
                        format.Alignment = StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.BottomRight:
                        format.Alignment = StringAlignment.Far;
                        format.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.MiddleCenter:
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.TopRight:
                        format.Alignment = StringAlignment.Far;
                        format.LineAlignment = StringAlignment.Near;
                        break;
                    case ContentAlignment.MiddleRight:
                        format.Alignment = StringAlignment.Far;
                        format.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.TopCenter:
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Near;
                        break;
                    case ContentAlignment.TopLeft:
                        format.Alignment = StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Near;
                        break;
                    default: //MiddleLeft
                        format.Alignment = StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Center;
                        break;
                }
                return format;
            }

            /// <summary>
            /// ����DataGridViewContentAlignmentö�ٻ�ȡStringFormat�ַ�����ʽ������
            /// </summary>
            /// <param name="alignment">ָ����ContentAlignmentö��</param>
            /// <returns>StringFormat�ַ�����ʽ������</returns>
            public static StringFormat GetStringFormat(DataGridViewContentAlignment alignment)
            {
                StringFormat format = new StringFormat();
                switch (alignment)
                {
                    case DataGridViewContentAlignment.BottomCenter:
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Far;
                        break;
                    case DataGridViewContentAlignment.BottomLeft:
                        format.Alignment = StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Far;
                        break;
                    case DataGridViewContentAlignment.BottomRight:
                        format.Alignment = StringAlignment.Far;
                        format.LineAlignment = StringAlignment.Far;
                        break;
                    case DataGridViewContentAlignment.MiddleCenter:
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        break;
                    case DataGridViewContentAlignment.TopRight:
                        format.Alignment = StringAlignment.Far;
                        format.LineAlignment = StringAlignment.Near;
                        break;
                    case DataGridViewContentAlignment.MiddleRight:
                        format.Alignment = StringAlignment.Far;
                        format.LineAlignment = StringAlignment.Center;
                        break;
                    case DataGridViewContentAlignment.TopCenter:
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Near;
                        break;
                    case DataGridViewContentAlignment.TopLeft:
                        format.Alignment = StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Near;
                        break;
                    default: //MiddleLeft
                        format.Alignment = StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Center;
                        break;
                }
                return format;
            }

            /// <summary>
            /// ��ָ���ı�����ָ���Ŀ��ת��Ϊ�ı�������
            /// </summary>
            /// <param name="text">�ı�</param>
            /// <param name="font">����</param>
            /// <param name="width">�п�</param>
            /// <returns>�ı�������</returns>
            public static string[] GetTextLines(string text, Font font, float width)
            {
                List<string> lines = new List<string>();
                if (string.IsNullOrEmpty(text) || font == null)
                    return lines.ToArray();
                IntPtr hdc = NativeMethods.User32.GetDC(0);
                Graphics graphics = Graphics.FromHdc(hdc);
                StringBuilder line = new StringBuilder();
                foreach (char ch in text)
                {
                    line.Append(ch);
                    SizeF size = graphics.MeasureString(line.ToString(), font);
                    if (size.Width > width)
                    {
                        line.Remove(line.Length - 1, 1);
                        if (line.Length > 0)
                            lines.Add(line.ToString());
                        line.Remove(0, line.Length);
                        line.Append(ch);
                    }
                    else if (ch == '\r' || ch == '\n')
                    {
                        line.Remove(line.Length - 1, 1);
                        if (line.Length > 0)
                            lines.Add(line.ToString());
                        line.Remove(0, line.Length);
                    }
                }
                //���һ������ǿ���,��ô����
                if (!GlobalMethods.Misc.IsEmptyString(line))
                    lines.Add(line.ToString());
                graphics.Dispose();
                NativeMethods.User32.ReleaseDC(0, hdc);
                return lines.ToArray();
            }

            private static Encoding m_defaultEncoding = null;
            /// <summary>
            /// ��ȡ����ϵͳĬ���ַ�����
            /// </summary>
            /// <returns>Ĭ���ַ�����</returns>
            public static Encoding GetDefaultEncoding()
            {
                try
                {
                    if (m_defaultEncoding == null)
                        m_defaultEncoding = Encoding.GetEncoding("utf-8");
                    return m_defaultEncoding;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetDefaultEncoding"
                        , "�޷���ȡ����ϵͳ��Ĭ���ַ�����!", ex);
                    return Encoding.Default;
                }
            }

            /// <summary>
            /// ��ȡָ���ַ������ֽڳ���
            /// </summary>
            /// <param name="value">�ַ���</param>
            /// <returns>�ֽڳ���</returns>
            public static int GetByteLength(string value)
            {
                return GetByteLength(value, Convert.GetDefaultEncoding());
            }

            /// <summary>
            /// ��ȡָ���ַ������ֽڳ���
            /// </summary>
            /// <param name="value">�ַ���</param>
            /// <returns>�ֽڳ���</returns>
            public static int GetByteLength(string value, Encoding encoding)
            {
                byte[] byteText = GetBytes(value, encoding);
                return (byteText == null) ? 0 : byteText.Length;
            }

            /// <summary>
            /// �ж�ָ���ı����Ƿ�����ֵ����
            /// </summary>
            /// <param name="text">�ı���</param>
            /// <returns>�Ƿ�����ֵ����</returns>
            public static bool IsNumericValue(string text)
            {
                if (string.IsNullOrEmpty(text))
                    return true;
                double value = 0d;
                if (double.TryParse(text, out value))
                    return true;
                return false;
            }

            /// <summary>
            /// �ж�ָ���ı����Ƿ�����������
            /// </summary>
            /// <param name="text">�ı���</param>
            /// <returns>�Ƿ�����������</returns>
            public static bool IsIntegerValue(string text)
            {
                if (string.IsNullOrEmpty(text))
                    return true;
                int value = 0;
                if (int.TryParse(text, out value))
                    return true;
                return false;
            }

            /// <summary>
            /// �ж�ָ�����ַ��Ƿ�Ϊȫ���ַ�
            /// </summary>
            /// <param name="ch">�����ַ�</param>
            /// <returns>ȫ���ַ�����true,��Ƿ���false</returns>
            public static bool IsCharSBC(char ch)
            {
                return (ch > 65280 && ch < 65375);
            }

            /// <summary>
            /// ��ָ����ȫ���ַ���ת��Ϊ��Ӧ�İ���ַ���
            /// </summary>
            /// <param name="szSourceString">�����ַ���</param>
            /// <param name="arrExceptChars">�ų��ַ��б�</param>
            /// <returns>����ַ���</returns>
            /// <remarks>
            /// ȫ�ǿո�Ϊ12288����ǿո�Ϊ32
            /// �����ַ����(33-126)��ȫ��(65281-65374)�Ķ�Ӧ��ϵ�ǣ������65248
            /// </remarks>
            public static string SBCToDBC(string szSourceString, params char[] arrExceptChars)
            {
                if (string.IsNullOrEmpty(szSourceString))
                    return string.Empty;

                StringBuilder sbDestString = new StringBuilder();
                int nLength = szSourceString.Length;
                int nExceptCharsLength = 0;
                if (arrExceptChars != null)
                    nExceptCharsLength = arrExceptChars.Length;

                for (int index = 0; index < nLength; index++)
                {
                    char ch = szSourceString[index];
                    int nCharIndex = nExceptCharsLength;
                    while (nCharIndex > 0)
                    {
                        if (arrExceptChars[--nCharIndex] == ch)
                            nCharIndex = -1;
                    }
                    if (nCharIndex < 0)
                        sbDestString.Append(ch);
                    //else if (ch == '��')
                    //    sbDestString.Append(' ');
                    //else if (ch == '��')
                    //    sbDestString.Append('.');
                    else if (ch > 65280 && ch < 65375)
                        sbDestString.Append((char)(ch - 65248));
                    else
                        sbDestString.Append(ch);
                }
                return sbDestString.ToString();
            }

            /// <summary>
            /// ��ָ���İ���ַ���תΪȫ��(SBC case)
            /// </summary>
            /// <param name="szSourceString">�����ַ���</param>
            /// <param name="arrExceptChars">�ų��ַ��б�</param>
            /// <returns>ȫ���ַ���</returns>
            /// <remarks>
            /// ȫ�ǿո�Ϊ12288����ǿո�Ϊ32
            /// �����ַ����(33-126)��ȫ��(65281-65374)�Ķ�Ӧ��ϵ�ǣ������65248
            /// </remarks>        
            public static string DBCToSBC(string szSourceString, params char[] arrExceptChars)
            {
                if (string.IsNullOrEmpty(szSourceString))
                    return string.Empty;

                StringBuilder sbDestString = new StringBuilder();
                int nLength = szSourceString.Length;
                int nExceptCharsLength = 0;
                if (arrExceptChars != null)
                    nExceptCharsLength = arrExceptChars.Length;

                for (int index = 0; index < nLength; index++)
                {
                    char ch = szSourceString[index];
                    int nCharIndex = nExceptCharsLength;
                    while (nCharIndex > 0)
                    {
                        if (arrExceptChars[--nCharIndex] == ch)
                            nCharIndex = -1;
                    }
                    //if (nCharIndex < 0)
                    //    sbDestString.Append(ch);
                    //else if (ch == ' ')
                    //    sbDestString.Append('��');
                    //else if (ch == '.')
                    //    sbDestString.Append('��');
                    //else if (ch < 127)
                    //    sbDestString.Append((char)(ch + 65248));
                    //else
                    //    sbDestString.Append(ch);
                }
                return sbDestString.ToString();
            }

            /// <summary>
            /// ���ֽڽ��ַ�������ಹ��ָ��λ���Ŀո�
            /// </summary>
            /// <param name="text">�ַ���</param>
            /// <param name="count">�ո���Ŀ</param>
            /// <returns>����ո����ַ���</returns>
            public static string PadLeft(string text, int count)
            {
                int length = GetByteLength(text, GetDefaultEncoding());
                string result = text;
                while (length++ < count)
                    result = string.Concat(" ", result);
                return result;
            }

            /// <summary>
            /// ���ֽڽ��ַ������Ҳಹ��ָ��λ���Ŀո�
            /// </summary>
            /// <param name="text">�ַ���</param>
            /// <param name="count">�ո���Ŀ</param>
            /// <returns>����ո����ַ���</returns>
            public static string PadRight(string text, int count)
            {
                int length = GetByteLength(text, GetDefaultEncoding());
                string result = text;
                while (length++ < count)
                    result = string.Concat(result, " ");
                return result;
            }

            /// <summary>
            /// ���л�ָ���Ķ���Ϊ����������
            /// </summary>
            /// <param name="value">����</param>
            /// <returns>����������</returns>
            public static byte[] Serialize(object value)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    formatter.Serialize(memoryStream, value);
                    byte[] byteData = memoryStream.ToArray();
                    return (byteData == null) ? new byte[0] : byteData;
                }
                catch { return new byte[0]; }
                finally
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }
            }

            /// <summary>
            /// ���л�ָ���Ķ���������Ϊ����
            /// </summary>
            /// <param name="data">����������</param>
            /// <returns>����</returns>
            public static object Deserialize(byte[] data)
            {
                if (data == null) data = new byte[0];

                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream(data);
                try
                {
                    return formatter.Deserialize(memoryStream);
                }
                catch { return null; }
                finally
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }
            }

            /// <summary>
            /// ��ָ������ת���ַ���Ϊ�ֽ�
            /// </summary>
            /// <param name="text">�ַ���</param>
            /// <returns>�ֽ�</returns>
            public static byte[] GetBytes(string value)
            {
                return GetBytes(value, GetDefaultEncoding());
            }

            /// <summary>
            /// ��ָ������ת���ַ���Ϊ�ֽ�
            /// </summary>
            /// <param name="text">�ַ���</param>
            /// <param name="encoding">����</param>
            /// <returns>�ֽ�</returns>
            public static byte[] GetBytes(string value, Encoding encoding)
            {
                byte[] byteData = null;
                StringToBytes(value, encoding, ref byteData);
                return byteData;
            }

            /// <summary>
            /// ���ֽ����л�ȡĬ�ϱ����ַ���
            /// </summary>
            /// <param name="byteData">�ֽ���</param>
            /// <returns>�ַ���</returns>
            public static string GetString(byte[] byteData)
            {
                return GetString(byteData, GetDefaultEncoding());
            }

            /// <summary>
            /// ���ֽ����л�ȡָ�������ַ���
            /// </summary>
            /// <param name="byteData">�ֽ���</param>
            /// <param name="encoding">����</param>
            /// <returns>�ַ���</returns>
            public static string GetString(byte[] byteData, Encoding encoding)
            {
                string value = null;
                BytesToString(byteData, encoding, ref value);
                return value;
            }

            /// <summary>
            /// ת��ȱʡ�����ַ���ΪBASE64�ַ���
            /// </summary>
            /// <param name="text">ȱʡ�����ַ���</param>
            /// <param name="byteData">���ص��ֽ�����</param>
            /// <returns>bool</returns>
            public static bool StringToBytes(string text, ref byte[] byteData)
            {
                return StringToBytes(text, GetDefaultEncoding(), ref byteData);
            }

            /// <summary>
            /// ��ָ������ת���ַ���ΪBASE64�ַ���
            /// </summary>
            /// <param name="text">�ַ���</param>
            /// <param name="encoding">����</param>
            /// <param name="byteData">���ص��ֽ�����</param>
            /// <returns>bool</returns>
            public static bool StringToBytes(string text, Encoding encoding, ref byte[] byteData)
            {
                byteData = new byte[0];
                if (GlobalMethods.Misc.IsEmptyString(text))
                    return true;
                try
                {
                    byteData = encoding.GetBytes(text);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.StringToBytes", new string[] { "text" }
                        , new string[] { text }, "�޷����ַ���תΪ�ֽ�����!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ת���ֽ�����Ϊȱʡ�����ַ���
            /// </summary>
            /// <param name="byteData">���ص�ȱʡ�����ֽ�����</param>
            /// <param name="value">�ַ���ֵ</param>
            /// <returns>bool</returns>
            public static bool BytesToString(byte[] byteData, ref string value)
            {
                return BytesToString(byteData, GetDefaultEncoding(), ref value);
            }

            /// <summary>
            /// ת���ֽ�����Ϊȱʡ�����ַ���
            /// </summary>
            /// <param name="byteData">���ص�ȱʡ�����ֽ�����</param>
            /// <param name="encoding">����</param>
            /// <param name="value">�ַ���ֵ</param>
            /// <returns>bool</returns>
            public static bool BytesToString(byte[] byteData, Encoding encoding, ref string value)
            {
                value = string.Empty;
                if (byteData == null || byteData.Length <= 0)
                    return true;
                try
                {
                    value = encoding.GetString(byteData);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.BytesToString", new string[] { "byteData" }
                        , new object[] { byteData }, "�޷�ת���ֽ�����Ϊȱʡ�����ַ���!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ת��ȱʡ�����ַ���ΪBASE64�ַ���
            /// </summary>
            /// <param name="text">ȱʡ�����ַ���</param>
            /// <param name="base64String">���ص�BASE64�ַ���</param>
            /// <returns>bool</returns>
            public static bool StringToBase64(string text, ref string base64String)
            {
                return StringToBase64(text, GetDefaultEncoding(), ref base64String);
            }

            /// <summary>
            /// ��ָ������ת���ַ���ΪBASE64�ַ���
            /// </summary>
            /// <param name="text">�ַ���</param>
            /// <param name="encoding">����</param>
            /// <param name="base64String">���ص�BASE64�ַ���</param>
            /// <returns>bool</returns>
            public static bool StringToBase64(string text, Encoding encoding, ref string base64String)
            {
                base64String = string.Empty;
                if (GlobalMethods.Misc.IsEmptyString(text))
                    return true;
                try
                {
                    byte[] byteText = encoding.GetBytes(text);
                    base64String = System.Convert.ToBase64String(byteText);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.StringToBase64", new string[] { "base64String" }
                        , new string[] { base64String }, "�޷���ȱʡ�����ַ���תΪBASE64�ַ���!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ת��BASE64�ַ���Ϊȱʡ�����ַ���
            /// </summary>
            /// <param name="base64String">BASE64�ַ���</param>
            /// <param name="text">���ص�ȱʡ�����ַ���</param>
            /// <returns>bool</returns>
            public static bool Base64ToString(string base64String, ref string text)
            {
                return Convert.Base64ToString(base64String, GetDefaultEncoding(), ref text);
            }

            /// <summary>
            /// ת��BASE64�ַ���Ϊȱʡ�����ַ���
            /// </summary>
            /// <param name="base64String">BASE64�ַ���</param>
            /// <param name="text">���ص�ȱʡ�����ַ���</param>
            /// <returns>bool</returns>
            public static bool Base64ToString(string base64String, Encoding encoding, ref string text)
            {
                text = string.Empty;
                if (GlobalMethods.Misc.IsEmptyString(base64String))
                    return true;
                try
                {
                    byte[] byteText = System.Convert.FromBase64String(base64String);
                    text = encoding.GetString(byteText);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Base64ToString", new string[] { "base64String" }
                        , new string[] { base64String }, "�޷���BASE64�ַ���תΪȱʡ�����ַ���!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ת��BASE64�ַ���Ϊȱʡ�����ֽ�����
            /// </summary>
            /// <param name="base64String">BASE64�ַ���</param>
            /// <param name="byteData">���ص�ȱʡ�����ֽ�����</param>
            /// <returns>bool</returns>
            public static bool Base64ToBytes(string base64String, ref byte[] byteData)
            {
                byteData = new byte[0];
                if (GlobalMethods.Misc.IsEmptyString(base64String))
                    return true;
                try
                {
                    byteData = System.Convert.FromBase64String(base64String);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Base64ToBytes", new string[] { "base64String" }
                        , new string[] { base64String }, "�޷���BASE64�ַ���Ϊȱʡ�����ֽ�����!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ת��ȱʡ�����ֽ�����ΪBASE64�ַ���
            /// </summary>
            /// <param name="byteData">���ص�ȱʡ�����ֽ�����</param>
            /// <param name="base64String">BASE64�ַ���</param>
            /// <returns>bool</returns>
            public static bool BytesToBase64(byte[] byteData, ref string base64String)
            {
                base64String = string.Empty;
                if (byteData == null || byteData.Length <= 0)
                    return true;
                try
                {
                    base64String = System.Convert.ToBase64String(byteData);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.BytesToBase64", new string[] { "byteData" }
                        , new object[] { byteData }, "�޷�ת��ȱʡ�����ֽ�����ΪBASE64�ַ���!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ������ֵת��Ϊ����ֵ
            /// </summary>
            /// <param name="value">����ֵ</param>
            /// <param name="defaultValue">�޷�ת��ʱ���ص�Ĭ��ֵ</param>
            /// <returns>ת���������ֵ</returns>
            public static int StringToValue(object value, int defaultValue)
            {
                if (value == null)
                    return defaultValue;
                int result = 0;
                if (int.TryParse(value.ToString(), out result))
                    return result;
                return defaultValue;
            }

            /// <summary>
            /// ������ֵת��Ϊ����ֵ
            /// </summary>
            /// <param name="value">����ֵ</param>
            /// <param name="defaultValue">�޷�ת��ʱ���ص�Ĭ��ֵ</param>
            /// <returns>ת����Ĳ���ֵ</returns>
            public static bool StringToValue(object value, bool defaultValue)
            {
                if (value == null)
                    return defaultValue;
                bool result = false;
                if (bool.TryParse(value.ToString(), out result))
                    return result;
                return defaultValue;
            }

            /// <summary>
            /// ������ֵת��Ϊ����ֵ
            /// </summary>
            /// <param name="value">����ֵ</param>
            /// <param name="defaultValue">�޷�ת��ʱ���ص�Ĭ��ֵ</param>
            /// <returns>ת����ĸ���ֵ</returns>
            public static float StringToValue(object value, float defaultValue)
            {
                if (value == null)
                    return defaultValue;
                float result = 0;
                if (float.TryParse(value.ToString(), out result))
                    return result;
                return defaultValue;
            }

            /// <summary>
            /// ������ֵת��Ϊ��ֵֵ
            /// </summary>
            /// <param name="value">����ֵ</param>
            /// <param name="defaultValue">�޷�ת��ʱ���ص�Ĭ��ֵ</param>
            /// <returns>ת�������ֵֵ</returns>
            public static decimal StringToValue(object value, decimal defaultValue)
            {
                if (value == null)
                    return defaultValue;
                decimal result = 0;
                if (decimal.TryParse(value.ToString(), out result))
                    return result;
                return defaultValue;
            }

            /// <summary>
            /// ������ֵת��Ϊ����ʱ��ֵ
            /// </summary>
            /// <param name="value">����ֵ</param>
            /// <param name="defaultValue">�޷�ת��ʱ���ص�Ĭ��ֵ</param>
            /// <returns>ת���������ʱ��ֵ</returns>
            public static DateTime StringToValue(object value, DateTime defaultValue)
            {
                if (value == null)
                    return defaultValue;
                DateTime result = DateTime.Now;
                if (DateTime.TryParse(value.ToString(), out result))
                    return result;
                return defaultValue;
            }

            /// <summary>
            /// ת������ֵΪʱ������
            /// </summary>
            /// <param name="value">����ֵ</param>
            /// <param name="dtResult">���ص�����ֵ</param>
            /// <returns>bool</returns>
            public static bool StringToDate(object value, ref DateTime dtResult)
            {
                if (GlobalMethods.Misc.IsEmptyString(value))
                    return false;
                try
                {
                    dtResult = DateTime.Parse(SBCToDBC(value.ToString(), null));
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.StringToDate", new string[] { "value" }
                        , new object[] { value }, "�ַ����޷�ת��Ϊʱ���ʽ!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ��ȡָ���ı����滻��ָ���ַ����ַ���
            /// </summary>
            /// <param name="szText">ԭ�ı�</param>
            /// <param name="arrChar">���滻�ַ��б�</param>
            /// <returns>���滻���ı�</returns>
            public static string ReplaceText(string szText, char[] arrChar)
            {
                return ReplaceText(szText, arrChar, null, 0);
            }

            /// <summary>
            /// ��ȡָ���ı����滻��ָ���ַ����ַ���
            /// </summary>
            /// <param name="szText">ԭ�ı�</param>
            /// <param name="arrChar">���滻�ַ��б�</param>
            /// <param name="szNewChar">�滻�ַ���</param>
            /// <returns>���滻���ı�</returns>
            public static string ReplaceText(string szText, char[] arrChar, string szNewChar)
            {
                return ReplaceText(szText, arrChar, szNewChar, 0);
            }

            /// <summary>
            /// ��ȡָ���ı����滻��ָ���ַ����ַ���
            /// </summary>
            /// <param name="szText">ԭ�ı�</param>
            /// <param name="arrOldChar">���ַ�</param>
            /// <param name="szNewChar">�滻�ı�</param>
            /// <param name="nCount">��󷵻س���,0��ʾ������</param>
            /// <returns>���滻���ı�</returns>
            public static string ReplaceText(string szText, char[] arrOldChar, string szNewChar, int nCount)
            {
                if (szText == null)
                    return string.Empty;
                if (arrOldChar == null || arrOldChar.Length <= 0)
                    return szText;

                bool bHasMaxLength = nCount > 0;

                StringBuilder sbText = new StringBuilder();
                int index = 0;
                while (index < szText.Length)
                {
                    char ch = szText[index++];
                    //��鵱ǰ�ַ��Ƿ���Ҫ����
                    int nCharIndex = 0;
                    while (nCharIndex < arrOldChar.Length)
                    {
                        if (arrOldChar[nCharIndex++] == ch)
                        {
                            nCharIndex = -1;
                            break;
                        }
                    }
                    //���ڵ���0ʱ��ʾ����Ҫ����
                    if (nCharIndex >= 0)
                    {
                        if (bHasMaxLength && nCount <= 0)
                            break;
                        nCount--;
                        sbText.Append(ch);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(szNewChar))
                            sbText.Append(szNewChar);
                    }
                }
                return sbText.ToString();
            }

            /// <summary>
            /// ��ָ���ı���һϵ�о��ı��滻Ϊ�µ��ı�
            /// </summary>
            /// <param name="szText">ָ��ԭ�ı�</param>
            /// <param name="arrOldText">���ı��б�</param>
            /// <param name="arrNewText">���ı��б�</param>
            /// <returns>�滻����ı�</returns>
            public static string ReplaceText(string szText, string[] arrOldText, string[] arrNewText)
            {
                if (string.IsNullOrEmpty(szText))
                    return szText;
                if (arrOldText == null || arrOldText.Length <= 0)
                    return szText;
                if (arrNewText == null || arrNewText.Length <= 0)
                    return szText;
                if (arrOldText.Length != arrNewText.Length)
                    return szText;
                for (int index = 0; index < arrOldText.Length; index++)
                {
                    if (string.IsNullOrEmpty(arrOldText[index]))
                        continue;
                    if (arrNewText[index] == null)
                        arrNewText[index] = string.Empty;
                    szText = szText.Replace(arrOldText[index], arrNewText[index]);
                }
                return szText;
            }

            #region "����ƴ����"
            /// <summary>
            /// ����ASCII�����������
            /// </summary>
            private static int[] m_arrPinyinCode = null;
            /// <summary>
            /// ��������ƴ�����ַ�������
            /// </summary>
            private static string[] m_arrPinyinText = null;
            /// <summary>
            /// ��ʼ��ƴ��ASCII�����
            /// </summary>
            private static void InitPinyinTable()
            {
                if (m_arrPinyinCode != null && m_arrPinyinText != null)
                    return;
                m_arrPinyinCode = new int[] { 
                    -20319, -20317, -20304, -20295, -20292, -20283, 
                    -20265, -20257, -20242, -20230, -20051, -20036, 
                    -20032, -20026, -20002, -19990, -19986, -19982, 
                    -19976, -19805, -19784, -19775, -19774, -19763, 
                    -19756, -19751, -19746, -19741, -19739, -19728, 
                    -19725, -19715, -19540, -19531, -19525, -19515, 
                    -19500, -19484, -19479, -19467, -19289, -19288, 
                    -19281, -19275, -19270, -19263, -19261, -19249, 
                    -19243, -19242, -19238, -19235, -19227, -19224,
                    -19218, -19212, -19038, -19023, -19018, -19006, 
                    -19003, -18996, -18977, -18961, -18952, -18783, 
                    -18774, -18773, -18763, -18756, -18741, -18735,
                    -18731, -18722, -18710, -18697, -18696, -18526, 
                    -18518, -18501, -18490, -18478, -18463, -18448,
                    -18447, -18446, -18239, -18237, -18231, -18220,
                    -18211, -18201, -18184, -18183, -18181, -18012, 
                    -17997, -17988, -17970, -17964, -17961, -17950,
                    -17947, -17931, -17928, -17922, -17759, -17752, 
                    -17733, -17730, -17721, -17703, -17701, -17697, 
                    -17692, -17683, -17676, -17496, -17487, -17482, 
                    -17468, -17454, -17433, -17427, -17417, -17202, 
                    -17185, -16983, -16970, -16942, -16915, -16733, 
                    -16708, -16706, -16689, -16664, -16657, -16647, 
                    -16474, -16470, -16465, -16459, -16452, -16448, 
                    -16433, -16429, -16427, -16423, -16419, -16412, 
                    -16407, -16403, -16401, -16393, -16220, -16216, 
                    -16212, -16205, -16202, -16187, -16180, -16171,
                    -16169, -16158, -16155, -15959, -15958, -15944,
                    -15933, -15920, -15915, -15903, -15889, -15878, 
                    -15707, -15701, -15681, -15667, -15661, -15659,
                    -15652, -15640, -15631, -15625, -15454, -15448,
                    -15436, -15435, -15419, -15416, -15408, -15394,
                    -15385, -15377, -15375, -15369, -15363, -15362,
                    -15183, -15180, -15165, -15158, -15153, -15150, 
                    -15149, -15144, -15143, -15141, -15140, -15139,
                    -15128, -15121, -15119, -15117, -15110, -15109,
                    -14941, -14937, -14933, -14930, -14929, -14928,
                    -14926, -14922, -14921, -14914, -14908, -14902,
                    -14894, -14889, -14882, -14873, -14871, -14857,
                    -14678, -14674, -14670, -14668, -14663, -14654,
                    -14645, -14630, -14594, -14429, -14407, -14399,
                    -14384, -14379, -14368, -14355, -14353, -14345,
                    -14170, -14159, -14151, -14149, -14145, -14140,
                    -14137, -14135, -14125, -14123, -14122, -14112,
                    -14109, -14099, -14097, -14094, -14092, -14090, 
                    -14087, -14083, -13917, -13914, -13910, -13907,
                    -13906, -13905, -13896, -13894, -13878, -13870, 
                    -13859, -13847, -13831, -13658, -13611, -13601, 
                    -13406, -13404, -13400, -13398, -13395, -13391,
                    -13387, -13383, -13367, -13359, -13356, -13343, 
                    -13340, -13329, -13326, -13318, -13147, -13138,
                    -13120, -13107, -13096, -13095, -13091, -13076,
                    -13068, -13063, -13060, -12888, -12875, -12871,
                    -12860, -12858, -12852, -12849, -12838, -12831,
                    -12829, -12812, -12802, -12607, -12597, -12594,
                    -12585, -12556, -12359, -12346, -12320, -12300,
                    -12120, -12099, -12089, -12074, -12067, -12058,
                    -12039, -11867, -11861, -11847, -11831, -11798,
                    -11781, -11604, -11589, -11536, -11358, -11340,
                    -11339, -11324, -11303, -11097, -11077, -11067, 
                    -11055, -11052, -11045, -11041, -11038, -11024, 
                    -11020, -11019, -11018, -11014, -10838, -10832,
                    -10815, -10800, -10790, -10780, -10764, -10587,
                    -10544, -10533, -10519, -10331, -10329, -10328,
                    -10322, -10315, -10309, -10307, -10296, -10281,
                    -10274, -10270, -10262, -10260, -10256, -10254 
                };

                m_arrPinyinText = new string[] { 
                    "a", "ai", "an", "ang", "ao", "ba", 
                    "bai","ban", "bang", "bao", "bei", "ben", 
                    "beng","bi", "bian", "biao", "bie", "bin",
                    "bing","bo", "bu", "ca", "cai", "can", 
                    "cang", "cao", "ce", "ceng", "cha", "chai", 
                    "chan","chang", "chao", "che", "chen", "cheng",
                    "chi","chong", "chou", "chu", "chuai", "chuan", 
                    "chuang", "chui", "chun", "chuo", "ci", "cong",
                    "cou", "cu", "cuan", "cui", "cun", "cuo", 
                    "da", "dai", "dan", "dang", "dao", "de",
                    "deng", "di", "dian", "diao", "die", "ding",
                    "diu", "dong", "dou", "du", "duan", "dui", 
                    "dun", "duo", "e", "en", "er", "fa", 
                    "fan", "fang", "fei", "fen", "feng", "fo", 
                    "fou", "fu", "ga", "gai", "gan", "gang", 
                    "gao", "ge", "gei", "gen", "geng", "gong", 
                    "gou", "gu", "gua", "guai", "guan", "guang", 
                    "gui", "gun", "guo", "ha", "hai", "han", 
                    "hang", "hao", "he", "hei", "hen", "heng", 
                    "hong", "hou", "hu", "hua", "huai", "huan", 
                    "huang", "hui", "hun", "huo", "ji", "jia", 
                    "jian", "jiang", "jiao", "jie", "jin", "jing", 
                    "jiong", "jiu", "ju", "juan", "jue", "jun", 
                    "ka", "kai", "kan", "kang", "kao", "ke",
                    "ken","keng", "kong", "kou", "ku", "kua",
                    "kuai","kuan", "kuang", "kui", "kun", "kuo",
                    "la","lai", "lan", "lang", "lao", "le",
                    "lei","leng", "li", "lia", "lian", "liang",
                    "liao","lie", "lin", "ling", "liu", "long",
                    "lou","lu", "lv", "luan", "lue", "lun", 
                    "luo","ma", "mai", "man", "mang", "mao",
                    "me", "mei", "men", "meng", "mi", "mian",
                    "miao", "mie", "min", "ming", "miu", "mo", 
                    "mou", "mu", "na", "nai", "nan", "nang",
                    "nao", "ne", "nei", "nen", "neng", "ni",
                    "nian", "niang", "niao", "nie", "nin", "ning", 
                    "niu", "nong","nu", "nv", "nuan", "nue",
                    "nuo", "o", "ou", "pa", "pai", "pan",
                    "pang", "pao", "pei", "pen", "peng", "pi", 
                    "pian", "piao", "pie", "pin", "ping", "po", 
                    "pu", "qi", "qia", "qian", "qiang", "qiao",
                    "qie", "qin", "qing", "qiong", "qiu", "qu",
                    "quan", "que", "qun", "ran", "rang", "rao",
                    "re", "ren", "reng", "ri", "rong", "rou",
                    "ru", "ruan", "rui", "run", "ruo", "sa",
                    "sai", "san", "sang", "sao", "se", "sen", 
                    "seng", "sha","shai", "shan", "shang", "shao",
                    "she", "shen","sheng", "shi", "shou", "shu",
                    "shua", "shuai","shuan", "shuang", "shui", "shun",
                    "shuo", "si","song", "sou", "su", "suan",
                    "sui", "sun", "suo", "ta", "tai", "tan", 
                    "tang", "tao", "te", "teng", "ti", "tian", 
                    "tiao", "tie", "ting", "tong", "tou", "tu",
                    "tuan", "tui", "tun", "tuo", "wa", "wai",
                    "wan", "wang", "wei", "wen", "weng", "wo",
                    "wu", "xi", "xia", "xian", "xiang", "xiao", 
                    "xie", "xin", "xing", "xiong", "xiu", "xu", 
                    "xuan", "xue", "xun", "ya", "yan", "yang",
                    "yao", "ye", "yi", "yin", "ying", "yo",
                    "yong", "you", "yu", "yuan", "yue", "yun",
                    "za", "zai", "zan", "zang", "zao", "ze",
                    "zei", "zen", "zeng", "zha", "zhai", "zhan", 
                    "zhang", "zhao", "zhe", "zhen", "zheng", "zhi", 
                    "zhong", "zhou", "zhu", "zhua", "zhuai", "zhuan",
                    "zhuang", "zhui", "zhun", "zhuo", "zi", "zong", 
                    "zou", "zu", "zuan", "zui", "zun", "zuo" 
                };
            }
            #endregion

            /// <summary>
            /// ��ָ�������ַ���ת��Ϊƴ����ʽ
            /// </summary>
            /// <param name="chsText">Ҫת���������ַ���</param>
            /// <param name="separator">����ƴ��֮��ķָ���</param>
            /// <param name="firstCapital">ָ���Ƿ�����ĸ��д</param>
            /// <returns>���������ַ�����ƴ�����ַ���</returns>
            public static string ChsToPinyin(string chsText, string separator, bool firstCapital)
            {
                if (string.IsNullOrEmpty(chsText))
                    return string.Empty;

                InitPinyinTable();

                StringBuilder sbPinyin = new StringBuilder();
                Encoding ansiEncoding = Convert.GetDefaultEncoding();
                System.Globalization.TextInfo currTextInfo =
                    System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;

                for (int index = 0; index < chsText.Length; index++)
                {
                    string szCurrChar = chsText[index].ToString(); ;
                    byte[] arrCharBytes = ansiEncoding.GetBytes(szCurrChar);
                    if (arrCharBytes.Length != 2)
                    {
                        sbPinyin.Append(szCurrChar);
                        continue;
                    }

                    int nLowByte = arrCharBytes[0];
                    int nHighByte = arrCharBytes[1];
                    int nCharCode = nLowByte * 256 + nHighByte - 65536;
                    int nPinyinIndex = m_arrPinyinCode.Length - 1;
                    for (; nPinyinIndex >= 0; nPinyinIndex--)
                    {
                        if (m_arrPinyinCode[nPinyinIndex] > nCharCode)
                            continue;

                        szCurrChar = m_arrPinyinText[nPinyinIndex];
                        if (firstCapital)
                            szCurrChar = currTextInfo.ToTitleCase(szCurrChar);
                        if (sbPinyin.Length > 0)
                            sbPinyin.Append(separator);
                        sbPinyin.Append(szCurrChar);
                        break;
                    }
                    if (nPinyinIndex < 0) sbPinyin.Append(szCurrChar);
                }
                return sbPinyin.ToString();
            }

            #region "ƴ���������"
            /// <summary>
            /// ������ת��ʹ�õ��������
            /// </summary>
            private static string m_szInputCodeText = null;
            /// <summary>
            /// ��ʼ��������ת��ʹ�õ��������
            /// </summary>
            private static void InitInputCodeTable()
            {
                if (m_szInputCodeText != null)
                    return;
                StringBuilder sbInputCode = new StringBuilder();
                sbInputCode.Append("YDYQSXMWZSSXJBYMGCCZQPSSQBYCDSCDQLDYLYBSSJGYZ");
                sbInputCode.Append("ZJJFKCCLZDHWDWZJLJPFYYNWJJTMYHZWZHFLZPPQHGSCY");
                sbInputCode.Append("YYNJQYXXGJHHSDSJNKKTMOMLCRXYPSNQSECCQZGGLLYJL");
                sbInputCode.Append("MYZZSECYKYYHQWJSSGGYXYZYJWWKDJHYCHMYXJTLXJYQB");
                sbInputCode.Append("YXZLDWRDJRWYSRLDZJPCBZJJBRCFTLECZSTZFXXZHTRQH");
                sbInputCode.Append("YBDLYCZSSYMMRFMYQZPWWJJYFCRWFDFZQPYDDWYXKYJAW");
                sbInputCode.Append("JFFXYPSFTZYHHYZYSWCJYXSCLCXXWZZXNBGNNXBXLZSZS");
                sbInputCode.Append("BSGPYSYZDHMDZBQBZCWDZZYYTZHBTSYYBZGNTNXQYWQSK");
                sbInputCode.Append("BPHHLXGYBFMJEBJHHGQTJCYSXSTKZHLYCKGLYSMZXYALM");
                sbInputCode.Append("ELDCCXGZYRJXSDLTYZCQKCNNJWHJTZZCQLJSTSTBNXBTY");
                sbInputCode.Append("XCEQXGKWJYFLZQLYHYXSPSFXLMPBYSXXXYDJCZYLLLSJX");
                sbInputCode.Append("FHJXPJBTFFYABYXBHZZBJYZLWLCZGGBTSSMDTJZXPTHYQ");
                sbInputCode.Append("TGLJSCQFZKJZJQNLZWLSLHDZBWJNCJZYZSQQYCQYRZCJJ");
                sbInputCode.Append("WYBRTWPYFTWEXCSKDZCTBZHYZZYYJXZCFFZZMJYXXSDZZ");
                sbInputCode.Append("OTTBZLQWFCKSZSXFYRLNYJMBDTHJXSQQCCSBXYYTSYFBX");
                sbInputCode.Append("DZTGBCNSLCYZZPSAZYZZSCJCSHZQYDXLBPJLLMQXTYDZX");
                sbInputCode.Append("SQJTZPXLCGLQTZWJBHCTSYJSFXYEJJTLBGXSXJMYJQQPF");
                sbInputCode.Append("ZASYJNTYDJXKJCDJSZCBARTDCLYJQMWNQNCLLLKBYBZZS");
                sbInputCode.Append("YHQQLTWLCCXTXLLZNTYLNEWYZYXCZXXGRKRMTCNDNJTSY");
                sbInputCode.Append("YSSDQDGHSDBJGHRWRQLYBGLXHLGTGXBQJDZPYJSJYJCTM");
                sbInputCode.Append("RNYMGRZJCZGJMZMGXMPRYXKJNYMSGMZJYMKMFXMLDTGFB");
                sbInputCode.Append("HCJHKYLPFMDXLQJJSMTQGZSJLQDLDGJYCALCMZCSDJLLN");
                sbInputCode.Append("XDJFFFFJCZFMZFFPFKHKGDPSXKTACJDHHZDDCRRCFQYJK");
                sbInputCode.Append("QCCWJDXHWJLYLLZGCFCQDSMLZPBJJPLSBCJGGDCKKDEZS");
                sbInputCode.Append("QCCKJGCGKDJTJDLZYCXKLQSCGJCLTFPCQCZGWPJDQYZJJ");
                sbInputCode.Append("BYJHSJDZWGFSJGZKQCCZLLPSPKJGQJHZZLJPLGJGJJTHJ");
                sbInputCode.Append("JYJZCZMLZLYQBGJWMLJKXZDZNJQSYZMLJLLJKYWXMKJLH");
                sbInputCode.Append("SKJGBMCLYYMKXJQLBMLLKMDXXKWYXYSLMLPSJQQJQXYXF");
                sbInputCode.Append("JTJDXMXXLLCXQBSYJBGWYMBGGBCYXPJYGPEPFGDJGBHBN");
                sbInputCode.Append("SQJYZJKJKHXQFGQZKFHYGKHDKLLSDJQXPQYKYBNQSXQNS");
                sbInputCode.Append("ZSWHBSXWHXWBZZXDMNSJBSBKBBZKLYLXGWXDRWYQZMYWS");
                sbInputCode.Append("JQLCJXXJXKJEQXSCYETLZHLYYYSDZPAQYZCMTLSHTZCFY");
                sbInputCode.Append("ZYXYLJSDCJQAGYSLCQLYYYSHMRQQKLDXZSCSSSYDYCJYS");
                sbInputCode.Append("FSJBFRSSZQSBXXPXJYSDRCKGJLGDKZJZBDKTCSYQPYHST");
                sbInputCode.Append("CLDJDHMXMCGXYZHJDDTMHLTXZXYLYMOHYJCLTYFBQQXPF");
                sbInputCode.Append("BDFHHTKSQHZYYWCNXXCRWHOWGYJLEGWDQCWGFJYCSNTMY");
                sbInputCode.Append("TOLBYGWQWESJPWNMLRYDZSZTXYQPZGCWXHNGPYXSHMYQJ");
                sbInputCode.Append("XZTDPPBFYHZHTJYFDZWKGKZBLDNTSXHQEEGZZYLZMMZYJ");
                sbInputCode.Append("ZGXZXKHKSTXNXXWYLYAPSTHXDWHZYMPXAGKYDXBHNHXKD");
                sbInputCode.Append("PJNMYHYLPMGOCSLNZHKXXLPZZLBMLSFBHHGYGYYGGBHSC");
                sbInputCode.Append("YAQTYWLXTZQCEZYDQDQMMHTKLLSZHLSJZWFYHQSWSCWLQ");
                sbInputCode.Append("AZYNYTLSXTHAZNKZZSZZLAXXZWWCTGQQTDDYZTCCHYQZF");
                sbInputCode.Append("LXPSLZYGPZSZNGLNDQTBDLXGTCTAJDKYWNSYZLJHHZZCW");
                sbInputCode.Append("NYYZYWMHYCHHYXHJKZWSXHZYXLYSKQYSPSLYZWMYPPKBY");
                sbInputCode.Append("GLKZHTYXAXQSYSHXASMCHKDSCRSWJPWXSGZJLWWSCHSJH");
                sbInputCode.Append("SQNHCSEGNDAQTBAALZZMSSTDQJCJKTSCJAXPLGGXHHGXX");
                sbInputCode.Append("ZCXPDMMHLDGTYBYSJMXHMRCPXXJZCKZXSHMLQXXTTHXWZ");
                sbInputCode.Append("FKHCCZDYTCJYXQHLXDHYPJQXYLSYYDZOZJNYXQEZYSQYA");
                sbInputCode.Append("YXWYPDGXDDXSPPYZNDLTWRHXYDXZZJHTCXMCZLHPYYYYM");
                sbInputCode.Append("HZLLHNXMYLLLMDCPPXHMXDKYCYRDLTXJCHHZZXZLCCLYL");
                sbInputCode.Append("NZSHZJZZLNNRLWHYQSNJHXYNTTTKYJPYCHHYEGKCTTWLG");
                sbInputCode.Append("QRLGGTGTYGYHPYHYLQYQGCWYQKPYYYTTTTLHYHLLTYTTS");
                sbInputCode.Append("PLKYZXGZWGPYDSSZZDQXSKCQNMJJZZBXYQMJRTFFBTKHZ");
                sbInputCode.Append("KBXLJJKDXJTLBWFZPPTKQTZTGPDGNTPJYFALQMKGXBDCL");
                sbInputCode.Append("ZFHZCLLLLADPMXDJHLCCLGYHDZFGYDDGCYYFGYDXKSSEB");
                sbInputCode.Append("DHYKDKDKHNAXXYBPBYYHXZQGAFFQYJXDMLJCSQZLLPCHB");
                sbInputCode.Append("SXGJYNDYBYQSPZWJLZKSDDTACTBXZDYZYPJZQSJNKKTKN");
                sbInputCode.Append("JDJGYYPGTLFYQKASDNTCYHBLWDZHBBYDWJRYGKZYHEYYF");
                sbInputCode.Append("JMSDTYFZJJHGCXPLXHLDWXXJKYTCYKSSSMTWCTTQZLPBS");
                sbInputCode.Append("ZDZWZXGZAGYKTYWXLHLSPBCLLOQMMZSSLCMBJCSZZKYDC");
                sbInputCode.Append("ZJGQQDSMCYTZQQLWZQZXSSFPTTFQMDDZDSHDTDWFHTDYZ");
                sbInputCode.Append("JYQJQKYPBDJYYXTLJHDRQXXXHAYDHRJLKLYTWHLLRLLRC");
                sbInputCode.Append("XYLBWSRSZZSYMKZZHHKYHXKSMDSYDYCJPBZBSQLFCXXXN");
                sbInputCode.Append("XKXWYWSDZYQOGGQMMYHCDZTTFJYYBGSTTTYBYKJDHKYXB");
                sbInputCode.Append("ELHTYPJQNFXFDYKZHQKZBYJTZBXHFDXKDASWTAWAJLDYJ");
                sbInputCode.Append("SFHBLDNNTNQJTJNCHXFJSRFWHZFMDRYJYJWZPDJKZYJYM");
                sbInputCode.Append("PCYZNYNXFBYTFYFWYGDBNZZZDNYTXZEMMQBSQEHXFZMBM");
                sbInputCode.Append("FLZZSRXYMJGSXWZJSPRYDJSJGXHJJGLJJYNZZJXHGXKYM");
                sbInputCode.Append("LPYYYCXYTWQZSWHWLYRJLPXSLSXMFSWWKLCTNXNYNPSJS");
                sbInputCode.Append("ZHDZEPTXMYYWXYYSYWLXJQZQXZDCLEEELMCPJPCLWBXSQ");
                sbInputCode.Append("HFWWTFFJTNQJHJQDXHWLBYZNFJLALKYYJLDXHHYCSTYYW");
                sbInputCode.Append("NRJYXYWTRMDRQHWQCMFJDYZMHMYYXJWMYZQZXTLMRSPWW");
                sbInputCode.Append("CHAQBXYGZYPXYYRRCLMPYMGKSJSZYSRMYJSNXTPLNBAPP");
                sbInputCode.Append("YPYLXYYZKYNLDZYJZCZNNLMZHHARQMPGWQTZMXXMLLHGD");
                sbInputCode.Append("ZXYHXKYXYCJMFFYYHJFSBSSQLXXNDYCANNMTCJCYPRRNY");
                sbInputCode.Append("TYQNYYMBMSXNDLYLYSLJRLXYSXQMLLYZLZJJJKYZZCSFB");
                sbInputCode.Append("ZXXMSTBJGNXYZHLXNMCWSCYZYFZLXBRNNNYLBNRTGZQYS");
                sbInputCode.Append("ATSWRYHYJZMZDHZGZDWYBSSCSKXSYHYTXXGCQGXZZSHYX");
                sbInputCode.Append("JSCRHMKKBXCZJYJYMKQHZJFNBHMQHYSNJNZYBKNQMCLGQ");
                sbInputCode.Append("HWLZNZSWXKHLJHYYBQLBFCDSXDLDSPFZPSKJYZWZXZDDX");
                sbInputCode.Append("JSMMEGJSCSSMGCLXXKYYYLNYPWWWGYDKZJGGGZGGSYCKN");
                sbInputCode.Append("JWNJPCXBJJTQTJWDSSPJXZXNZXUMELPXFSXTLLXCLJXJJ");
                sbInputCode.Append("LJZXCTPSWXLYDHLYQRWHSYCSQYYBYAYWJJJQFWQCQQCJQ");
                sbInputCode.Append("GXALDBZZYJGKGXPLTZYFXJLTPADKYQHPMATLCPDCKBMTX");
                sbInputCode.Append("YBHKLENXDLEEGQDYMSAWHZMLJTWYGXLYQZLJEEYYBQQFF");
                sbInputCode.Append("NLYXRDSCTGJGXYYNKLLYQKCCTLHJLQMKKZGCYYGLLLJDZ");
                sbInputCode.Append("GYDHZWXPYSJBZKDZGYZZHYWYFQYTYZSZYEZZLYMHJJHTS");
                sbInputCode.Append("MQWYZLKYYWZCSRKQYTLTDXWCTYJKLWSQZWBDCQYNCJSRS");
                sbInputCode.Append("ZJLKCDCDTLZZZACQQZZDDXYPLXZBQJYLZLLLQDDZQJYJY");
                sbInputCode.Append("JZYXNYYYNYJXKXDAZWYRDLJYYYRJLXLLDYXJCYWYWNQCC");
                sbInputCode.Append("LDDNYYYNYCKCZHXXCCLGZQJGKWPPCQQJYSBZZXYJSQPXJ");
                sbInputCode.Append("PZBSBDSFNSFPZXHDWZTDWPPTFLZZBZDMYYPQJRSDZSQZS");
                sbInputCode.Append("QXBDGCPZSWDWCSQZGMDHZXMWWFYBPDGPHTMJTHZSMMBGZ");
                sbInputCode.Append("MBZJCFZWFZBBZMQCFMBDMCJXLGPNJBBXGYHYYJGPTZGZM");
                sbInputCode.Append("QBQTCGYXJXLWZKYDPDYMGCFTPFXYZTZXDZXTGKMTYBBCL");
                sbInputCode.Append("BJASKYTSSQYYMSZXFJEWLXLLSZBQJJJAKLYLXLYCCTSXM");
                sbInputCode.Append("CWFKKKBSXLLLLJYXTYLTJYYTDPJHNHNNKBYQNFQYYZBYY");
                sbInputCode.Append("ESSESSGDYHFHWTCJBSDZZTFDMXHCNJZYMQWSRYJDZJQPD");
                sbInputCode.Append("QBBSTJGGFBKJBXTGQHNGWJXJGDLLTHZHHYYYYYYSXWTYY");
                sbInputCode.Append("YCCBDBPYPZYCCZYJPZYWCBDLFWZCWJDXXHYHLHWZZXJTC");
                sbInputCode.Append("ZLCDPXUJCZZZLYXJJTXPHFXWPYWXZPTDZZBDZCYHJHMLX");
                sbInputCode.Append("BQXSBYLRDTGJRRCTTTHYTCZWMXFYTWWZCWJWXJYWCSKYB");
                sbInputCode.Append("ZSCCTZQNHXNWXXKHKFHTSWOCCJYBCMPZZYKBNNZPBZHHZ");
                sbInputCode.Append("DLSYDDYTYFJPXYNGFXBYQXCBHXCPSXTYZDMKYSNXSXLHK");
                sbInputCode.Append("MZXLYHDHKWHXXSSKQYHHCJYXGLHZXCSNHEKDTGZXQYPKD");
                sbInputCode.Append("HEXTYKCNYMYYYPKQYYYKXZLTHJQTBYQHXBMYHSQCKWWYL");
                sbInputCode.Append("LHCYYLNNEQXQWMCFBDCCMLJGGXDQKTLXKGNQCDGZJWYJJ");
                sbInputCode.Append("LYHHQTTTNWCHMXCXWHWSZJYDJCCDBQCDGDNYXZTHCQRXC");
                sbInputCode.Append("BHZTQCBXWGQWYYBXHMBYMYQTYEXMQKYAQYRGYZSLFYKKQ");
                sbInputCode.Append("HYSSQYSHJGJCNXKZYCXSBXYXHYYLSTYCXQTHYSMGSCPMM");
                sbInputCode.Append("GCCCCCMTZTASMGQZJHKLOSQYLSWTMXSYQKDZLJQQYPLSY");
                sbInputCode.Append("CZTCQQPBBQJZCLPKHQZYYXXDTDDTSJCXFFLLCHQXMJLWC");
                sbInputCode.Append("JCXTSPYCXNDTJSHJWXDQQJSKXYAMYLSJHMLALYKXCYYDM");
                sbInputCode.Append("NMDQMXMCZNNCYBZKKYFLMCHCMLHXRCJJHSYLNMTJZGZGY");
                sbInputCode.Append("WJXSRXCWJGJQHQZDQJDCJJZKJKGDZQGJJYJYLXZXXCDQH");
                sbInputCode.Append("HHEYTMHLFSBDJSYYSHFYSTCZQLPBDRFRZTZYKYWHSZYQK");
                sbInputCode.Append("WDQZRKMSYNBCRXQBJYFAZPZZEDZCJYWBCJWHYJBQSZYWR");
                sbInputCode.Append("YSZPTDKZPFPBNZTKLQYHBBZPNPPTYZZYBQNYDCPJMMCYC");
                sbInputCode.Append("QMCYFZZDCMNLFPBPLNGQJTBTTNJZPZBBZNJKLJQYLNBZQ");
                sbInputCode.Append("HKSJZNGGQSZZKYXSHPZSNBCGZKDDZQANZHJKDRTLZLSWJ");
                sbInputCode.Append("LJZLYWTJNDJZJHXYAYNCBGTZCSSQMNJPJYTYSWXZFKWJQ");
                sbInputCode.Append("TKHTZPLBHSNJZSYZBWZZZZLSYLSBJHDWWQPSLMMFBJDWA");
                sbInputCode.Append("QYZTCJTBNNWZXQXCDSLQGDSDPDZHJTQQPSWLYYJZLGYXY");
                sbInputCode.Append("ZLCTCBJTKTYCZJTQKBSJLGMGZDMCSGPYNJZYQYYKNXRPW");
                sbInputCode.Append("SZXMTNCSZZYXYBYHYZAXYWQCJTLLCKJJTJHGDXDXYQYZZ");
                sbInputCode.Append("BYWDLWQCGLZGJGQRQZCZSSBCRPCSKYDZNXJSQGXSSJMYD");
                sbInputCode.Append("NSTZTPBDLTKZWXQWQTZEXNQCZGWEZKSSBYBRTSSSLCCGB");
                sbInputCode.Append("PSZQSZLCCGLLLZXHZQTHCZMQGYZQZNMCOCSZJMMZSQPJY");
                sbInputCode.Append("GQLJYJPPLDXRGZYXCCSXHSHGTZNLZWZKJCXTCFCJXLBMQ");
                sbInputCode.Append("BCZZWPQDNHXLJCTHYZLGYLNLSZZPCXDSCQQHJQKSXZPBA");
                sbInputCode.Append("JYEMSMJTZDXLCJYRYYNWJBNGZZTMJXLTBSLYRZPYLSSCN");
                sbInputCode.Append("XPHLLHYLLQQZQLXYMRSYCXZLMMCZLTZSDWTJJLLNZGGQX");
                sbInputCode.Append("PFSKYGYGHBFZPDKMWGHCXMSGDXJMCJZDYCABXJDLNBCDQ");
                sbInputCode.Append("YGSKYDQTXDJJYXMSZQAZDZFSLQXYJSJZYLBTXXWXQQZBJ");
                sbInputCode.Append("ZUFBBLYLWDSLJHXJYZJWTDJCZFQZQZZDZSXZZQLZCDZFJ");
                sbInputCode.Append("HYSPYMPQZMLPPLFFXJJNZZYLSJEYQZFPFZKSYWJJJHRDJ");
                sbInputCode.Append("ZZXTXXGLGHYDXCSKYSWMMZCWYBAZBJKSHFHJCXMHFQHYX");
                sbInputCode.Append("XYZFTSJYZFXYXPZLCHMZMBXHZZSXYFYMNCWDABAZLXKTC");
                sbInputCode.Append("SHHXKXJJZJSTHYGXSXYYHHHJWXKZXSSBZZWHHHCWTZZZP");
                sbInputCode.Append("JXSNXQQJGZYZYWLLCWXZFXXYXYHXMKYYSWSQMNLNAYCYS");
                sbInputCode.Append("PMJKHWCQHYLAJJMZXHMMCNZHBHXCLXTJPLTXYJHDYYLTT");
                sbInputCode.Append("XFSZHYXXSJBJYAYRSMXYPLCKDUYHLXRLNLLSTYZYYQYGY");
                sbInputCode.Append("HHSCCSMZCTZQXKYQFPYYRPFFLKQUNTSZLLZMWWTCQQYZW");
                sbInputCode.Append("TLLMLMPWMBZSSTZRBPDDTLQJJBXZCSRZQQYGWCSXFWZLX");
                sbInputCode.Append("CCRSZDZMCYGGDZQSGTJSWLJMYMMZYHFBJDGYXCCPSHXNZ");
                sbInputCode.Append("CSBSJYJGJMPPWAFFYFNXHYZXZYLREMZGZCYZSSZDLLJCS");
                sbInputCode.Append("QFNXZKPTXZGXJJGFMYYYSNBTYLBNLHPFZDCYFBMGQRRSS");
                sbInputCode.Append("SZXYSGTZRNYDZZCDGPJAFJFZKNZBLCZSZPSGCYCJSZLML");
                sbInputCode.Append("RSZBZZLDLSLLYSXSQZQLYXZLSKKBRXBRBZCYCXZZZEEYF");
                sbInputCode.Append("GKLZLYYHGZSGZLFJHGTGWKRAAJYZKZQTSSHJJXDCYZUYJ");
                sbInputCode.Append("LZYRZDQQHGJZXSSZBYKJPBFRTJXLLFQWJHYLQTYMBLPZD");
                sbInputCode.Append("XTZYGBDHZZRBGXHWNJTJXLKSCFSMWLSDQYSJTXKZSCFWJ");
                sbInputCode.Append("LBXFTZLLJZLLQBLSQMQQCGCZFPBPHZCZJLPYYGGDTGWDC");
                sbInputCode.Append("FCZQYYYQYSSCLXZSKLZZZGFFCQNWGLHQYZJJCZLQZZYJP");
                sbInputCode.Append("JZZBPDCCMHJGXDQDGDLZQMFGPSYTSDYFWWDJZJYSXYYCZ");
                sbInputCode.Append("CYHZWPBYKXRYLYBHKJKSFXTZJMMCKHLLTNYYMSYXYZPYJ");
                sbInputCode.Append("QYCSYCWMTJJKQYRHLLQXPSGTLYYCLJSCPXJYZFNMLRGJJ");
                sbInputCode.Append("TYZBXYZMSJYJHHFZQMSYXRSZCWTLRTQZSSTKXGQKGSPTG");
                sbInputCode.Append("CZNJSJCQCXHMXGGZTQYDJKZDLBZSXJLHYQGGGTHQSZPYH");
                sbInputCode.Append("JHHGYYGKGGCWJZZYLCZLXQSFTGZSLLLMLJSKCTBLLZZSZ");
                sbInputCode.Append("MMNYTPZSXQHJCJYQXYZXZQZCPSHKZZYSXCDFGMWQRLLQX");
                sbInputCode.Append("RFZTLYSTCTMJCXJJXHJNXTNRZTZFQYHQGLLGCXSZSJDJL");
                sbInputCode.Append("JCYDSJTLNYXHSZXCGJZYQPYLFHDJSBPCCZHJJJQZJQDYB");
                sbInputCode.Append("SSLLCMYTTMQTBHJQNNYGKYRQYQMZGCJKPDCGMYZHQLLSL");
                sbInputCode.Append("LCLMHOLZGDYYFZSLJCQZLYLZQJESHNYLLJXGJXLYSYYYX");
                sbInputCode.Append("NBZLJSSZCQQCJYLLZLTJYLLZLLBNYLGQCHXYYXOXCXQKY");
                sbInputCode.Append("JXXXYKLXSXXYQXCYKQXQCSGYXXYQXYGYTQOHXHXPYXXXU");
                sbInputCode.Append("LCYEYCHZZCBWQBBWJQZSCSZSSLZYLKDESJZWMYMCYTSDS");
                sbInputCode.Append("XXSCJPQQSQYLYYZYCMDJDZYWCBTJSYDJKCYDDJLBDJJSO");
                sbInputCode.Append("DZYSYXQQYXDHHGQQYQHDYXWGMMMAJDYBBBPPBCMUUPLJZ");
                sbInputCode.Append("SMTXERXJMHQNUTPJDCBSSMSSSTKJTSSMMTRCPLZSZMLQD");
                sbInputCode.Append("SDMJMQPNQDXCFYNBFSDQXYXHYAYKQYDDLQYYYSSZBYDSL");
                sbInputCode.Append("NTFQTZQPZMCHDHCZCWFDXTMYQSPHQYYXSRGJCWTJTZZQM");
                sbInputCode.Append("GWJJTJHTQJBBHWZPXXHYQFXXQYWYYHYSCDYDHHQMNMTMW");
                sbInputCode.Append("CPBSZPPZZGLMZFOLLCFWHMMSJZTTDHZZYFFYTZZGZYSKY");
                sbInputCode.Append("JXQYJZQBHMBZZLYGHGFMSHPZFZSNCLPBQSNJXZSLXXFPM");
                sbInputCode.Append("TYJYGBXLLDLXPZJYZJYHHZCYWHJYLSJEXFSZZYWXKZJLU");
                sbInputCode.Append("YDTMLYMQJPWXYHXSKTQJEZRPXXZHHMHWQPWQLYJJQJJZS");
                sbInputCode.Append("ZCPHJLCHHNXJLQWZJHBMZYXBDHHYPZLHLHLGFWLCHYYTL");
                sbInputCode.Append("HJXCJMSCPXSTKPNHQXSRTYXXTESYJCTLSSLSTDLLLWWYH");
                sbInputCode.Append("DHRJZSFGXTSYCZYNYHTDHWJSLHTZDQDJZXXQHGYLTZPHC");
                sbInputCode.Append("SQFCLNJTCLZPFSTPDYNYLGMJLLYCQHYSSHCHYLHQYQTMZ");
                sbInputCode.Append("YPBYWRFQYKQSYSLZDQJMPXYYSSRHZJNYWTQDFZBWWTWWR");
                sbInputCode.Append("XCWHGYHXMKMYYYQMSMZHNGCEPMLQQMTCWCTMMPXJPJJHF");
                sbInputCode.Append("XYYZSXZHTYBMSTSYJTTQQQYYLHYNPYQZLCYZHZWSMYLKF");
                sbInputCode.Append("JXLWGXYPJYTYSYXYMZCKTTWLKSMZSYLMPWLZWXWQZSSAQ");
                sbInputCode.Append("SYXYRHSSNTSRAPXCPWCMGDXHXZDZYFJHGZTTSBJHGYZSZ");
                sbInputCode.Append("YSMYCLLLXBTYXHBBZJKSSDMALXHYCFYGMQYPJYCQXJLLL");
                sbInputCode.Append("JGSLZGQLYCJCCZOTYXMTMTTLLWTGPXYMZMKLPSZZZXHKQ");
                sbInputCode.Append("YSXCTYJZYHXSHYXZKXLZWPSQPYHJWPJPWXQQYLXSDHMRS");
                sbInputCode.Append("LZZYZWTTCYXYSZZSHBSCCSTPLWSSCJCHNLCGCHSSPHYLH");
                sbInputCode.Append("FHHXJSXYLLNYLSZDHZXYLSXLWZYKCLDYAXZCMDDYSPJTQ");
                sbInputCode.Append("JZLNWQPSSSWCTSTSZLBLNXSMNYYMJQBQHRZWTYYDCHQLX");
                sbInputCode.Append("KPZWBGQYBKFCMZWPZLLYYLSZYDWHXPSBCMLJBSCGBHXLQ");
                sbInputCode.Append("HYRLJXYSWXWXZSLDFHLSLYNJLZYFLYJYCDRJLFSYZFSLL");
                sbInputCode.Append("CQYQFGJYHYXZLYLMSTDJCYHBZLLNWLXXYGYYHSMGDHXXH");
                sbInputCode.Append("HLZZJZXCZZZCYQZFNGWPYLCPKPYYPMCLQKDGXZGGWQBDX");
                sbInputCode.Append("ZZKZFBXXLZXJTPJPTTBYTSZZDWSLCHZHSLTYXHQLHYXXX");
                sbInputCode.Append("YYZYSWTXZKHLXZXZPYHGCHKCFSYHUTJRLXFJXPTZTWHPL");
                sbInputCode.Append("YXFCRHXSHXKYXXYHZQDXQWULHYHMJTBFLKHTXCWHJFWJC");
                sbInputCode.Append("FPQRYQXCYYYQYGRPYWSGSUNGWCHKZDXYFLXXHJJBYZWTS");
                sbInputCode.Append("XXNCYJJYMSWZJQRMHXZWFQSYLZJZGBHYNSLBGTTCSYBYX");
                sbInputCode.Append("XWXYHXYYXNSQYXMQYWRGYQLXBBZLJSYLPSYTJZYHYZAWL");
                sbInputCode.Append("RORJMKSCZJXXXYXCHDYXRYXXJDTSQFXLYLTSFFYXLMTYJ");
                sbInputCode.Append("MJUYYYXLTZCSXQZQHZXLYYXZHDNBRXXXJCTYHLBRLMBRL");
                sbInputCode.Append("LAXKYLLLJLYXXLYCRYLCJTGJCMTLZLLCYZZPZPCYAWHJJ");
                sbInputCode.Append("FYBDYYZSMPCKZDQYQPBPCJPDCYZMDPBCYYDYCNNPLMTML");
                sbInputCode.Append("RMFMMGWYZBSJGYGSMZQQQZTXMKQWGXLLPJGZBQCDJJJFP");
                sbInputCode.Append("KJKCXBLJMSWMDTQJXLDLPPBXCWRCQFBFQJCZAHZGMYKPH");
                sbInputCode.Append("YYHZYKNDKZMBPJYXPXYHLFPNYYGXJDBKXNXHJMZJXSTRS");
                sbInputCode.Append("TLDXSKZYSYBZXJLXYSLBZYSLHXJPFXPQNBYLLJQKYGZMC");
                sbInputCode.Append("YZZYMCCSLCLHZFWFWYXZMWSXTYNXJHPYYMCYSPMHYSMYD");
                sbInputCode.Append("YSHQYZCHMJJMZCAAGCFJBBHPLYZYLXXSDJGXDHKXXTXXN");
                sbInputCode.Append("BHRMLYJSLTXMRHNLXQJXYZLLYSWQGDLBJHDCGJYQYCMHW");
                sbInputCode.Append("FMJYBMBYJYJWYMDPWHXQLDYGPDFXXBCGJSPCKRSSYZJMS");
                sbInputCode.Append("LBZZJFLJJJLGXZGYXYXLSZQYXBEXYXHGCXBPLDYHWETTW");
                sbInputCode.Append("WCJMBTXCHXYQXLLXFLYXLLJLSSFWDPZSMYJCLMWYTCZPC");
                sbInputCode.Append("HQEKCQBWLCQYDPLQPPQZQFJQDJHYMMCXTXDRMJWRHXCJZ");
                sbInputCode.Append("YLQXDYYNHYYHRSLSRSYWWZJYMTLTLLGTQCJZYABTCKZCJ");
                sbInputCode.Append("YCCQLJZQXALMZYHYWLWDXZXQDLLQSHGPJFJLJHJABCQZD");
                sbInputCode.Append("JGTKHSSTCYJLPSWZLXZXRWGLDLZRLZXTGSLLLLZLYXXWG");
                sbInputCode.Append("DZYGBDPHZPBRLWSXQBPFDWOFMWHLYPCBJCCLDMBZPBZZL");
                sbInputCode.Append("CYQXLDOMZBLZWPDWYYGDSTTHCSQSCCRSSSYSLFYBFNTYJ");
                sbInputCode.Append("SZDFNDPDHDZZMBBLSLCMYFFGTJJQWFTMTPJWFNLBZCMMJ");
                sbInputCode.Append("TGBDZLQLPYFHYYMJYLSDCHDZJWJCCTLJCLDTLJJCPDDSQ");
                sbInputCode.Append("DSSZYBNDBJLGGJZXSXNLYCYBJXQYCBYLZCFZPPGKCXZDZ");
                sbInputCode.Append("FZTJJFJSJXZBNZYJQTTYJYHTYCZHYMDJXTTMPXSPLZCDW");
                sbInputCode.Append("SLSHXYPZGTFMLCJTYCBPMGDKWYCYZCDSZZYHFLYCTYGWH");
                sbInputCode.Append("KJYYLSJCXGYWJCBLLCSNDDBTZBSCLYZCZZSSQDLLMQYYH");
                sbInputCode.Append("FSLQLLXFTYHABXGWNYWYYPLLSDLDLLBJCYXJZMLHLJDXY");
                sbInputCode.Append("YQYTDLLLBUGBFDFBBQJZZMDPJHGCLGMJJPGAEHHBWCQXA");
                sbInputCode.Append("XHHHZCHXYPHJAXHLPHJPGPZJQCQZGJJZZUZDMQYYBZZPH");
                sbInputCode.Append("YHYBWHAZYJHYKFGDPFQSDLZMLJXKXGALXZDAGLMDGXMWZ");
                sbInputCode.Append("QYXXDXXPFDMMSSYMPFMDMMKXKSYZYSHDZKXSYSMMZZZMS");
                sbInputCode.Append("YDNZZCZXFPLSTMZDNMXCKJMZTYYMZMZZMSXHHDCZJEMXX");
                sbInputCode.Append("KLJSTLWLSQLYJZLLZJSSDPPMHNLZJCZYHMXXHGZCJMDHX");
                sbInputCode.Append("TKGRMXFWMCGMWKDTKSXQMMMFZZYDKMSCLCMPCGMHSPXQP");
                sbInputCode.Append("ZDSSLCXKYXTWLWJYAHZJGZQMCSNXYYMMPMLKJXMHLMLQM");
                sbInputCode.Append("XCTKZMJQYSZJSYSZHSYJZJCDAJZYBSDQJZGWZQQXFKDMS");
                sbInputCode.Append("DJLFWEHKZQKJPEYPZYSZCDWYJFFMZZYLTTDZZEFMZLBNP");
                sbInputCode.Append("PLPLPEPSZALLTYLKCKQZKGENQLWAGYXYDPXLHSXQQWQCQ");
                sbInputCode.Append("XQCLHYXXMLYCCWLYMQYSKGCHLCJNSZKPYZKCQZQLJPDMD");
                sbInputCode.Append("ZHLASXLBYDWQLWDNBQCRYDDZTJYBKBWSZDXDTNPJDTCTQ");
                sbInputCode.Append("DFXQQMGNXECLTTBKPWSLCTYQLPWYZZKLPYGZCQQPLLKCC");
                sbInputCode.Append("YLPQMZCZQCLJSLQZDJXLDDHPZQDLJJXZQDXYZQKZLJCYQ");
                sbInputCode.Append("DYJPPYPQYKJYRMPCBYMCXKLLZLLFQPYLLLMBSGLCYSSLR");
                sbInputCode.Append("SYSQTMXYXZQZFDZUYSYZTFFMZZSMZQHZSSCCMLYXWTPZG");
                sbInputCode.Append("XZJGZGSJSGKDDHTQGGZLLBJDZLCBCHYXYZHZFYWXYZYMS");
                sbInputCode.Append("DBZZYJGTSMTFXQYXQSTDGSLNXDLRYZZLRYYLXQHTXSRTZ");
                sbInputCode.Append("NGZXBNQQZFMYKMZJBZYMKBPNLYZPBLMCNQYZZZSJZHJCT");
                sbInputCode.Append("ZKHYZZJRDYZHNPXGLFZTLKGJTCTSSYLLGZRZBBQZZKLPK");
                sbInputCode.Append("LCZYSSUYXBJFPNJZZXCDWXZYJXZZDJJKGGRSRJKMSMZJL");
                sbInputCode.Append("SJYWQSKYHQJSXPJZZZLSNSHRNYPZTWCHKLPSRZLZXYJQX");
                sbInputCode.Append("QKYSJYCZTLQZYBBYBWZPQDWWYZCYTJCJXCKCWDKKZXSGK");
                sbInputCode.Append("DZXWWYYJQYYTCYTDLLXWKCZKKLCCLZCQQDZLQLCSFQCHQ");
                sbInputCode.Append("HSFSMQZZLNBJJZBSJHTSZDYSJQJPDLZCDCWJKJZZLPYCG");
                sbInputCode.Append("MZWDJJBSJQZSYZYHHXJPBJYDSSXDZNCGLQMBTSFSBPDZD");
                sbInputCode.Append("LZNFGFJGFSMPXJQLMBLGQCYYXBQKDJJQYRFKZTJDHCZKL");
                sbInputCode.Append("BSDZCFJTPLLJGXHYXZCSSZZXSTJYGKGCKGYOQXJPLZPBP");
                sbInputCode.Append("GTGYJZGHZQZZLBJLSQFZGKQQJZGYCZBZQTLDXRJXBSXXP");
                sbInputCode.Append("ZXHYZYCLWDXJJHXMFDZPFZHQHQMQGKSLYHTYCGFRZGNQX");
                sbInputCode.Append("CLPDLBZCSCZQLLJBLHBZCYPZZPPDYMZZSGYHCKCPZJGSL");
                sbInputCode.Append("JLNSCDSLDLXBMSTLDDFJMKDJDHZLZXLSZQPQPGJLLYBDS");
                sbInputCode.Append("ZGQLBZLSLKYYHZTTNTJYQTZZPSZQZTLLJTYYLLQLLQYZQ");
                sbInputCode.Append("LBDZLSLYYZYMDFSZSNHLXZNCZQZPBWSKRFBSYZMTHBLGJ");
                sbInputCode.Append("PMCZZLSTLXSHTCSYZLZBLFEQHLXFLCJLYLJQCBZLZJHHS");
                sbInputCode.Append("STBRMHXZHJZCLXFNBGXGTQJCZTMSFZKJMSSNXLJKBHSJX");
                sbInputCode.Append("NTNLZDNTLMSJXGZJYJCZXYJYJWRWWQNZTNFJSZPZSHZJF");
                sbInputCode.Append("YRDJSFSZJZBJFZQZZHZLXFYSBZQLZSGYFTZDCSZXZJBQM");
                sbInputCode.Append("SZKJRHYJZCKMJKHCHGTXKXQGLXPXFXTRTYLXJXHDTSJXH");
                sbInputCode.Append("JZJXZWZLCQSBTXWXGXTXXHXFTSDKFJHZYJFJXRZSDLLLT");
                sbInputCode.Append("QSQQZQWZXSYQTWGWBZCGZLLYZBCLMQQTZHZXZXLJFRMYZ");
                sbInputCode.Append("FLXYSQXXJKXRMQDZDMMYYBSQBHGZMWFWXGMXLZPYYTGZY");
                sbInputCode.Append("CCDXYZXYWGSYJYZNBHPZJSQSYXSXRTFYZGRHZTXSZZTHC");
                sbInputCode.Append("BFCLSYXZLZQMZLMPLMXZJXSFLBYZMYQHXJSXRXSQZZZSS");
                sbInputCode.Append("LYFRCZJRCRXHHZXQYDYHXSJJHZCXZBTYNSYSXJBQLPXZQ");
                sbInputCode.Append("PYMLXZKYXLXCJLCYSXXZZLXDLLLJJYHZXGYJWKJRWYHCP");
                sbInputCode.Append("SGNRZLFZWFZZNSXGXFLZSXZZZBFCSYJDBRJKRDHHGXJLJ");
                sbInputCode.Append("JTGXJXXSTJTJXLYXQFCSGSWMSBCTLQZZWLZZKXJMLTMJY");
                sbInputCode.Append("HSDDBXGZHDLBMYJFRZFSGCLYJBPMLYSMSXLSZJQQHJZFX");
                sbInputCode.Append("GFQFQBPXZGYYQXGZTCQWYLTLGWSGWHRLFSFGZJMGMGBGT");
                sbInputCode.Append("JFSYZZGZYZAFLSSPMLPFLCWBJZCLJJMZLPJJLYMQDMYYY");
                sbInputCode.Append("FBGYGYZMLYZDXQYXRQQQHSYYYQXYLJTYXFSFSLLGNQCYH");
                sbInputCode.Append("YCWFHCCCFXPYLYPLLZYXXXXXKQHHXSHJZCFZSCZJXCPZW");
                sbInputCode.Append("HHHHHAPYLQALPQAFYHXDYLUKMZQGGGDDESRNNZLTZGCHY");
                sbInputCode.Append("PPYSQJJHCLLJTOLNJPZLJLHYMHEYDYDSQYCDDHGZUNDZC");
                sbInputCode.Append("LZYZLLZNTNYZGSLHSLPJJBDGWXPCDUTJCKLKCLWKLLCAS");
                sbInputCode.Append("STKZZDNQNTTLYYZSSYSSZZRYLJQKCQDHHCRXRZYDGRGCW");
                sbInputCode.Append("CGZQFFFPPJFZYNAKRGYWYQPQXXFKJTSZZXSWZDDFBBXTB");
                sbInputCode.Append("GTZKZNPZZPZXZPJSZBMQHKCYXYLDKLJNYPKYGHGDZJXXE");
                sbInputCode.Append("AHPNZKZTZCMXCXMMJXNKSZQNMNLWBWWXJKYHCPSTMCSQT");
                sbInputCode.Append("ZJYXTPCTPDTNNPGLLLZSJLSPBLPLQHDTNJNLYYRSZFFJF");
                sbInputCode.Append("QWDPHZDWMRZCCLODAXNSSNYZRESTYJWJYJDBCFXNMWTTB");
                sbInputCode.Append("YLWSTSZGYBLJPXGLBOCLHPCBJLTMXZLJYLZXCLTPNCLCK");
                sbInputCode.Append("XTPZJSWCYXSFYSZDKNTLBYJCYJLLSTGQCBXRYZXBXKLYL");
                sbInputCode.Append("HZLQZLNZCXWJZLJZJNCJHXMNZZGJZZXTZJXYCYYCXXJYY");
                sbInputCode.Append("XJJXSSSJSTSSTTPPGQTCSXWZDCSYFPTFBFHFBBLZJCLZZ");
                sbInputCode.Append("DBXGCXLQPXKFZFLSYLTUWBMQJHSZBMDDBCYSCCLDXYCDD");
                sbInputCode.Append("QLYJJWMQLLCSGLJJSYFPYYCCYLTJANTJJPWYCMMGQYYSX");
                sbInputCode.Append("DXQMZHSZXPFTWWZQSWQRFKJLZJQQYFBRXJHHFWJJZYQAZ");
                sbInputCode.Append("MYFRHCYYBYQWLPEXCCZSTYRLTTDMQLYKMBBGMYYJPRKZN");
                sbInputCode.Append("PBSXYXBHYZDJDNGHPMFSGMWFZMFQMMBCMZZCJJLCNUXYQ");
                sbInputCode.Append("LMLRYGQZCYXZLWJGCJCGGMCJNFYZZJHYCPRRCMTZQZXHF");
                sbInputCode.Append("QGTJXCCJEAQCRJYHPLQLSZDJRBCQHQDYRHYLYXJSYMHZY");
                sbInputCode.Append("DWLDFRYHBPYDTSSCNWBXGLPZMLZZTQSSCPJMXXYCSJYTY");
                sbInputCode.Append("CGHYCJWYRXXLFEMWJNMKLLSWTXHYYYNCMMCWJDQDJZGLL");
                sbInputCode.Append("JWJRKHPZGGFLCCSCZMCBLTBHBQJXQDSPDJZZGKGLFQYWB");
                sbInputCode.Append("ZYZJLTSTDHQHCTCBCHFLQMPWDSHYYTQWCNZZJTLBYMBPD");
                sbInputCode.Append("YYYXSQKXWYYFLXXNCWCXYPMAELYKKJMZZZBRXYYQJFLJP");
                sbInputCode.Append("FHHHYTZZXSGQQMHSPGDZQWBWPJHZJDYSCQWZKTXXSQLZY");
                sbInputCode.Append("YMYSDZGRXCKKUJLWPYSYSCSYZLRMLQSYLJXBCXTLWDQZP");
                sbInputCode.Append("CYCYKPPPNSXFYZJJRCEMHSZMSXLXGLRWGCSTLRSXBZGBZ");
                sbInputCode.Append("GZTCPLUJLSLYLYMTXMTZPALZXPXJTJWTCYYZLBLXBZLQM");
                sbInputCode.Append("YLXPGHDSLSSDMXMBDZZSXWHAMLCZCPJMCNHJYSNSYGCHS");
                sbInputCode.Append("KQMZZQDLLKABLWJXSFMOCDXJRRLYQZKJMYBYQLYHETFJZ");
                sbInputCode.Append("FRFKSRYXFJTWDSXXSYSQJYSLYXWJHSNLXYYXHBHAWHHJZ");
                sbInputCode.Append("XWMYLJCSSLKYDZTXBZSYFDXGXZJKHSXXYBSSXDPYNZWRP");
                sbInputCode.Append("TQZCZENYGCXQFJYKJBZMLJCMQQXUOXSLYXXLYLLJDZBTY");
                sbInputCode.Append("MHPFSTTQQWLHOKYBLZZALZXQLHZWRRQHLSTMYPYXJJXMQ");
                sbInputCode.Append("SJFNBXYXYJXXYQYLTHYLQYFMLKLJTMLLHSZWKZHLJMLHL");
                sbInputCode.Append("JKLJSTLQXYLMBHHLNLZXQJHXCFXXLHYHJJGBYZZKBXSCQ");
                sbInputCode.Append("DJQDSUJZYYHZHHMGSXCSYMXFEBCQWWRBPYYJQTYZCYQYQ");
                sbInputCode.Append("QZYHMWFFHGZFRJFCDPXNTQYZPDYKHJLFRZXPPXZDBBGZQ");
                sbInputCode.Append("STLGDGYLCQMLCHHMFYWLZYXKJLYPQHSYWMQQGQZMLZJNS");
                sbInputCode.Append("QXJQSYJYCBEHSXFSZPXZWFLLBCYYJDYTDTHWZSFJMQQYJ");
                sbInputCode.Append("LMQXXLLDTTKHHYBFPWTYYSQQWNQWLGWDEBZWCMYGCULKJ");
                sbInputCode.Append("XTMXMYJSXHYBRWFYMWFRXYQMXYSZTZZTFYKMLDHQDXWYY");
                sbInputCode.Append("NLCRYJBLPSXCXYWLSPRRJWXHQYPHTYDNXHHMMYWYTZCSQ");
                sbInputCode.Append("MTSSCCDALWZTCPQPYJLLQZYJSWXMZZMMYLMXCLMXCZMXM");
                sbInputCode.Append("ZSQTZPPQQBLPGXQZHFLJJHYTJSRXWZXSCCDLXTYJDCQJX");
                sbInputCode.Append("SLQYCLZXLZZXMXQRJMHRHZJBHMFLJLMLCLQNLDXZLLLPY");
                sbInputCode.Append("PSYJYSXCQQDCMQJZZXHNPNXZMEKMXHYKYQLXSXTXJYYHW");
                sbInputCode.Append("DCWDZHQYYBGYBCYSCFGPSJNZDYZZJZXRZRQJJYMCANYRJ");
                sbInputCode.Append("TLDPPYZBSTJKXXZYPFDWFGZZRPYMTNGXZQBYXNBUFNQKR");
                sbInputCode.Append("JQZMJEGRZGYCLKXZDSKKNSXKCLJSPJYYZLQQJYBZSSQLL");
                sbInputCode.Append("LKJXTBKTYLCCDDBLSPPFYLGYDTZJYQGGKQTTFZXBDKTYY");
                sbInputCode.Append("HYBBFYTYYBCLPDYTGDHRYRNJSPTCSNYJQHKLLLZSLYDXX");
                sbInputCode.Append("WBCJQSPXBPJZJCJDZFFXXBRMLAZHCSNDLBJDSZBLPRZTS");
                sbInputCode.Append("WSBXBCLLXXLZDJZSJPYLYXXYFTFFFBHJJXGBYXJPMMMPS");
                sbInputCode.Append("SJZJMTLYZJXSWXTYLEDQPJMYGQZJGDJLQJWJQLLSJGJGY");
                sbInputCode.Append("GMSCLJJXDTYGJQJQJCJZCJGDZZSXQGSJGGCXHQXSNQLZZ");
                sbInputCode.Append("BXHSGZXCXYLJXYXYYDFQQJHJFXDHCTXJYRXYSQTJXYEFY");
                sbInputCode.Append("YSSYYJXNCYZXFXMSYSZXYYSCHSHXZZZGZZZGFJDLTYLNP");
                sbInputCode.Append("ZGYJYZYYQZPBXQBDZTZCZYXXYHHSQXSHDHGQHJHGYWSZT");
                sbInputCode.Append("MZMLHYXGEBTYLZKQWYTJZRCLEKYSTDBCYKQQSAYXCJXWW");
                sbInputCode.Append("GSBHJYZYDHCSJKQCXSWXFLTYNYZPZCCZJQTZWJQDZZZQZ");
                sbInputCode.Append("LJJXLSBHPYXXPSXSHHEZTXFPTLQYZZXHYTXNCFZYYHXGN");
                sbInputCode.Append("XMYWXTZSJPTHHGYMXMXQZXTSBCZYJYXXTYYZYPCQLMMSZ");
                sbInputCode.Append("MJZZLLZXGXZAAJZYXJMZXWDXZSXZDZXLEYJJZQBHZWZZZ");
                sbInputCode.Append("QTZPSXZTDSXJJJZNYAZPHXYYSRNQDTHZHYYKYJHDZXZLS");
                sbInputCode.Append("WCLYBZYECWCYCRYLCXNHZYDZYDYJDFRJJHTRSQTXYXJRJ");
                sbInputCode.Append("HOJYNXELXSFSFJZGHPZSXZSZDZCQZBYYKLSGSJHCZSHDG");
                sbInputCode.Append("QGXYZGXCHXZJWYQWGYHKSSEQZZNDZFKWYSSTCLZSTSYMC");
                sbInputCode.Append("DHJXXYWEYXCZAYDMPXMDSXYBSQMJMZJMTZQLPJYQZCGQH");
                sbInputCode.Append("XJHHLXXHLHDLDJQCLDWBSXFZZYYSCHTYTYYBHECXHYKGJ");
                sbInputCode.Append("PXHHYZJFXHWHBDZFYZBCAPNPGNYDMSXHMMMMAMYNBYJTM");
                sbInputCode.Append("PXYYMCTHJBZYFCGTYHWPHFTWZZEZSBZEGPFMTSKFTYCMH");
                sbInputCode.Append("FLLHGPZJXZJGZJYXZSBBQSCZZLZCCSTPGXMJSFTCCZJZD");
                sbInputCode.Append("JXCYBZLFCJSYZFGSZLYBCWZZBYZDZYPSWYJZXZBDSYUXL");
                sbInputCode.Append("ZZBZFYGCZXBZHZFTPBGZGEJBSTGKDMFHYZZJHZLLZZGJQ");
                sbInputCode.Append("ZLSFDJSSCBZGPDLFZFZSZYZYZSYGCXSNXXCHCZXTZZLJF");
                sbInputCode.Append("ZGQSQYXZJQDCCZTQCDXZJYQJQCHXZTDLGSCXZSYQJQTZW");
                sbInputCode.Append("LQDQZTQCHQQJZYEZZZPBWKDJFCJPZTYPQYQTTYNLMBDKT");
                sbInputCode.Append("JZPQZQZZFPZSBNJLGYJDXJDZZKZGQKXDLPZJTCJDQBXDJ");
                sbInputCode.Append("QJSTCKNXBXZMSLYJCQMTJQWWCJQNJNLLLHJCWQTBZQYDZ");
                sbInputCode.Append("CZPZZDZYDDCYZZZCCJTTJFZDPRRTZTJDCQTQZDTJNPLZB");
                sbInputCode.Append("CLLCTZSXKJZQZPZLBZRBTJDCXFCZDBCCJJLTQQPLDCGZD");
                sbInputCode.Append("BBZJCQDCJWYNLLZYZCCDWLLXWZLXRXNTQQCZXKQLSGDFQ");
                sbInputCode.Append("TDDGLRLAJJTKUYMKQLLTZYTDYYCZGJWYXDXFRSKSTQTEN");
                sbInputCode.Append("QMRKQZHHQKDLDAZFKYPBGGPZREBZZYKZZSPEGJXGYKQZZ");
                sbInputCode.Append("ZSLYSYYYZWFQZYLZZLZHWCHKYPQGNPGBLPLRRJYXCCSYY");
                sbInputCode.Append("HSFZFYBZYYTGZXYLXCZWXXZJZBLFFLGSKHYJZEYJHLPLL");
                sbInputCode.Append("LLCZGXDRZELRHGKLZZYHZLYQSZZJZQLJZFLNBHGWLCZCF");
                sbInputCode.Append("JYSPYXZLZLXGCCPZBLLCYBBBBUBBCBPCRNNZCZYRBFSRL");
                sbInputCode.Append("DCGQYYQXYGMQZWTZYTYJXYFWTEHZZJYWLCCNTZYJJZDED");
                sbInputCode.Append("PZDZTSYQJHDYMBJNYJZLXTSSTPHNDJXXBYXQTZQDDTJTD");
                sbInputCode.Append("YYTGWSCSZQFLSHLGLBCZPHDLYZJYCKWTYTYLBNYTSDSYC");
                sbInputCode.Append("CTYSZYYEBHEXHQDTWNYGYCLXTSZYSTQMYGZAZCCSZZDSL");
                sbInputCode.Append("ZCLZRQXYYELJSBYMXSXZTEMBBLLYYLLYTDQYSHYMRQWKF");
                sbInputCode.Append("KBFXNXSBYCHXBWJYHTQBPBSBWDZYLKGZSKYHXQZJXHXJX");
                sbInputCode.Append("GNLJKZLYYCDXLFYFGHLJGJYBXQLYBXQPQGZTZPLNCYPXD");
                sbInputCode.Append("JYQYDYMRBESJYYHKXXSTMXRCZZYWXYQYBMCLLYZHQYZWQ");
                sbInputCode.Append("XDBXBZWZMSLPDMYSKFMZKLZCYQYCZLQXFZZYDQZPZYGYJ");
                sbInputCode.Append("YZMZXDZFYFYTTQTZHGSPCZMLCCYTZXJCYTJMKSLPZHYSN");
                sbInputCode.Append("ZLLYTPZCTZZCKTXDHXXTQCYFKSMQCCYYAZHTJPCYLZLYJ");
                sbInputCode.Append("BJXTPNYLJYYNRXSYLMMNXJSMYBCSYSYLZYLXJJQYLDZLP");
                sbInputCode.Append("QBFZZBLFNDXQKCZFYWHGQMRDSXYCYTXNQQJZYYPFZXDYZ");
                sbInputCode.Append("FPRXEJDGYQBXRCNFYYQPGHYJDYZXGRHTKYLNWDZNTSMPK");
                sbInputCode.Append("LBTHBPYSZBZTJZSZZJTYYXZPHSSZZBZCZPTQFZMYFLYPY");
                sbInputCode.Append("BBJQXZMXXDJMTSYSKKBJZXHJCKLPSMKYJZCXTMLJYXRZZ");
                sbInputCode.Append("QSLXXQPYZXMKYXXXJCLJPRMYYGADYSKQLSNDHYZKQXZYZ");
                sbInputCode.Append("TCGHZTLMLWZYBWSYCTBHJHJFCWZTXWYTKZLXQSHLYJZJX");
                sbInputCode.Append("TMPLPYCGLTBZZTLZJCYJGDTCLKLPLLQPJMZPAPXYZLKKT");
                sbInputCode.Append("KDZCZZBNZDYDYQZJYJGMCTXLTGXSZLMLHBGLKFWNWZHDX");
                sbInputCode.Append("UHLFMKYSLGXDTWWFRJEJZTZHYDXYKSHWFZCQSHKTMQQHT");
                sbInputCode.Append("ZHYMJDJSKHXZJZBZZXYMPAGQMSTPXLSKLZYNWRTSQLSZB");
                sbInputCode.Append("PSPSGZWYHTLKSSSWHZZLYYTNXJGMJSZSUFWNLSOZTXGXL");
                sbInputCode.Append("SAMMLBWLDSZYLAKQCQCTMYCFJBSLXCLZZCLXXKSBZQCLH");
                sbInputCode.Append("JPSQPLSXXCKSLNHPSFQQYTXYJZLQLDXZQJZDYYDJNZPTU");
                sbInputCode.Append("ZDSKJFSLJHYLZSQZLBTXYDGTQFDBYAZXDZHZJNHHQBYKN");
                sbInputCode.Append("XJJQCZMLLJZKSPLDYCLBBLXKLELXJLBQYCXJXGCNLCQPL");
                sbInputCode.Append("ZLZYJTZLJGYZDZPLTQCSXFDMNYCXGBTJDCZNBGBQYQJWG");
                sbInputCode.Append("KFHTNPYQZQGBKPBBYZMTJDYTBLSQMPSXTBNPDXKLEMYYC");
                sbInputCode.Append("JYNZCTLDYKZZXDDXHQSHDGMZSJYCCTAYRZLPYLTLKXSLZ");
                sbInputCode.Append("CGGEXCLFXLKJRTLQJAQZNCMBYDKKCXGLCZJZXJHPTDJJM");
                sbInputCode.Append("ZQYKQSECQZDSHHADMLZFMMZBGNTJNNLGBYJBRBTMLBYJD");
                sbInputCode.Append("ZXLCJLPLDLPCQDHLXZLYCBLCXZZJADJLNZMMSSSMYBHBS");
                sbInputCode.Append("QKBHRSXXJMXSDZNZPXLGBRHWGGFCXGMSKLLTSJYYCQLTS");
                sbInputCode.Append("KYWYYHYWXBXQYWPYWYKQLSQPTNTKHQCWDQKTWPXXHCPTH");
                sbInputCode.Append("TWUMSSYHBWCRWXHJMKMZNGWTMLKFGHKJYLSYYCXWHYECL");
                sbInputCode.Append("QHKQHTTQKHFZLDXQWYZYYDESBPKYRZPJFYYZJCEQDZZDL");
                sbInputCode.Append("ATZBBFJLLCXDLMJSSXEGYGSJQXCWBXSSZPDYZCXDNYXPP");
                sbInputCode.Append("ZYDLYJCZPLTXLSXYZYRXCYYYDYLWWNZSAHJSYQYHGYWWA");
                sbInputCode.Append("XTJZDAXYSRLTDPSSYYFNEJDXYZHLXLLLZQZSJNYQYQQXY");
                sbInputCode.Append("JGHZGZCYJCHZLYCDSHWSHJZYJXCLLNXZJJYYXNFXMWFPY");
                sbInputCode.Append("LCYLLABWDDHWDXJMCXZTZPMLQZHSFHZYNZTLLDYWLSLXH");
                sbInputCode.Append("YMMYLMBWWKYXYADTXYLLDJPYBPWUXJMWMLLSAFDLLYFLB");
                sbInputCode.Append("HHHBQQLTZJCQJLDJTFFKMMMBYTHYGDCQRDDWRQJXNBYSN");
                sbInputCode.Append("WZDBYYTBJHPYBYTTJXAAHGQDQTMYSTQXKBTZPKJLZRBEQ");
                sbInputCode.Append("QSSMJJBDJOTGTBXPGBKTLHQXJJJCTHXQDWJLWRFWQGWSH");
                sbInputCode.Append("CKRYSWGFTGYGBXSDWDWRFHWYTJJXXXJYZYSLPYYYPAYXH");
                sbInputCode.Append("YDQKXSHXYXGSKQHYWFDDDPPLCJLQQEEWXKSYYKDYPLTJT");
                sbInputCode.Append("HKJLTCYYHHJTTPLTZZCDLTHQKZXQYSTEEYWYYZYXXYYST");
                sbInputCode.Append("TJKLLPZMCYHQGXYHSRMBXPLLNQYDQHXSXXWGDQBSHYLLP");
                sbInputCode.Append("JJJTHYJKYPPTHYYKTYEZYENMDSHLCRPQFDGFXZPSFTLJX");
                sbInputCode.Append("XJBSWYYSKSFLXLPPLBBBLBSFXFYZBSJSSYLPBBFFFFSSC");
                sbInputCode.Append("JDSTZSXZRYYSYFFSYZYZBJTBCTSBSDHRTJJBYTCXYJEYL");
                sbInputCode.Append("XCBNEBJDSYXYKGSJZBXBYTFZWGENYHHTHZHHXFWGCSTBG");
                sbInputCode.Append("XKLSXYWMTMBYXJSTZSCDYQRCYTWXZFHMYMCXLZNSDJTTT");
                sbInputCode.Append("XRYCFYJSBSDYERXJLJXBBDEYNJGHXGCKGSCYMBLXJMSZN");
                sbInputCode.Append("SKGXFBNBPTHFJAAFXYXFPXMYPQDTZCXZZPXRSYWZDLYBB");
                sbInputCode.Append("KTYQPQJPZYPZJZNJPZJLZZFYSBTTSLMPTZRTDXQSJEHBZ");
                sbInputCode.Append("YLZDHLJSQMLHTXTJECXSLZZSPKTLZKQQYFSYGYWPCPQFH");
                sbInputCode.Append("QHYTQXZKRSGTTSQCZLPTXCDYYZXSQZSLXLZMYCPCQBZYX");
                sbInputCode.Append("HBSXLZDLTCDXTYLZJYYZPZYZLTXJSJXHLPMYTXCQRBLZS");
                sbInputCode.Append("SFJZZTNJYTXMYJHLHPPLCYXQJQQKZZSCPZKSWALQSBLCC");
                sbInputCode.Append("ZJSXGWWWYGYKTJBBZTDKHXHKGTGPBKQYSLPXPJCKBMLLX");
                sbInputCode.Append("DZSTBKLGGQKQLSBKKTFXRMDKBFTPZFRTBBRFERQGXYJPZ");
                sbInputCode.Append("SSTLBZTPSZQZSJDHLJQLZBPMSMMSXLQQNHKNBLRDDNXXD");
                sbInputCode.Append("HDDJCYYGYLXGZLXSYGMQQGKHBPMXYXLYTQWLWGCPBMQXC");
                sbInputCode.Append("YZYDRJBHTDJYHQSHTMJSBYPLWHLZFFNYPMHXXHPLTBQPF");
                sbInputCode.Append("BJWQDBYGPNZTPFZJGSDDTQSHZEAWZZYLLTYYBWJKXXGHL");
                sbInputCode.Append("FKXDJTMSZSQYNZGGSWQSPHTLSSKMCLZXYSZQZXNCJDQGZ");
                sbInputCode.Append("DLFNYKLJCJLLZLMZZNHYDSSHTHZZLZZBBHQZWWYCRZHLY");
                sbInputCode.Append("QQJBEYFXXXWHSRXWQHWPSLMSSKZTTYGYQQWRSLALHMJTQ");
                sbInputCode.Append("JSMXQBJJZJXZYZKXBYQXBJXSHZTSFJLXMXZXFGHKZSZGG");
                sbInputCode.Append("YLCLSARJYHSLLLMZXELGLXYDJYTLFBHBPNLYZFBBHPTGJ");
                sbInputCode.Append("KWETZHKJJXZXXGLLJLSTGSHJJYQLQZFKCGNNDJSSZFDBC");
                sbInputCode.Append("TWWSEQFHQJBSAQTGYPQLBXBMMYWXGSLZHGLZGQYFLZBYF");
                sbInputCode.Append("ZJFRYSFMBYZHQGFWZSYFYJJPHZBYYZFFWODGRLMFTWLBZ");
                sbInputCode.Append("GYCQXCDJYGZYYYYTYTYDWEGAZYHXJLZYYHLRMGRXXZCLH");
                sbInputCode.Append("NELJJTJTPWJYBJJBXJJTJTEEKHWSLJPLPSFYZPQQBDLQJ");
                sbInputCode.Append("JTYYQLYZKDKSQJYYQZLDQTGJQYZJSUCMRYQTHTEJMFCTY");
                sbInputCode.Append("HYPKMHYZWJDQFHYYXWSHCTXRLJHQXHCCYYYJLTKTTYTMX");
                sbInputCode.Append("GTCJTZAYYOCZLYLBSZYWJYTSJYHBYSHFJLYGJXXTMZYYL");
                sbInputCode.Append("TXXYPZLXYJZYZYYPNHMYMDYYLBLHLSYYQQLLNJJYMSOYQ");
                sbInputCode.Append("BZGDLYXYLCQYXTSZEGXHZGLHWBLJHEYXTWQMAKBPQCGYS");
                sbInputCode.Append("HHEGQCMWYYWLJYJHYYZLLJJYLHZYHMGSLJLJXCJJYCLYC");
                sbInputCode.Append("JPCPZJZJMMYLCQLNQLJQJSXYJMLSZLJQLYCMMHCFMMFPQ");
                sbInputCode.Append("QMFYLQMCFFQMMMMHMZNFHHJGTTHHKHSLNCHHYQDXTMMQD");
                sbInputCode.Append("CYZYXYQMYQYLTDCYYYZAZZCYMZYDLZFFFMMYCQZWZZMAB");
                sbInputCode.Append("TBYZTDMNZZGGDFTYPCGQYTTSSFFWFDTZQSSYSTWXJHXYT");
                sbInputCode.Append("SXXYLBYQHWWKXHZXWZNNZZJZJJQJCCCHYYXBZXZCYZTLL");
                sbInputCode.Append("CQXYNJYCYYCYNZZQYYYEWYCZDCJYCCHYJLBTZYYCQWMPW");
                sbInputCode.Append("PYMLGKDLDLGKQQBGYCHJXY");
                m_szInputCodeText = sbInputCode.ToString();
            }
            #endregion

            /// <summary>
            /// ���ı�ת����ƴ������ĸ��ʾ��������
            /// </summary>
            /// <param name="szText">ָ�����ı�</param>
            /// <param name="bUpper">�Ƿ�ת��Ϊ��д</param>
            /// <param name="nCount">ָ��ת��������</param>
            /// <returns>�������ַ���</returns>
            public static string GetInputCode(string szText, bool bUpper, int nCount)
            {
                if (GlobalMethods.Misc.IsEmptyString(szText))
                    return string.Empty;
                szText = szText.Trim();
                if (szText.Length > nCount)
                    szText = szText.Substring(0, nCount).Trim();

                InitInputCodeTable();
                try
                {
                    StringBuilder sbInputCode = new StringBuilder();
                    for (int index = 0; index < szText.Length; index++)
                    {
                        char ch = szText[index];
                        if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')
                            || (ch >= '0' && ch <= '9'))
                        {
                            //������ĸ��������ֱ�����
                            sbInputCode.Append(char.ToLower(ch));
                            continue;
                        }
                        int codeIndex = (int)ch - 19968;
                        if (codeIndex < 0 || codeIndex >= m_szInputCodeText.Length)
                        {
                            //������ĸҲ���Ǻ���,���ݲ����
                            //sbInputCode.Append(ch);
                        }
                        else
                        {
                            //���Ǻ����򷵻���д��ĸ
                            sbInputCode.Append(m_szInputCodeText[codeIndex]);
                        }
                    }
                    return bUpper ? sbInputCode.ToString() : sbInputCode.ToString().ToLower();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.ChsToInputCode"
                        , new string[] { "szText", "bUpper", "nCount" }, new object[] { szText, bUpper, nCount }, ex);
                    return string.Empty;
                }
            }
        }
    }
}
