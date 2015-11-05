// ***********************************************************
// Ftp������������,��Ҫ���ڲ����������ķ���
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Threading;

namespace Windy.Common.Libraries.Ftp
{
    public class FtpAccess
    {
        private string m_ftpIP = null;
        private int m_ftpPort = -1;
        private FtpMode m_ftpMode = FtpMode.PASV;
        private string m_userName = null;
        private string m_password = null;

        private Dictionary<string, OperateContext> m_contexts = null;
        private object m_objectLocker = null;

        /// <summary>
        /// ��ȡ������FTP�ķ���IP��ַ
        /// </summary>
        public string FtpIP
        {
            get { return this.m_ftpIP; }
            set { this.m_ftpIP = value; }
        }

        /// <summary>
        /// ��ȡ������FTP�ķ��ʶ˿ں�
        /// </summary>
        public int FtpPort
        {
            get { return this.m_ftpPort; }
            set { this.m_ftpPort = value; }
        }

        /// <summary>
        /// ��ȡ������FTP�ķ����û�����
        /// </summary>
        public string UserName
        {
            get { return this.m_userName; }
            set { this.m_userName = value; }
        }

        /// <summary>
        /// ��ȡ������FTP�ķ����û�����
        /// </summary>
        public string Password
        {
            get { return this.m_password; }
            set { this.m_password = value; }
        }

        /// <summary>
        /// ��ȡ������FTPЭ��Ĺ���ģʽ
        /// </summary>
        public FtpMode FtpMode
        {
            get { return this.m_ftpMode; }
            set { this.m_ftpMode = value; }
        }

        /// <summary>
        /// ��ȡ��ǰFTP����������
        /// </summary>
        private OperateContext CurrentContext
        {
            get
            {
                lock (this.m_objectLocker)
                { return this.GetCurrentContext(); }
            }
        }

        public FtpAccess()
        {
            this.m_contexts = new Dictionary<string, OperateContext>();
            this.m_objectLocker = new object();
        }

        /// <summary>
        /// ��ȡ��ǰFTP����������
        /// </summary>
        /// <returns>OperateContext</returns>
        private OperateContext GetCurrentContext()
        {
            string szThreadID =
               Thread.CurrentThread.ManagedThreadId.ToString();
            OperateContext context = null;
            if (this.m_contexts.ContainsKey(szThreadID))
                context = this.m_contexts[szThreadID];
            if (context != null)
                return context;
            context = new OperateContext(szThreadID, this.m_ftpIP
                , this.m_ftpPort, this.m_ftpMode, this.m_userName, this.m_password);
            this.m_contexts.Add(szThreadID, context);
            return context;
        }

        /// <summary>
        /// ���ӵ�FTPָ����·��
        /// </summary>
        /// <param name="path">·��</param>
        public bool OpenConnection()
        {
            return this.CurrentContext.OpenConnection();
        }

        /// <summary>
        /// �ر�FTP����
        /// </summary>
        public void CloseConnection()
        {
            this.CurrentContext.CloseConnection();
        }

        /// <summary>
        /// �ж��ļ����ļ����Ƿ����
        /// </summary>
        /// <param name="path">�ļ����ļ���·��</param>
        /// <param name="bIsFolder">��Դ�Ƿ����ļ���</param>
        /// <returns>true:��Դ����;false:��Դ������</returns>
        public bool ResExists(string szResPath, bool bIsFolder)
        {
            return this.CurrentContext.ResExists(szResPath, bIsFolder);
        }

        /// <summary>
        /// �ϴ������ļ���������
        /// </summary>
        /// <param name="szLocalFile">����Դ�ļ�ȫ·��</param>
        /// <param name="szRemoteFile">Ŀ�ķ������ļ�ȫ·��</param>
        /// <returns>true:�ϴ��ɹ�;false:�ϴ�ʧ��</returns>
        public bool Upload(string szLocalFile, string szRemoteFile)
        {
            return this.CurrentContext.Upload(szLocalFile, szRemoteFile);
        }

        /// <summary>
        /// ����ָ����FTP·���ϵ��ļ�
        /// </summary>
        /// <param name="szRemoteFile">Զ���ļ�</param>
        /// <param name="szLocalFile">���汾���ļ���</param>
        /// <returns>true:���سɹ�;false:����ʧ��</returns>
        public bool Download(string szRemoteFile, string szLocalFile)
        {
            return this.CurrentContext.Download(szRemoteFile, szLocalFile);
        }

        /// <summary>
        /// ɾ��һ��Զ���ļ�
        /// </summary>
        /// <param name="szRemoteFile">Զ���ļ�</param>
        /// <returns>short</returns>
        public bool DeleteFile(string szRemoteFile)
        {
            return this.CurrentContext.DeleteFile(szRemoteFile);
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
            return this.CurrentContext.MoveFile(szSouFile, szDesFile);
        }

        /// <summary>
        /// ����ָ��·��������ȱʧ��Ŀ¼
        /// </summary>
        /// <param name="dirPath">Ŀ¼</param>
        /// <returns>short</returns>
        public bool CreateDirectory(string szDirPath)
        {
            return this.CurrentContext.CreateDirectory(szDirPath);
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
            return this.CurrentContext.GetFileList(szDirPath, bFolder, ref lstFilePath);
        }

        public override string ToString()
        {
            return string.Format("IP={0};PORT={1};USER={2}", this.m_ftpIP, this.m_ftpPort.ToString(), this.m_userName);
        }
    }
}