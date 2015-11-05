// ***********************************************************
// 封装一些基本的操作系统相关操作方法集合
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;

namespace Windy.Common.Libraries
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// 封装操作系统相关操作
        /// </summary>
        public struct Win32
        {
            /// <summary>
            /// 获取系统中已经安装的打印机列表
            /// </summary>
            /// <returns>打印机列表</returns>
            public static string[] GetPrinterList()
            {
                try
                {
                    System.Drawing.Printing.PrinterSettings.StringCollection lstPrinter = null;
                    lstPrinter = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
                    if (lstPrinter.Count <= 0)
                        return null;
                    string[] arrPrinterName = new string[lstPrinter.Count];
                    lstPrinter.CopyTo(arrPrinterName, 0);
                    return arrPrinterName;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetPrinterList", null, null, ex);
                    return null;
                }
            }

            /// <summary>
            /// 设置指定打印机为默认打印机
            /// </summary>
            /// <param name="szPrinterName">打印机名称</param>
            public static bool SetSysDefaultPrinter(string szPrinterName)
            {
                if (GlobalMethods.Misc.IsEmptyString(szPrinterName))
                    return false;
                try
                {
                    return NativeMethods.WinSpool.SetDefaultPrinter(szPrinterName);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.SetSysDefaultPrinter"
                        , new string[] { "szPrinterName" }, new object[] { szPrinterName }, ex);
                    return false;
                }
            }

            /// <summary>
            /// 获取操作系统当前默认打印机名称
            /// </summary>
            /// <return>默认打印机名称</return>
            public static string GetSysDefaultPrinter()
            {
                try
                {
                    return (new System.Drawing.Printing.PrinterSettings()).PrinterName;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetSysDefaultPrinter", null, null, ex);
                    return string.Empty;
                }
            }

            /// <summary>
            /// 获取系统中安装的打印机数目
            /// </summary>
            /// <returns>打印机数目</returns>
            public static int GetPrinterCount()
            {
                try
                {
                    return System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetPrinterCount", null, null, ex);
                    return 0;
                }
            }

            /// <summary>
            /// 获取指定打印机的相关信息
            /// </summary>
            /// <param name="szPrinterName">打印机名称</param>
            /// <param name="stPrinterInfo">打印机信息</param>
            /// <returns>是否执行成功</returns>
            public static bool GetPrinterInfo(string szPrinterName, ref NativeMethods.WinSpool.PRINTER_INFO stPrinterInfo)
            {
                IntPtr hPrinter = IntPtr.Zero;
                NativeMethods.WinSpool.StructPrinterDefaults defaults = new NativeMethods.WinSpool.StructPrinterDefaults();
                if (!NativeMethods.WinSpool.OpenPrinter(szPrinterName, out hPrinter, ref defaults))
                    return false;

                int cbNeeded = 0;
                NativeMethods.WinSpool.GetPrinter(hPrinter, 2, IntPtr.Zero, 0, out cbNeeded);
                if (cbNeeded <= 0)
                {
                    NativeMethods.WinSpool.ClosePrinter(hPrinter);
                    return false;
                }
                try
                {
                    IntPtr pAddr = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)cbNeeded);
                    if (NativeMethods.WinSpool.GetPrinter(hPrinter, 2, pAddr, cbNeeded, out cbNeeded))
                    {
                        stPrinterInfo = (NativeMethods.WinSpool.PRINTER_INFO)
                            System.Runtime.InteropServices.Marshal.PtrToStructure(pAddr, typeof(NativeMethods.WinSpool.PRINTER_INFO));
                    }
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(pAddr);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetPrinterInfo", new string[] { "szPrinterName" }
                        , new object[] { szPrinterName }, ex);
                }
                NativeMethods.WinSpool.ClosePrinter(hPrinter);
                return true;
            }

            /// <summary>
            /// 根据打印机的状态参数获取状态名称
            /// </summary>
            /// <param name="nStatus">打印机的状态</param>
            /// <returns>状态中文名称</returns>
            public static string GetPrinterStatus(int nStatus)
            {
                string strStatus = string.Empty;
                switch (nStatus)
                {
                    case -1:
                        strStatus = string.Empty;
                        break;
                    case 0:
                        strStatus = "准备就绪（Ready）";
                        break;
                    case 0x00000200:
                        strStatus = "忙（Busy）";
                        break;
                    case 0x00400000:
                        strStatus = "被打开（Printer Door Open）";
                        break;
                    case 0x00000002:
                        strStatus = "错误（Printer Error）";
                        break;
                    case 0x0008000:
                        strStatus = "初始化（Initializing）";
                        break;
                    case 0x00000100:
                        strStatus = "正在输入,输出（I/O Active）";
                        break;
                    case 0x00000020:
                        strStatus = "手工送纸（Manual Feed）";
                        break;
                    case 0x00040000:
                        strStatus = "无墨粉（No Toner）";
                        break;
                    case 0x00001000:
                        strStatus = "不可用（Not Available）";
                        break;
                    case 0x00000080:
                        strStatus = "脱机（Off Line）";
                        break;
                    case 0x00200000:
                        strStatus = "内存溢出（Out of Memory）";
                        break;
                    case 0x00000800:
                        strStatus = "输出口已满（Output Bin Full）";
                        break;
                    case 0x00080000:
                        strStatus = "当前页无法打印（Page Punt）";
                        break;
                    case 0x00000008:
                        strStatus = "塞纸（Paper Jam）";
                        break;
                    case 0x00000010:
                        strStatus = "打印纸用完（Paper Out）";
                        break;
                    case 0x00000040:
                        strStatus = "纸张问题（Page Problem）";
                        break;
                    case 0x00000001:
                        strStatus = "暂停（Paused）";
                        break;
                    case 0x00000004:
                        strStatus = "正在删除（Pending Deletion）";
                        break;
                    case 0x00000400:
                        strStatus = "正在打印（Printing）";
                        break;
                    case 0x00004000:
                        strStatus = "正在处理（Processing）";
                        break;
                    case 0x00020000:
                        strStatus = "墨粉不足（Toner Low）";
                        break;
                    case 0x00100000:
                        strStatus = "需要用户干预（User Intervention）";
                        break;
                    case 0x20000000:
                        strStatus = "等待（Waiting）";
                        break;
                    case 0x00010000:
                        strStatus = "热机中（Warming Up）";
                        break;
                    default:
                        strStatus = "未知状态（Unknown Status）";
                        break;
                }
                return strStatus;
            }

            /// <summary>
            /// 获取当前用户所用机器的名称
            /// </summary>
            /// <returns>机器名称</returns>
            public static string GetMachineName()
            {
                try
                {
                    return System.Environment.MachineName;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetMachineName", ex);
                    return string.Empty;
                }
            }

            /// <summary>
            /// 获取使用内存映像文件机制进行单实例运行的系统的句柄
            /// </summary>
            /// <param name="systemName">系统标识名</param>
            /// <returns>返回系统主窗口的句柄</returns>
            public static IntPtr GetSystemHandle(string systemName)
            {
                FileMapping fileMapping = null;
                try
                {
                    fileMapping = new FileMapping(systemName);
                }
                catch
                {
                    return IntPtr.Zero;
                }
                if (fileMapping.IsFirstInstance)
                {
                    fileMapping.Dispose(true);
                    return IntPtr.Zero;
                }
                IntPtr hMainFormHandle = fileMapping.ReadHandleValue(0);
                fileMapping.Dispose(false);
                return hMainFormHandle;
            }

            /// <summary>
            /// 安装或反安装指定的ActiveX控件
            /// </summary>
            /// <param name="szFileName">ActiveX控件全路径</param>
            /// <param name="bIsUninstall">执行反安装</param>
            /// <returns>true-执行成功;false-执行失败</returns>
            public static bool InstallActiveXControl(string szFileName, bool bIsUninstall)
            {
                if (GlobalMethods.Misc.IsEmptyString(szFileName) || !System.IO.File.Exists(szFileName))
                    return false;
                IntPtr hOcxLib = NativeMethods.Kernel32.LoadLibrary(szFileName);
                if (hOcxLib == IntPtr.Zero)
                {
                    NativeMethods.Kernel32.FreeLibrary(hOcxLib);
                    return false;
                }

                IntPtr hRegFunc = IntPtr.Zero;
                if (!bIsUninstall)
                    hRegFunc = NativeMethods.Kernel32.GetProcAddress(hOcxLib, "DllRegisterServer");
                else
                    hRegFunc = NativeMethods.Kernel32.GetProcAddress(hOcxLib, "DllUnregisterServer");
                if (hRegFunc == IntPtr.Zero)
                {
                    NativeMethods.Kernel32.FreeLibrary(hOcxLib);
                    return false;
                }

                int nRet = NativeMethods.User32.CallWindowProc(hRegFunc, IntPtr.Zero, 0, IntPtr.Zero, IntPtr.Zero);
                NativeMethods.Kernel32.FreeLibrary(hOcxLib);
                return nRet == 0;
            }

            /// <summary>
            /// 执行一段批处理命令文本内容,会自动将命令文本内容保存到指定的文件
            /// </summary>
            /// <param name="szBatchCommand">批处理命令文本内容</param>
            /// <param name="szBatFileSavePath">批处理文件保存路径</param>
            public static void ExecuteBatchCommand(string szBatchCommand, string szBatFileSavePath)
            {
                if (GlobalMethods.Misc.IsEmptyString(szBatchCommand))
                    return;
                try
                {
                    //把{app_path}替换为运行目录路径
                    szBatchCommand = szBatchCommand.Replace("{app_path}", GlobalMethods.Misc.GetWorkingPath());
                    //将命令写入BAT批处理文件
                    GlobalMethods.IO.WriteFileText(szBatFileSavePath, szBatchCommand);
                    //以窗口隐藏的方式执行BAT文件
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.FileName = szBatFileSavePath;
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    System.Diagnostics.Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.ExecuteBatchCommand", ex);
                }
            }

            /// <summary>
            /// 将指针指向的地址上的数据转换为字符串
            /// </summary>
            /// <param name="hTextData">指针指向的地址</param>
            /// <returns>string</returns>
            public static string PtrToString(IntPtr hTextData)
            {
                if (hTextData == IntPtr.Zero)
                    return null;
                try
                {
                    NativeStructs.COPYDATASTRUCT stTextData = (NativeStructs.COPYDATASTRUCT)
                        System.Runtime.InteropServices.Marshal.PtrToStructure(hTextData, typeof(NativeStructs.COPYDATASTRUCT));

                    byte[] byteTextData = new byte[stTextData.cbData];
                    System.Runtime.InteropServices.Marshal.Copy(stTextData.lpData, byteTextData, 0, byteTextData.Length);

                    return Convert.GetDefaultEncoding().GetString(byteTextData);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.PtrToString", new string[] { "hTextData" }, new object[] { hTextData }, ex);
                    return null;
                }
            }

            /// <summary>
            /// 将指定字符串数据映射到内存地址,然后返回这个地址
            /// </summary>
            /// <param name="szTextData">指定字符串数据</param>
            /// <returns>IntPtr</returns>
            public static IntPtr StringToPtr(string szTextData)
            {
                if (GlobalMethods.Misc.IsEmptyString(szTextData))
                    return IntPtr.Zero;
                try
                {
                    byte[] byteTextData = Convert.GetDefaultEncoding().GetBytes(szTextData);
                    IntPtr hTextData = System.Runtime.InteropServices.Marshal.AllocHGlobal(byteTextData.Length);
                    System.Runtime.InteropServices.Marshal.Copy(byteTextData, 0, hTextData, byteTextData.Length);

                    NativeStructs.COPYDATASTRUCT copyDataStruct = new NativeStructs.COPYDATASTRUCT();
                    copyDataStruct.dwData = IntPtr.Zero;
                    copyDataStruct.lpData = hTextData;
                    copyDataStruct.cbData = byteTextData.Length;

                    hTextData = System.Runtime.InteropServices.Marshal.AllocHGlobal(System.Runtime.InteropServices.Marshal.SizeOf(copyDataStruct));
                    System.Runtime.InteropServices.Marshal.StructureToPtr(copyDataStruct, hTextData, true);

                    return hTextData;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.StringToPtr", new string[] { "szTextData" }, new object[] { szTextData }, ex);
                    return IntPtr.Zero;
                }
            }
        }
    }
}
