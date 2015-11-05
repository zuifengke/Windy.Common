// ***********************************************************
// ��װWindows���а���ز�����������
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
        /// Windows���а���ز���
        /// </summary>
        public struct Clipbrd
        {
            /// <summary>
            /// ��ȡ���а�����,Ĭ�ϸ�ʽΪ���ı�
            /// </summary>
            /// <returns>���а�����</returns>
            public static string GetData()
            {
                try
                {
                    return Clipboard.GetText(TextDataFormat.Text);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Clipbrd.GetData"
                        , null, null, "Clipboard.GetTextִ��ʧ��!", ex);
                    return string.Empty;
                }
            }

            /// <summary>
            /// ��ȡ���а����ݣ��Զ����ʽ
            /// </summary>
            /// <param name="format">���ݸ�ʽ</param>
            /// <returns>���а�����</returns>
            public static object GetData(string format)
            {
                try
                {
                    IDataObject data = Clipboard.GetDataObject();
                    if (data != null && data.GetDataPresent(format))
                        return data.GetData(format);
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Clipbrd.GetData"
                        , null, null, "Clipboard.GetDataִ��ʧ��!", ex);
                    return string.Empty;
                }
            }

            /// <summary>
            /// ���ü��а����ݣ���Ӵ��ı���
            /// </summary>
            /// <param name="text">���а�����</param>
            public static void SetData(string text)
            {
                try
                {
                    Clipboard.Clear();
                    Clipboard.SetText(text);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Clipbrd.SetData"
                        , null, null, "Clipboard.SetTextִ��ʧ��!", ex);
                }
            }

            /// <summary>
            /// ���ü��а����ݣ��Զ����ʽ��
            /// </summary>
            /// <param name="data">���а�����</param>
            /// <param name="format">���ݸ�ʽ</param>
            public static void SetData(object data, string format)
            {
                try
                {
                    Clipboard.Clear();
                    Clipboard.SetData(format, data);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Clipbrd.SetData"
                        , null, null, "Clipboard.SetDataִ��ʧ��!", ex);
                }
            }

            /// <summary>
            /// ���а������ı�����
            /// </summary>
            /// <returns>bool</returns>
            public static bool IsText()
            {
                try
                {
                    return Clipboard.ContainsText();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Clipbrd.IsText"
                        , null, null, "Clipboard.ContainsDataִ��ʧ��!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ���а�����RTF����ʽ����
            /// </summary>
            /// <returns>bool</returns>
            public static bool IsRtf()
            {
                try
                {
                    return Clipboard.ContainsData(DataFormats.Rtf);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Clipbrd.IsRtf"
                        , null, null, "Clipboard.ContainsDataִ��ʧ��!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ���а�����ͼ������
            /// </summary>
            /// <returns>bool</returns>
            public static bool IsBitmap()
            {
                try
                {
                    return Clipboard.ContainsData(DataFormats.Bitmap);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Clipbrd.IsBitmap"
                        , null, null, "Clipboard.ContainsDataִ��ʧ��!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ���Windwos���а�����
            /// </summary>
            public static void Clear()
            {
                try
                {
                    Clipboard.Clear();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Clipbrd.Clear"
                        , null, null, "Clipboard.ClearClipboardִ��ʧ��!", ex);
                }
            }
        }
    }
}
