// ***********************************************************
// 创建指定名称的内存映射共享文件,系统单实例运行需要.
// Creator:YangMingkun  Date:2009-6-27
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Windy.Common.Libraries
{
    /// <summary>
    /// 创建指定名称的内存映射共享文件
    /// </summary>
    public class FileMapping
    {
        private string m_szMappingName = null;
        private const int MAPPING_SIZE = 8; // sizeof(int64)
        private IntPtr m_hFileMapping = IntPtr.Zero;
        private bool m_bIsFirstInstance = false;

        /// <summary>
        /// 获取当前指定的MappingName是否已经存在
        /// </summary>
        public bool IsFirstInstance
        {
            get { return this.m_bIsFirstInstance; }
        }

        /// <summary>
        /// 获取当前指定的MappingName是否已经存在
        /// </summary>
        public string MappingName
        {
            get { return this.m_szMappingName; }
        }

        /// <summary>
        /// 创建指定名称的内存映射文件,会设置IsFirstInstance属性
        /// </summary>
        /// <param name="szMonikerName">文件名称</param>
        public FileMapping(string szMonikerName)
        {
            int nWin32Error = NativeConstants.ERROR_SUCCESS;

            if (GlobalMethods.Misc.IsEmptyString(szMonikerName) || szMonikerName.IndexOf('\\') != -1)
            {
                throw new ArgumentException(string.Format("别名{0}包含非法字符!"));
            }

            this.m_szMappingName = string.Format("Local\\{0}", szMonikerName);

            this.m_hFileMapping = NativeMethods.Kernel32.CreateFileMappingW(
                NativeConstants.INVALID_HANDLE_VALUE,
                IntPtr.Zero,
                NativeConstants.PAGE_READWRITE | NativeConstants.SEC_COMMIT,
                0,
                MAPPING_SIZE,
                this.m_szMappingName);

            nWin32Error = Marshal.GetLastWin32Error();

            if (this.m_hFileMapping == IntPtr.Zero)
            {
                throw new Win32Exception(nWin32Error, string.Format("系统函数CreateFileMappingW返回NULL({0})!", nWin32Error));
            }

            this.m_bIsFirstInstance = (nWin32Error != NativeConstants.ERROR_ALREADY_EXISTS);
        }

        public override string ToString()
        {
            return this.m_szMappingName;
        }

        /// <summary>
        /// 释放内存映射文件对象引用
        /// </summary>
        /// <param name="bClearHandle">是否清除文件中的句柄</param>
        public void Dispose(bool bClearHandle)
        {
            if (bClearHandle)
            {
                this.WriteHandleValue(IntPtr.Zero);
            }
            if (this.m_hFileMapping != IntPtr.Zero)
            {
                NativeMethods.Kernel32.CloseHandle(this.m_hFileMapping);
                this.m_hFileMapping = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 写指定的Handle值到当前内存映射文件
        /// </summary>
        /// <param name="hValue">Handle值</param>
        /// <returns>bool</returns>
        public bool WriteHandleValue(IntPtr hValue)
        {
            int nWin32Error = NativeConstants.ERROR_SUCCESS;
            bool bResult = true;

            IntPtr lpData = NativeMethods.Kernel32.MapViewOfFile(
                this.m_hFileMapping,
                NativeConstants.FILE_MAP_WRITE,
                0,
                0,
                new UIntPtr((uint)MAPPING_SIZE));

            if (lpData == IntPtr.Zero)
            {
                nWin32Error = Marshal.GetLastWin32Error();
                LogManager.Instance.WriteLog("FileMapping.WriteHandleValueToMappedFile"
                    , new Win32Exception(nWin32Error, string.Format("系统函数MapViewOfFile返回NULL({0})!", nWin32Error)));
                return false;
            }

            long int64 = hValue.ToInt64();
            byte[] int64Bytes = new byte[(int)MAPPING_SIZE];

            for (int index = 0; index < MAPPING_SIZE; ++index)
            {
                int64Bytes[index] = (byte)((int64 >> (index * 8)) & 0xff);
            }

            Marshal.Copy(int64Bytes, 0, lpData, MAPPING_SIZE);

            bResult = NativeMethods.Kernel32.UnmapViewOfFile(lpData);
            if (!bResult)
            {
                nWin32Error = Marshal.GetLastWin32Error();
                LogManager.Instance.WriteLog("FileMapping.WriteHandleValueToMappedFile"
                   , new Win32Exception(nWin32Error, string.Format("系统函数UnmapViewOfFile返回FLASE({0})!", nWin32Error)));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 读取指定的指定的内存映射文件内保存的Handle值
        /// </summary>
        /// <param name="timeoutSeconds">超时时间</param>
        /// <returns>IntPtr</returns>
        public IntPtr ReadHandleValue(int timeoutSeconds)
        {
            DateTime dtNow = DateTime.Now;
            DateTime dtTimeoutTime = dtNow.AddSeconds(timeoutSeconds);

            IntPtr hValue = IntPtr.Zero;
            while (hValue == IntPtr.Zero && dtNow <= dtTimeoutTime)
            {
                hValue = this.ReadHandleValue();
                dtNow = DateTime.Now;

                if (hValue == IntPtr.Zero)
                    System.Threading.Thread.Sleep(100);
            }
            return hValue;
        }

        /// <summary>
        /// 读取指定的指定的内存映射文件内保存的Handle值
        /// </summary>
        /// <returns>IntPtr</returns>
        private IntPtr ReadHandleValue()
        {
            int nWin32Error = NativeConstants.ERROR_SUCCESS;

            IntPtr lpData = NativeMethods.Kernel32.MapViewOfFile(
                this.m_hFileMapping,
                NativeConstants.FILE_MAP_READ,
                0,
                0,
                new UIntPtr((uint)MAPPING_SIZE));

            if (lpData == IntPtr.Zero)
            {
                nWin32Error = Marshal.GetLastWin32Error();
                LogManager.Instance.WriteLog("FileMapping.ReadHandleValueFromMappedFile"
                    , new Win32Exception(nWin32Error, string.Format("系统函数MapViewOfFile返回NULL({0})!", nWin32Error)));
                return IntPtr.Zero;
            }

            byte[] int64Bytes = new byte[(int)MAPPING_SIZE];
            Marshal.Copy(lpData, int64Bytes, 0, MAPPING_SIZE);

            long int64 = 0;
            for (int index = 0; index < MAPPING_SIZE; ++index)
            {
                int64 += (long)(int64Bytes[index] << (index * 8));
            }

            bool bResult = NativeMethods.Kernel32.UnmapViewOfFile(lpData);
            if (!bResult)
            {
                nWin32Error = Marshal.GetLastWin32Error();
                LogManager.Instance.WriteLog("FileMapping.ReadHandleValueFromMappedFile"
                    , new Win32Exception(nWin32Error, string.Format("系统函数UnmapViewOfFile返回FLASE({0})!", nWin32Error)));
                return IntPtr.Zero;
            }
            return new IntPtr(int64);
        }
    }
}
