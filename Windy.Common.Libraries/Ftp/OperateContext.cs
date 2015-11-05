// ***********************************************************
// FTP���������ʻ���������֮���ʲ��������Ķ���
// ��ҪΪ֧�ֶ��̷߳���ͬһ��FtpAccess��ʵ��������
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
        /// ���ӵ�FTPָ����·��
        /// </summary>
        /// <param name="path">·��</param>
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
                LogManager.Instance.WriteLog("FtpAccess.OpenConnection", "FTP���ʲ���δ��������!");
                return false;
            }

            this.m_hInternet = NativeMethods.WinInet.InternetOpen("CommonFTP"
                , NativeMethods.WinInet.INTERNET_OPEN_TYPE_PRECONFIG, null, null, 0);
            if (this.m_hInternet == null || this.m_hInternet == IntPtr.Zero)
            {
                LogManager.Instance.WriteLog("FtpAccess.OpenConnection", "InternetOpenִ��ʧ��!", this.GetWin32Ex());
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
                LogManager.Instance.WriteLog("FtpAccess.OpenConnection", "InternetConnectִ��ʧ��!", this.GetWin32Ex());
                return false;
            }

            if (!NativeMethods.WinInet.FtpSetCurrentDirectory(this.m_hConnect, "/"))
            {
                LogManager.Instance.WriteLog("FtpAccess.OpenConnection", "FtpSetCurrentDirectoryִ��ʧ��!", this.GetWin32Ex());
                return false;
            }
            return true;
        }

        /// <summary>
        /// �ر�FTP����
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
        /// �ж��ļ����ļ����Ƿ����
        /// </summary>
        /// <param name="path">�ļ����ļ���·��</param>
        /// <param name="bIsFolder">��Դ�Ƿ����ļ���</param>
        /// <returns>true:��Դ����;false:��Դ������</returns>
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
        /// �ϴ������ļ���������
        /// </summary>
        /// <param name="szLocalFile">����Դ�ļ�ȫ·��</param>
        /// <param name="szRemoteFile">Ŀ�ķ������ļ�ȫ·��</param>
        /// <returns>true:�ϴ��ɹ�;false:�ϴ�ʧ��</returns>
        public bool Upload(string szLocalFile, string szRemoteFile)
        {
            if (!File.Exists(szLocalFile))
            {
                LogManager.Instance.WriteLog("FtpAccess.Upload", new string[] { "szLocalFile", "szRemoteFile" }
                    , new string[] { szLocalFile, szRemoteFile }, "�����ļ�������!", null);
                return false;
            }

            // �ϴ��ļ�
            if (!NativeMethods.WinInet.FtpPutFile(this.m_hConnect, szLocalFile, szRemoteFile, NativeMethods.WinInet.FTP_TRANSFER_TYPE_BINARY, 0))
            {
                LogManager.Instance.WriteLog("FtpAccess.Upload", new string[] { "szLocalFile", "szRemoteFile" }
                    , new string[] { szLocalFile, szRemoteFile }, "FtpPutFileִ��ʧ��", this.GetWin32Ex());
                return false;
            }
            return true;
        }

        /// <summary>
        /// ����ָ����FTP·���ϵ��ļ�
        /// </summary>
        /// <param name="szRemoteFile">Զ���ļ�</param>
        /// <param name="szLocalFile">���汾���ļ���</param>
        /// <returns>true:���سɹ�;false:����ʧ��</returns>
        public bool Download(string szRemoteFile, string szLocalFile)
        {
            string szLocalDir = GlobalMethods.IO.GetFilePath(szLocalFile);
            if (!GlobalMethods.IO.CreateDirectory(szLocalDir))
            {
                LogManager.Instance.WriteLog("FtpAccess.Download", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szRemoteFile, szLocalFile }, "�޷���������Ŀ¼!", null);
                return false;
            }

            if (!GlobalMethods.IO.DeleteFile(szLocalFile))
            {
                LogManager.Instance.WriteLog("FtpAccess.Download", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szRemoteFile, szLocalFile }, "�޷�ɾ�������Ѵ����ļ�!", null);
                return false;
            }

            if (!this.ResExists(szRemoteFile, false))
            {
                LogManager.Instance.WriteLog("FtpAccess.Download", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szRemoteFile, szLocalFile }, "Զ���ļ�������!", null);
                return false;
            }

            if (!NativeMethods.WinInet.FtpGetFile(this.m_hConnect, szRemoteFile, szLocalFile, false, 0, NativeMethods.WinInet.FTP_TRANSFER_TYPE_BINARY, 0))
            {
                LogManager.Instance.WriteLog("FtpAccess.Download", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szRemoteFile, szLocalFile }, "FtpGetFileִ��ʧ��!", this.GetWin32Ex());
                return false;
            }
            if (!File.Exists(szLocalFile))
            {
                LogManager.Instance.WriteLog("FtpAccess.Download", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szRemoteFile, szLocalFile }, "�ļ����سɹ�,��δ�������ɵı����ļ�!", null);
                return false;
            }
            return true;
        }

        /// <summary>
        /// ɾ��һ��Զ���ļ�
        /// </summary>
        /// <param name="szRemoteFile">Զ���ļ�</param>
        /// <returns>short</returns>
        public bool DeleteFile(string szRemoteFile)
        {
            if (!this.ResExists(szRemoteFile, false))
            {
                LogManager.Instance.WriteLog("FtpAccess.FtpDeleteFile", new string[] { "szRemoteFile" }
                    , new string[] { szRemoteFile }, "Զ���ļ�������!", null);
                return false;
            }

            if (!NativeMethods.WinInet.FtpDeleteFile(this.m_hConnect, szRemoteFile))
            {
                LogManager.Instance.WriteLog("FtpAccess.FtpDeleteFile", new string[] { "szRemoteFile" }
                    , new string[] { szRemoteFile }, "Զ���ļ�ɾ��ʧ��!", this.GetWin32Ex());
                return false;
            }
            return true;
        }

        /// <summary>
        /// ������һ��Զ���ļ�
        /// </summary>
        /// <param name="szSouFile">����������Դ�ļ�</param>
        /// <param name="szDesFile">��������Ŀ���ļ�</param>
        /// <returns>short</returns>
        public bool RenameFile(string szSouFile, string szDesFile)
        {
            return this.MoveFile(szSouFile, szDesFile);
        }

        /// <summary>
        /// �ƶ�һ��Զ���ļ�
        /// </summary>
        /// <param name="szSouFile">���ƶ���Դ�ļ�</param>
        /// <param name="szDesFile">�ƶ�����Ŀ���ļ�</param>
        /// <returns>bool</returns>
        public bool MoveFile(string szSouFile, string szDesFile)
        {
            if (!this.ResExists(szSouFile, false))
            {
                LogManager.Instance.WriteLog("FtpAccess.MoveFile", new string[] { "szSouFile", "szDesFile" }
                    , new string[] { szSouFile, szDesFile }, "Զ��Դ�ļ�������!", null);
                return false;
            }

            string szDesFileParentDir = GlobalMethods.IO.GetFilePath(szDesFile);
            if (!this.ResExists(szDesFileParentDir, true))
            {
                LogManager.Instance.WriteLog("FtpAccess.MoveFile", new string[] { "szSouFile", "szDesFile" }
                    , new string[] { szSouFile, szDesFile }, "Զ��Ŀ��Ŀ¼������!", null);
                return false;
            }

            if (!NativeMethods.WinInet.FtpRenameFile(this.m_hConnect, szSouFile, szDesFile))
            {
                LogManager.Instance.WriteLog("FtpAccess.MoveFile", new string[] { "szRemoteFile", "szLocalFile" }
                    , new string[] { szSouFile, szDesFile }, "FtpRenameFileִ��ʧ��!", this.GetWin32Ex());
                return false;
            }
            return true;
        }

        /// <summary>
        /// ����ָ��·��������ȱʧ��Ŀ¼
        /// </summary>
        /// <param name="dirPath">Ŀ¼</param>
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
                        , new string[] { szDirPath }, "CreateDirectoryִ��ʧ��!", this.GetWin32Ex());
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// ��ȡָ��Ŀ¼���ļ��б�
        /// </summary>
        /// <param name="szDirPath">Ŀ¼·��</param>
        /// <param name="bFolder">�Ƿ�Ϊ�ļ�������</param>
        /// <param name="lstFilePath">���ص��ļ��б�</param>
        /// <returns>short</returns>
        public bool GetFileList(string szDirPath, bool bFolder, ref List<string> lstFilePath)
        {
            if (!this.ResExists(szDirPath, true))
            {
                LogManager.Instance.WriteLog("FtpAccess.GetFileList", new string[] { "szDirPath", "bFolder" }
                    , new string[] { szDirPath, bFolder.ToString() }, "Զ��Ŀ¼������!", null);
                return false;
            }
            NativeMethods.WinInet.WIN32_FIND_DATA stFileInfo = new NativeMethods.WinInet.WIN32_FIND_DATA();
            IntPtr hFile = NativeMethods.WinInet.FtpFindFirstFile(this.m_hConnect, szDirPath, stFileInfo, 0, 0);
            if (hFile == IntPtr.Zero)
                return false;

            if (lstFilePath == null)
                lstFilePath = new List<string>();

            // ������Ŀ¼����ָ������Ŀ¼�Ƿ����
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
        /// ָ�����ļ������Ƿ����ļ�������
        /// </summary>
        /// <param name="dwFileAttributes">�ļ�����</param>
        /// <returns>bool</returns>
        private bool IsDirectory(UInt32 dwFileAttributes)
        {
            return (dwFileAttributes & NativeMethods.WinInet.FILE_ATTRIBUTE_DIRECTORY) == NativeMethods.WinInet.FILE_ATTRIBUTE_DIRECTORY;
        }

        private Exception GetWin32Ex()
        {
            int nErrCode = Marshal.GetLastWin32Error();
            if (nErrCode == 0)
                return new Exception("δ֪����!");
            return new Win32Exception(nErrCode);
        }

        public override string ToString()
        {
            return string.Format("IP={0};PORT={1};USER={2}", this.m_ftpIP, this.m_ftpPort.ToString(), this.m_userName);
        }
    }
}