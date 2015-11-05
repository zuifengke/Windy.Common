// ***********************************************************
// 封装Windows剪切板相关操作方法集合
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
        /// Windows剪切板相关操作
        /// </summary>
        public struct Clipbrd
        {
            /// <summary>
            /// 获取剪切板数据,默认格式为纯文本
            /// </summary>
            /// <returns>剪切板数据</returns>
            public static string GetData()
            {
                try
                {
                    return Clipboard.GetText(TextDataFormat.Text);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Clipbrd.GetData"
                        , null, null, "Clipboard.GetText执行失败!", ex);
                    return string.Empty;
                }
            }

            /// <summary>
            /// 获取剪切板数据，自定义格式
            /// </summary>
            /// <param name="format">数据格式</param>
            /// <returns>剪切板数据</returns>
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
                        , null, null, "Clipboard.GetData执行失败!", ex);
                    return string.Empty;
                }
            }

            /// <summary>
            /// 设置剪切板数据（添加纯文本）
            /// </summary>
            /// <param name="text">剪切板数据</param>
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
                        , null, null, "Clipboard.SetText执行失败!", ex);
                }
            }

            /// <summary>
            /// 设置剪切板数据（自定义格式）
            /// </summary>
            /// <param name="data">剪切板数据</param>
            /// <param name="format">数据格式</param>
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
                        , null, null, "Clipboard.SetData执行失败!", ex);
                }
            }

            /// <summary>
            /// 剪切板内是文本数据
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
                        , null, null, "Clipboard.ContainsData执行失败!", ex);
                    return false;
                }
            }

            /// <summary>
            /// 剪切板内是RTF富格式数据
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
                        , null, null, "Clipboard.ContainsData执行失败!", ex);
                    return false;
                }
            }

            /// <summary>
            /// 剪切板内是图像数据
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
                        , null, null, "Clipboard.ContainsData执行失败!", ex);
                    return false;
                }
            }

            /// <summary>
            /// 清空Windwos剪切板数据
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
                        , null, null, "Clipboard.ClearClipboard执行失败!", ex);
                }
            }
        }
    }
}
