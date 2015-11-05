// ***********************************************************
// ��װһЩ�����Ĳ���ϵͳ��ز�����������
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
        /// ��װ����ϵͳ��ز���
        /// </summary>
        public struct Win32
        {
            /// <summary>
            /// ��ȡϵͳ���Ѿ���װ�Ĵ�ӡ���б�
            /// </summary>
            /// <returns>��ӡ���б�</returns>
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
            /// ����ָ����ӡ��ΪĬ�ϴ�ӡ��
            /// </summary>
            /// <param name="szPrinterName">��ӡ������</param>
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
            /// ��ȡ����ϵͳ��ǰĬ�ϴ�ӡ������
            /// </summary>
            /// <return>Ĭ�ϴ�ӡ������</return>
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
            /// ��ȡϵͳ�а�װ�Ĵ�ӡ����Ŀ
            /// </summary>
            /// <returns>��ӡ����Ŀ</returns>
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
            /// ��ȡָ����ӡ���������Ϣ
            /// </summary>
            /// <param name="szPrinterName">��ӡ������</param>
            /// <param name="stPrinterInfo">��ӡ����Ϣ</param>
            /// <returns>�Ƿ�ִ�гɹ�</returns>
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
            /// ���ݴ�ӡ����״̬������ȡ״̬����
            /// </summary>
            /// <param name="nStatus">��ӡ����״̬</param>
            /// <returns>״̬��������</returns>
            public static string GetPrinterStatus(int nStatus)
            {
                string strStatus = string.Empty;
                switch (nStatus)
                {
                    case -1:
                        strStatus = string.Empty;
                        break;
                    case 0:
                        strStatus = "׼��������Ready��";
                        break;
                    case 0x00000200:
                        strStatus = "æ��Busy��";
                        break;
                    case 0x00400000:
                        strStatus = "���򿪣�Printer Door Open��";
                        break;
                    case 0x00000002:
                        strStatus = "����Printer Error��";
                        break;
                    case 0x0008000:
                        strStatus = "��ʼ����Initializing��";
                        break;
                    case 0x00000100:
                        strStatus = "��������,�����I/O Active��";
                        break;
                    case 0x00000020:
                        strStatus = "�ֹ���ֽ��Manual Feed��";
                        break;
                    case 0x00040000:
                        strStatus = "��ī�ۣ�No Toner��";
                        break;
                    case 0x00001000:
                        strStatus = "�����ã�Not Available��";
                        break;
                    case 0x00000080:
                        strStatus = "�ѻ���Off Line��";
                        break;
                    case 0x00200000:
                        strStatus = "�ڴ������Out of Memory��";
                        break;
                    case 0x00000800:
                        strStatus = "�����������Output Bin Full��";
                        break;
                    case 0x00080000:
                        strStatus = "��ǰҳ�޷���ӡ��Page Punt��";
                        break;
                    case 0x00000008:
                        strStatus = "��ֽ��Paper Jam��";
                        break;
                    case 0x00000010:
                        strStatus = "��ӡֽ���꣨Paper Out��";
                        break;
                    case 0x00000040:
                        strStatus = "ֽ�����⣨Page Problem��";
                        break;
                    case 0x00000001:
                        strStatus = "��ͣ��Paused��";
                        break;
                    case 0x00000004:
                        strStatus = "����ɾ����Pending Deletion��";
                        break;
                    case 0x00000400:
                        strStatus = "���ڴ�ӡ��Printing��";
                        break;
                    case 0x00004000:
                        strStatus = "���ڴ���Processing��";
                        break;
                    case 0x00020000:
                        strStatus = "ī�۲��㣨Toner Low��";
                        break;
                    case 0x00100000:
                        strStatus = "��Ҫ�û���Ԥ��User Intervention��";
                        break;
                    case 0x20000000:
                        strStatus = "�ȴ���Waiting��";
                        break;
                    case 0x00010000:
                        strStatus = "�Ȼ��У�Warming Up��";
                        break;
                    default:
                        strStatus = "δ֪״̬��Unknown Status��";
                        break;
                }
                return strStatus;
            }

            /// <summary>
            /// ��ȡ��ǰ�û����û���������
            /// </summary>
            /// <returns>��������</returns>
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
            /// ��ȡʹ���ڴ�ӳ���ļ����ƽ��е�ʵ�����е�ϵͳ�ľ��
            /// </summary>
            /// <param name="systemName">ϵͳ��ʶ��</param>
            /// <returns>����ϵͳ�����ڵľ��</returns>
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
            /// ��װ�򷴰�װָ����ActiveX�ؼ�
            /// </summary>
            /// <param name="szFileName">ActiveX�ؼ�ȫ·��</param>
            /// <param name="bIsUninstall">ִ�з���װ</param>
            /// <returns>true-ִ�гɹ�;false-ִ��ʧ��</returns>
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
            /// ִ��һ�������������ı�����,���Զ��������ı����ݱ��浽ָ�����ļ�
            /// </summary>
            /// <param name="szBatchCommand">�����������ı�����</param>
            /// <param name="szBatFileSavePath">�������ļ�����·��</param>
            public static void ExecuteBatchCommand(string szBatchCommand, string szBatFileSavePath)
            {
                if (GlobalMethods.Misc.IsEmptyString(szBatchCommand))
                    return;
                try
                {
                    //��{app_path}�滻Ϊ����Ŀ¼·��
                    szBatchCommand = szBatchCommand.Replace("{app_path}", GlobalMethods.Misc.GetWorkingPath());
                    //������д��BAT�������ļ�
                    GlobalMethods.IO.WriteFileText(szBatFileSavePath, szBatchCommand);
                    //�Դ������صķ�ʽִ��BAT�ļ�
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
            /// ��ָ��ָ��ĵ�ַ�ϵ�����ת��Ϊ�ַ���
            /// </summary>
            /// <param name="hTextData">ָ��ָ��ĵ�ַ</param>
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
            /// ��ָ���ַ�������ӳ�䵽�ڴ��ַ,Ȼ�󷵻������ַ
            /// </summary>
            /// <param name="szTextData">ָ���ַ�������</param>
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
