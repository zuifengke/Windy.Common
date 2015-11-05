// ***********************************************************
// Ftp服务器访问类,主要用于病历服务器的访问
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
        /// 获取或设置FTP的访问IP地址
        /// </summary>
        public string FtpIP
        {
            get { return this.m_ftpIP; }
            set { this.m_ftpIP = value; }
        }

        /// <summary>
        /// 获取或设置FTP的访问端口号
        /// </summary>
        public int FtpPort
        {
            get { return this.m_ftpPort; }
            set { this.m_ftpPort = value; }
        }

        /// <summary>
        /// 获取或设置FTP的访问用户名称
        /// </summary>
        public string UserName
        {
            get { return this.m_userName; }
            set { this.m_userName = value; }
        }

        /// <summary>
        /// 获取或设置FTP的访问用户密码
        /// </summary>
        public string Password
        {
            get { return this.m_password; }
            set { this.m_password = value; }
        }

        /// <summary>
        /// 获取或设置FTP协议的工作模式
        /// </summary>
        public FtpMode FtpMode
        {
            get { return this.m_ftpMode; }
            set { this.m_ftpMode = value; }
        }

        /// <summary>
        /// 获取当前FTP操作上下文
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
        /// 获取当前FTP操作上下文
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
        /// 连接到FTP指定的路径
        /// </summary>
        /// <param name="path">路径</param>
        public bool OpenConnection()
        {
            return this.CurrentContext.OpenConnection();
        }

        /// <summary>
        /// 关闭FTP连接
        /// </summary>
        public void CloseConnection()
        {
            this.CurrentContext.CloseConnection();
        }

        /// <summary>
        /// 判断文件或文件夹是否存在
        /// </summary>
        /// <param name="path">文件或文件夹路径</param>
        /// <param name="bIsFolder">资源是否是文件夹</param>
        /// <returns>true:资源存在;false:资源不存在</returns>
        public bool ResExists(string szResPath, bool bIsFolder)
        {
            return this.CurrentContext.ResExists(szResPath, bIsFolder);
        }

        /// <summary>
        /// 上传本地文件到服务器
        /// </summary>
        /// <param name="szLocalFile">本地源文件全路径</param>
        /// <param name="szRemoteFile">目的服务器文件全路径</param>
        /// <returns>true:上传成功;false:上传失败</returns>
        public bool Upload(string szLocalFile, string szRemoteFile)
        {
            return this.CurrentContext.Upload(szLocalFile, szRemoteFile);
        }

        /// <summary>
        /// 下载指定的FTP路径上的文件
        /// </summary>
        /// <param name="szRemoteFile">远程文件</param>
        /// <param name="szLocalFile">保存本地文件名</param>
        /// <returns>true:下载成功;false:下载失败</returns>
        public bool Download(string szRemoteFile, string szLocalFile)
        {
            return this.CurrentContext.Download(szRemoteFile, szLocalFile);
        }

        /// <summary>
        /// 删除一个远程文件
        /// </summary>
        /// <param name="szRemoteFile">远程文件</param>
        /// <returns>short</returns>
        public bool DeleteFile(string szRemoteFile)
        {
            return this.CurrentContext.DeleteFile(szRemoteFile);
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
            return this.CurrentContext.MoveFile(szSouFile, szDesFile);
        }

        /// <summary>
        /// 创建指定路径上所有缺失的目录
        /// </summary>
        /// <param name="dirPath">目录</param>
        /// <returns>short</returns>
        public bool CreateDirectory(string szDirPath)
        {
            return this.CurrentContext.CreateDirectory(szDirPath);
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
            return this.CurrentContext.GetFileList(szDirPath, bFolder, ref lstFilePath);
        }

        public override string ToString()
        {
            return string.Format("IP={0};PORT={1};USER={2}", this.m_ftpIP, this.m_ftpPort.ToString(), this.m_userName);
        }
    }
}