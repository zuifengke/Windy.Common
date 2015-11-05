// ***********************************************************
// FTP服务器访问基础方法类之访问操作上下文对象
// 主要为支持多线程访问同一个FtpAccess类实例而设置
// Creator:YangMingkun  Date:2012-4-24
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Windy.Common.Libraries.Ftp
{
    internal class OperateContext
    {
        private string m_ftpIP = null;
        private int m_ftpPort = -1;
        private FtpMode m_ftpMode = FtpMode.PASV;
        private string m_userName = null;
        private string m_password = null;

        private IntPtr m_hInternet = IntPtr.Zero;
        private IntPtr m_hConnect = IntPtr.Zero;

        private string m_identifier = null;
        public string Identifier
        {
            get { return this.m_identifier; }
        }

        public OperateContext(string identifier
            , string ip, int port, FtpMode mode, string user, string pwd)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("identifier");
            this.m_identifier = identifier;
            this.m_ftpIP = ip;
            this.m_ftpPort = port;
            this.m_ftpMode = mode;
            this.m_userName = user;
            this.m_password = pwd;
        }

        /// <summary>
        /// 连接到FTP指定的路径
        /// </summary>
        /// <param name="path">路径</param>
        public bool OpenConnection()
        {
            if (this.m_hConnect != IntPtr.Zero)
            {
                if (NativeMethods.WinInet.FtpSetCurrentDirectory(this.m_hConnect, "/"))
                    return true;
            }
            this.CloseConnection();

            if (GlobalMethods.Misc.IsEmptyString(this.m_ftpIP)
                || GlobalMethods.Misc.IsEmptyString(this.m_userName)
                || GlobalMethods.Misc.IsEmptyString(this.m_password))
            {
                LogManager.Instance.WriteLog("FtpAccess.OpenConnection", "FTP访问参数未设置完整!");
                return false;
            }

            this.m_hInternet = NativeMethods.WinInet.InternetOpen("CommonFTP"
                , NativeMethods.WinInet.INTERNET_OPEN_TYPE_PRECONFIG, null, null, 0);
            if (this.m_hInternet == null || this.m_hInternet == IntPtr.Zero)
            {
                LogManager.Instance.WriteLog("FtpAccess.OpenConnection", "InternetOpen执行失败!", this.GetWin32Ex());
                return false;
            }

            int nFtpMode = NativeMethods.WinInet.INTERNET_FLAG_PASSIVE;
            if (this.m_ftpMode == FtpMode.PORT)
                nFtpMode = NativeMethods.WinInet.INTERNET_FLAG_PORT;

            this.m_hConnect = NativeMethods.WinInet.InternetConnect(this.m_hInternet
                , this.m_ftpIP, this.m_ftpPort, this.m_userName, this.m_password
                , NativeMethods.WinInet.INTERNET_SERVICE_FTP, nFtpMode, 0);
            if (this.m_hConnect == null || this.m_hConnect == IntPtr.Zero)
            {
                LogManager.Instance.WriteLog("FtpAccess.OpenConnection", "InternetConnect执行失败!", this.GetWin32Ex());
                return false;
            }

            if (!NativeMethods.WinInet.FtpSetCurrentDirectory(this.m_hConnect, "/"))
            {
                LogManager.Instance.WriteLog("FtpAccess.OpenConnection", "FtpSetCurrentDirectory执行失败!", this.GetWin32Ex());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 关闭FTP连接
        /// </summary>
        public void CloseConnection()
        {
            if (this.m_hConnect != IntPtr.Zero)
            {
                NativeMethods.WinInet.InternetCloseHandle(this.m_hConnect);
                this.m_hConnect = IntPtr.Zero;
            }
            if (this.m_hInternet != IntPtr.Zero)
            {
                NativeMethods.WinInet.InternetCloseHandle(this.m_hInternet);
                this.m_hInternet = IntPtr.Zero;
            }
        }

        /// <summary>
        /// 判断文件或文件夹是否存在
        /// </summary>
        /// <param name="path">文件或文件夹路径</param>
        /// <param name="bIsFolder">资源是否是文件夹</param>
        /// <returns>true:资源存在;false:资源不存在</returns>
        public bool ResExists(string szResPath, bool bIsFolder)
        {
            szResPath = szResPath.Trim();
            if (szResPath == "/" || szResPath == "")
                return true;
            if (bIsFolder)
            {
                if (!NativeMethods.WinInet.FtpSetCurrentDirectory(this.m_hConnect, szResPath))
                    return false;
                NativeMethods.WinInet.FtpSetCurrentDirectory(this.m_hConnect, "/");
                return true;
            }
            else
            {
                NativeMethods.WinInet.WIN32_FIND_DATA stFileInfo = new NativeMethods.WinInet.WIN32_FIND_DATA();
                IntPtr hFile = NativeMethods.WinInet.FtpFindFirstFile(this.m_hConnect, szResPath, stFileInfo, 0, 0);
                if (hFile == null || hFile == IntPtr.Zero)
                    return false;
                NativeMethods.WinInet.InternetCloseHandle(hFile);
                return true;
            }
        }

        /// <summary>
        /// 上传本地文件到服务器
        /// </summary>
        /// <param name="szLocalFile">本地源文件全路径</param>
        /// <param name="szRemoteFile">目的服务器文件全路径</param>
        /// <returns>true:上传成功;false:上传失败</returns>
        public bool Upload(string szLocalFile, string szRemoteFile)
        {
            if (!File.Exists(szLocalFile))
            {
                LogManager.Instance.WriteLog("FtpAccess.Upload", new string[] { "szLocalFile", "szRemoteFile" }
                    , new string[] { szLocalFile, szRemoteFile }, "本地文件不存在!", null);
                return false;
            }

            // 上传文件
            if (!NativeMethods.WinInet.FtpPutFile(this.m_hConnect, szLocalFile, szRemoteFile, NativeMethods.WinInet.FTP_TRANSFER_TYPE_BINARY, 0))
            {
                LogManager.Instance.WriteLog("FtpAccess.Upload", new string[] { "szLocalFile", "szRemoteFile" }
                    , new string[] { szLocalFile, szRemoteFile }, "FtpPutFile执行失败", this.GetWin32Ex());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 下载指定的FTP路径上的文件
        /// </summary>
        /// <param name="szRemoteFile">远程文件</param>
        /// <param name="szLocalFile">保存本地文件名</param>
        /// <returns>true:下载成功;false:下载失败</returns>
        public bool Download(string szRemoteFile, string szLocalFile)
        {
            string szLocalDir = GlobalMethods.IO.GetFilePath(szLocalFile);
            if (!GlobalMethods.IO.CreateDirectory(szLocalDir))
            {
                LogManager.Instance.WriteLog("FtpAccess.Download", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szRemoteFile, szLocalFile }, "无法创建本地目录!", null);
                return false;
            }

            if (!GlobalMethods.IO.DeleteFile(szLocalFile))
            {
                LogManager.Instance.WriteLog("FtpAccess.Download", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szRemoteFile, szLocalFile }, "无法删除本地已存在文件!", null);
                return false;
            }

            if (!this.ResExists(szRemoteFile, false))
            {
                LogManager.Instance.WriteLog("FtpAccess.Download", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szRemoteFile, szLocalFile }, "远程文件不存在!", null);
                return false;
            }

            if (!NativeMethods.WinInet.FtpGetFile(this.m_hConnect, szRemoteFile, szLocalFile, false, 0, NativeMethods.WinInet.FTP_TRANSFER_TYPE_BINARY, 0))
            {
                LogManager.Instance.WriteLog("FtpAccess.Download", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szRemoteFile, szLocalFile }, "FtpGetFile执行失败!", this.GetWin32Ex());
                return false;
            }
            if (!File.Exists(szLocalFile))
            {
                LogManager.Instance.WriteLog("FtpAccess.Download", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szRemoteFile, szLocalFile }, "文件下载成功,但未发现生成的本地文件!", null);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除一个远程文件
        /// </summary>
        /// <param name="szRemoteFile">远程文件</param>
        /// <returns>short</returns>
        public bool DeleteFile(string szRemoteFile)
        {
            if (!this.ResExists(szRemoteFile, false))
            {
                LogManager.Instance.WriteLog("FtpAccess.FtpDeleteFile", new string[] { "szRemoteFile" }
                    , new string[] { szRemoteFile }, "远程文件不存在!", null);
                return false;
            }

            if (!NativeMethods.WinInet.FtpDeleteFile(this.m_hConnect, szRemoteFile))
            {
                LogManager.Instance.WriteLog("FtpAccess.FtpDeleteFile", new string[] { "szRemoteFile" }
                    , new string[] { szRemoteFile }, "远程文件删除失败!", this.GetWin32Ex());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 重命名一个远程文件
        /// </summary>
        /// <param name="szSouFile">被重命名的源文件</param>
        /// <param name="szDesFile">重命名的目标文件</param>
        /// <returns>short</returns>
        public bool RenameFile(string szSouFile, string szDesFile)
        {
            return this.MoveFile(szSouFile, szDesFile);
        }

        /// <summary>
        /// 移动一个远程文件
        /// </summary>
        /// <param name="szSouFile">被移动的源文件</param>
        /// <param name="szDesFile">移动到的目标文件</param>
        /// <returns>bool</returns>
        public bool MoveFile(string szSouFile, string szDesFile)
        {
            if (!this.ResExists(szSouFile, false))
            {
                LogManager.Instance.WriteLog("FtpAccess.MoveFile", new string[] { "szSouFile", "szDesFile" }
                    , new string[] { szSouFile, szDesFile }, "远程源文件不存在!", null);
                return false;
            }

            string szDesFileParentDir = GlobalMethods.IO.GetFilePath(szDesFile);
            if (!this.ResExists(szDesFileParentDir, true))
            {
                LogManager.Instance.WriteLog("FtpAccess.MoveFile", new string[] { "szSouFile", "szDesFile" }
                    , new string[] { szSouFile, szDesFile }, "远程目标目录不存在!", null);
                return false;
            }

            if (!NativeMethods.WinInet.FtpRenameFile(this.m_hConnect, szSouFile, szDesFile))
            {
                LogManager.Instance.WriteLog("FtpAccess.MoveFile", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szSouFile, szDesFile }, "FtpRenameFile执行失败!", this.GetWin32Ex());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建指定路径上所有缺失的目录
        /// </summary>
        /// <param name="dirPath">目录</param>
        /// <returns>short</returns>
        public bool CreateDirectory(string szDirPath)
        {
            if (this.ResExists(szDirPath, true))
                return true;

            string[] arrDirName = szDirPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            szDirPath = string.Empty;
            for (int index = 0; index < arrDirName.Length; index++)
            {
                string szDirName = arrDirName[index];
                if (szDirName == null || szDirName.Trim() == string.Empty)
                    continue;
                szDirPath += "/" + szDirName;
                if (this.ResExists(szDirPath, true))
                    continue;
                if (!NativeMethods.WinInet.FtpCreateDirectory(this.m_hConnect, szDirPath))
                {
                    LogManager.Instance.WriteLog("FtpAccess.CreateDirectory", new string[] { "szDirPath" }
                        , new string[] { szDirPath }, "CreateDirectory执行失败!", this.GetWin32Ex());
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取指定目录的文件列表
        /// </summary>
        /// <param name="szDirPath">目录路径</param>
        /// <param name="bFolder">是否为文件夹属性</param>
        /// <param name="lstFilePath">返回的文件列表</param>
        /// <returns>short</returns>
        public bool GetFileList(string szDirPath, bool bFolder, ref List<string> lstFilePath)
        {
            if (!this.ResExists(szDirPath, true))
            {
                LogManager.Instance.WriteLog("FtpAccess.GetFileList", new string[] { "szDirPath", "bFolder" }
                    , new string[] { szDirPath, bFolder.ToString() }, "远程目录不存在!", null);
                return false;
            }
            NativeMethods.WinInet.WIN32_FIND_DATA stFileInfo = new NativeMethods.WinInet.WIN32_FIND_DATA();
            IntPtr hFile = NativeMethods.WinInet.FtpFindFirstFile(this.m_hConnect, szDirPath, stFileInfo, 0, 0);
            if (hFile == IntPtr.Zero)
                return false;

            if (lstFilePath == null)
                lstFilePath = new List<string>();

            // 遍历父目录查找指定的子目录是否存在
            bool bSuccess = true;
            while (bSuccess)
            {
                if (bFolder)
                {
                    if (this.IsDirectory(stFileInfo.dwFileAttributes))
                    {
                        string szChildPath = string.Format("{0}/{1}", szDirPath, stFileInfo.cFileName);
                        lstFilePath.Add(szChildPath);
                    }
                }
                else
                {
                    if (!this.IsDirectory(stFileInfo.dwFileAttributes))
                    {
                        string szChildPath = string.Format("{0}/{1}", szDirPath, stFileInfo.cFileName);
                        lstFilePath.Add(szChildPath);
                    }
                }
                bSuccess = NativeMethods.WinInet.InternetFindNextFile(hFile, stFileInfo);
            }
            NativeMethods.WinInet.InternetCloseHandle(hFile);
            return true;
        }

        /// <summary>
        /// 指定的文件属性是否是文件夹属性
        /// </summary>
        /// <param name="dwFileAttributes">文件属性</param>
        /// <returns>bool</returns>
        private bool IsDirectory(UInt32 dwFileAttributes)
        {
            return (dwFileAttributes & NativeMethods.WinInet.FILE_ATTRIBUTE_DIRECTORY) == NativeMethods.WinInet.FILE_ATTRIBUTE_DIRECTORY;
        }

        private Exception GetWin32Ex()
        {
            int nErrCode = Marshal.GetLastWin32Error();
            if (nErrCode == 0)
                return new Exception("未知错误!");
            return new Win32Exception(nErrCode);
        }

        public override string ToString()
        {
            return string.Format("IP={0};PORT={1};USER={2}", this.m_ftpIP, this.m_ftpPort.ToString(), this.m_userName);
        }
    }
}