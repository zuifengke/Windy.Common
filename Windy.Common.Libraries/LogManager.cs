// ***********************************************************
// 医疗文档系统日志管理器.用于写错误日志到Windows日志表
// 或者文本文件
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace Windy.Common.Libraries
{
    public enum LogType
    {
        Error,
        Warning,
        Information
    }
    public class LogManager : IDisposable
    {
        private const int LOG_FULL_ERROR_CODE = 1502;

        private string m_szLogSourceName = "MedDocSys";
        private string m_szLogFilePath = null;
        private EventLog m_clsEventLog = null;
        private bool m_bTextLogOnly = false;
        private bool m_bAutoCleanLog = true;
        private int m_nAutoCleanPeriod = 3;

        #region "单实例模式"
        private static LogManager m_Instance = null;
        /// <summary>
        /// 单实例
        /// </summary>
        public static LogManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new LogManager();
                return m_Instance;
            }
        }
        private LogManager()
        {
        }
        #endregion

        /// <summary>
        /// 获取或设置是否仅写文本文件日志
        /// </summary>
        public bool TextLogOnly
        {
            get { return this.m_bTextLogOnly; }
            set { this.m_bTextLogOnly = value; }
        }

        /// <summary>
        /// 获取或设置是否自动清理日志文件
        /// </summary>
        public bool AutoCleanLog
        {
            get { return this.m_bAutoCleanLog; }
            set { this.m_bAutoCleanLog = value; }
        }

        /// <summary>
        /// 获取或设置日志自动清理期限
        /// </summary>
        public int AutoCleanPeriod
        {
            get { return this.m_nAutoCleanPeriod; }
            set { this.m_nAutoCleanPeriod = value; }
        }

        /// <summary>
        /// 获取或设置日志名称
        /// </summary>
        public string LogSourceName
        {
            get { return this.m_szLogSourceName; }
            set { this.m_szLogSourceName = value; }
        }

        /// <summary>
        /// 获取或设置日志保存路径
        /// </summary>
        public string LogFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_szLogFilePath))
                    this.m_szLogFilePath = this.GetLogFilePath();
                return this.m_szLogFilePath;
            }
            set { this.m_szLogFilePath = value; }
        }

        #region "Windows日志"
        /// <summary>
        /// 初始化Windows事件日志对象
        /// </summary>
        /// <returns>bool</returns>
        private bool InitEventLog()
        {
            if (this.m_clsEventLog != null)
                return true;
            try
            {
                if (!EventLog.Exists(this.m_szLogSourceName))
                    EventLog.CreateEventSource(this.m_szLogSourceName, this.m_szLogSourceName);
                this.m_clsEventLog = new EventLog();
                this.m_clsEventLog.Source = this.m_szLogSourceName;
                //不覆盖旧日志
                if (this.m_clsEventLog.OverflowAction != OverflowAction.DoNotOverwrite)
                    this.m_clsEventLog.ModifyOverflowPolicy(OverflowAction.DoNotOverwrite, -1);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 写入一条日志到Windows日志表
        /// </summary>
        /// <param name="szText">日志基本信息</param>
        public void WriteLog(string szText)
        {
            this.WriteLog(szText, null, LogType.Error);
        }

        /// <summary>
        /// 写入一条日志到Windows日志表
        /// </summary>
        /// <param name="szText">日志基本信息</param>
        /// <param name="ex">.NET例外信息</param>
        public void WriteLog(string szText, Exception ex)
        {
            this.WriteLog(szText, ex, LogType.Error);
        }

        /// <summary>
        /// 写方法执行失败的错误信息
        /// </summary>
        /// <param name="szMethodName">方法名</param>
        /// <param name="szExtraInfo">错误信息</param>
        public void WriteLog(string szMethodName, string szErrorInfo)
        {
            this.WriteLog(szMethodName, null, null, szErrorInfo, null);
        }

        /// <summary>
        /// 写方法执行失败的错误信息
        /// </summary>
        /// <param name="szMethodName">方法名</param>
        /// <param name="szExtraInfo">错误信息</param>
        /// <param name="szExtraInfo">例外信息</param>
        public void WriteLog(string szMethodName, string szErrorInfo, Exception ex)
        {
            this.WriteLog(szMethodName, null, null, szErrorInfo, ex);
        }

        /// <summary>
        /// 写方法执行失败的错误信息
        /// </summary>
        /// <param name="szMethodName">方法名</param>
        /// <param name="arrParamName">输入参数的名称列表</param>
        /// <param name="arrParamValue">输入参数的值列表</param>
        /// <param name="ex">例外信息</param>
        public void WriteLog(string szMethodName, string[] arrParamName, object[] arrParamValue, Exception ex)
        {
            this.WriteLog(szMethodName, arrParamName, arrParamValue, null, ex);
        }

        /// <summary>
        /// 写方法执行失败的错误信息
        /// </summary>
        /// <param name="szMethodName">方法名</param>
        /// <param name="arrParamName">输入参数的名称列表</param>
        /// <param name="arrParamValue">输入参数的值列表</param>
        /// <param name="szErrorInfo">错误信息</param>
        public void WriteLog(string szMethodName, string[] arrParamName, object[] arrParamValue, string szErrorInfo)
        {
            this.WriteLog(szMethodName, arrParamName, arrParamValue, szErrorInfo, null);
        }

        /// <summary>
        /// 写方法执行失败的错误信息
        /// </summary>
        /// <param name="szMethodName">方法名</param>
        /// <param name="arrParamName">输入参数的名称列表</param>
        /// <param name="arrParamValue">输入参数的值列表</param>
        /// <param name="szExtraInfo">其他信息</param>
        /// <param name="ex">例外信息</param>
        public void WriteLog(string szMethodName, string[] arrParamName, object[] arrParamValue, string szExtraInfo, Exception ex)
        {
            StringBuilder sbErrorInfo = new StringBuilder();
            sbErrorInfo.Append("方法");
            sbErrorInfo.Append(szMethodName);
            sbErrorInfo.AppendLine("执行失败!");
            sbErrorInfo.AppendLine("输入参数：");
            if (arrParamName == null || arrParamName.Length <= 0)
            {
                sbErrorInfo.AppendLine("    无");
            }
            else
            {
                for (int index = 0; index < arrParamName.Length; index++)
                {
                    sbErrorInfo.Append("    ");
                    sbErrorInfo.Append(arrParamName[index]);
                    sbErrorInfo.Append("=");
                    if (arrParamValue != null && index < arrParamValue.Length)
                    {
                        if (arrParamValue[index] != null)
                            sbErrorInfo.Append(arrParamValue[index].ToString());
                    }
                    sbErrorInfo.AppendLine(";");
                }
            }
            sbErrorInfo.AppendLine("其他信息：");
            sbErrorInfo.Append("    ");
            sbErrorInfo.Append(szExtraInfo == null ? "无" : szExtraInfo);
            this.WriteLog(sbErrorInfo.ToString(), ex, LogType.Error);
            sbErrorInfo = null;
        }

        /// <summary>
        /// 写入一条日志到Windows日志表
        /// </summary>
        /// <param name="szText">日志基本信息</param>
        /// <param name="ex">.NET例外信息</param>
        /// <param name="eLogType">日志级别</param>
        public void WriteLog(string szText, Exception ex, LogType eLogType)
        {
            StringBuilder sbLogInfo = new StringBuilder();
            sbLogInfo.AppendLine("基本信息：");
            sbLogInfo.Append("    ");
            sbLogInfo.AppendLine(szText);
            sbLogInfo.AppendLine("例外信息：");
            sbLogInfo.Append("    ");
            sbLogInfo.Append((ex == null) ? "无" : ex.ToString());

            if (this.m_bTextLogOnly || !this.InitEventLog())
            {
                this.WriteTextLog(szText, ex, eLogType);
                return;
            }

            EventLogEntryType eWinEventType = EventLogEntryType.Error;
            if (eLogType == LogType.Information)
                eWinEventType = EventLogEntryType.Information;
            else if (eLogType == LogType.Warning)
                eWinEventType = EventLogEntryType.Warning;
            else
                eWinEventType = EventLogEntryType.Error;
            try
            {
                this.m_clsEventLog.WriteEntry(sbLogInfo.ToString(), eWinEventType);
                sbLogInfo = null;
            }
            catch (System.ComponentModel.Win32Exception win32ex)
            {
                //日志已满,则启动备份机制
                if (win32ex.NativeErrorCode == LOG_FULL_ERROR_CODE)
                {
                    //备份日志表
                    bool bSuccess = this.BackupToEvt();
                    if (!bSuccess)
                        bSuccess = this.BackupToTxt();
                    //清空日志表
                    if (bSuccess)
                        bSuccess = this.ClearEventLog();
                    //重写日志
                    if (bSuccess)
                        this.WriteLog(szText, ex, eLogType);
                    else
                        this.WriteTextLog(szText, ex, eLogType);
                }
                else
                {
                    this.WriteTextLog(szText, ex, eLogType);
                }
            }
            catch { this.WriteTextLog(szText, ex, eLogType); }
            finally { this.CloseEventLog(); }
        }

        /// <summary>
        /// 清除Windows系统日志表中MedDoc内容
        /// </summary>
        /// <returns>bool</returns>
        private bool ClearEventLog()
        {
            if (this.m_clsEventLog == null)
                return false;
            try
            {
                this.m_clsEventLog.Clear();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 关闭Windows系统日志表中MedDoc日志
        /// </summary>
        /// <returns>bool</returns>
        private bool CloseEventLog()
        {
            if (this.m_clsEventLog == null)
                return false;
            try
            {
                this.m_clsEventLog.Close();
                this.m_clsEventLog.Dispose();
                this.m_clsEventLog = null;
                return true;
            }
            catch { return false; }
        }
        #endregion

        #region "文本文件日志"
        /// <summary>
        /// 初始化指定的文件，主要检查父目录的存在性
        /// </summary>
        /// <param name="szFilePath">指定的文件</param>
        /// <returns>bool</returns>
        private bool CreateFilePath(string szFilePath)
        {
            try
            {
                FileInfo clsFileInfo = new FileInfo(szFilePath);
                DirectoryInfo clsParentDirInfo = clsFileInfo.Directory;
                if (!clsParentDirInfo.Exists)
                    clsParentDirInfo.Create();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 写日志到文本文件,文本文件自动以yyyyMMdd命名
        /// </summary>
        /// <param name="szLogInfo">日志基本信息</param>
        /// <param name="ex">.NET例外信息</param>
        /// <param name="eLogType">日志级别</param>
        /// <returns>bool</returns>
        private bool WriteTextLog(string szLogInfo, Exception ex, LogType eLogType)
        {
            string szTextLogFile = string.Format(@"{0}\TxtLog\{1}.log"
                , this.LogFilePath, DateTime.Now.ToString("yyyyMMdd"));

            //连创建目录都失败,则取消写日志
            if (!this.CreateFilePath(szTextLogFile))
                return false;

            StreamWriter clsFileWriter = null;
            try
            {
                clsFileWriter = new StreamWriter(szTextLogFile, true);
                StringBuilder sbLogInfo = new StringBuilder();
                sbLogInfo.AppendLine("----------------------------------------------");
                sbLogInfo.Append("时间：");
                sbLogInfo.AppendLine(DateTime.Now.ToString());
                sbLogInfo.Append("类型：");
                sbLogInfo.AppendLine(eLogType.ToString());
                sbLogInfo.Append("基本信息：");
                sbLogInfo.AppendLine(szLogInfo);
                sbLogInfo.Append("例外信息：");
                sbLogInfo.AppendLine((ex == null) ? "null" : ex.ToString());
                clsFileWriter.WriteLine(sbLogInfo.ToString());
                clsFileWriter.WriteLine();
                sbLogInfo = null;
                return true;
            }
            catch { return false; }
            finally
            {
                if (clsFileWriter != null)
                {
                    clsFileWriter.Close();
                    clsFileWriter.Dispose();
                }
            }
        }
        #endregion

        #region "日志自动备份"
        /// <summary>
        /// 将Windows系统日志表中MedDoc内容备份到备份目录(evt格式)
        /// </summary>
        /// <returns>bool</returns>
        private bool BackupToEvt()
        {
            string szBackupFile = string.Format(@"{0}\Backup\{1}.evt"
                , this.LogFilePath
                , DateTime.Now.ToString("yyyyMMddHHmmss"));

            //连创建目录都失败,则取消写日志
            if (!this.CreateFilePath(szBackupFile))
                return false;

            IntPtr hLogHandle = NativeMethods.Advapi32.OpenEventLog(null, this.m_szLogSourceName);
            if (hLogHandle == IntPtr.Zero)
                return false;

            if (File.Exists(szBackupFile))
            {
                try
                {
                    File.Delete(szBackupFile);
                }
                catch
                {
                    NativeMethods.Advapi32.CloseEventLog(hLogHandle);
                    //重新获取文件名备份
                    this.BackupToEvt();
                }
            }
            bool success = NativeMethods.Advapi32.BackupEventLog(hLogHandle, szBackupFile);
            NativeMethods.Advapi32.CloseEventLog(hLogHandle);
            return success;
        }

        /// <summary>
        /// 将Windows系统日志表中MedDoc内容备份到备份目录(txt格式)
        /// </summary>
        /// <returns>bool</returns>
        private bool BackupToTxt()
        {
            if (this.m_clsEventLog == null)
                return false;
            string szBackupFile = string.Format(@"{0}\Backup\{1}.log"
                , this.LogFilePath, DateTime.Now.ToString("yyyyMMddHHmmss"));

            //连创建目录都失败,则取消写日志
            if (!this.CreateFilePath(szBackupFile))
                return false;

            System.Collections.IEnumerator clsLogEnumerator = null;
            EventLogEntryCollection lstLogEntries = this.m_clsEventLog.Entries;
            StreamWriter clsFileWriter = null;
            try
            {
                clsFileWriter = new StreamWriter(szBackupFile, true);
                clsLogEnumerator = lstLogEntries.GetEnumerator();
                StringBuilder sbLogInfo = new StringBuilder();
                while (clsLogEnumerator.MoveNext())
                {
                    EventLogEntry clsLogEntry = (EventLogEntry)clsLogEnumerator.Current;
                    sbLogInfo.Remove(0, sbLogInfo.Length);
                    sbLogInfo.Append("时间：");
                    sbLogInfo.AppendLine(clsLogEntry.TimeGenerated.ToString());
                    sbLogInfo.Append("类型：");
                    sbLogInfo.AppendLine(clsLogEntry.EntryType.ToString());
                    sbLogInfo.AppendLine("日志信息：");
                    sbLogInfo.AppendLine(clsLogEntry.Message.ToString());
                    clsFileWriter.WriteLine(sbLogInfo.ToString());
                    clsFileWriter.WriteLine();
                }
                sbLogInfo = null;
                return true;
            }
            catch { return false; }
            finally
            {
                if (clsLogEnumerator != null && clsLogEnumerator is IDisposable)
                {
                    (clsLogEnumerator as IDisposable).Dispose();
                }
                if (clsFileWriter != null)
                {
                    clsFileWriter.Close();
                    clsFileWriter.Dispose();
                }
                this.CloseEventLog();
            }
        }
        #endregion

        /// <summary>
        /// 得到MedDoc动态库运行目录
        /// </summary>
        /// <returns>string</returns>
        private string GetLogFilePath()
        {
            try
            {
                string szDllPath = typeof(LogManager).Assembly.Location;
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(szDllPath);
                return string.Concat(fileInfo.DirectoryName, "\\Logs");
            }
            catch { return "C:\\Logs"; }
        }

        /// <summary>
        /// 清理过期的文本日志文件
        /// </summary>
        /// <param name="szLogDir">日志目录</param>
        /// <param name="szFilemask">文件通配符</param>
        /// <param name="nOverdueDays">过期天数</param>
        private void CleanOverdueLog(string szLogDir, string szFilemask, int nOverdueDays)
        {
            if (nOverdueDays <= 0 || nOverdueDays > 365)
                return;
            List<string> lstLogFiles = GlobalMethods.IO.SearchDirectory(szLogDir, szFilemask);
            if (lstLogFiles == null || lstLogFiles.Count <= 0)
                return;

            DateTime dtOverdueDate = DateTime.Now.AddDays(0 - nOverdueDays);
            for (int index = 0; index < lstLogFiles.Count; index++)
            {
                DateTime dtFileModifyTime = DateTime.Now;
                GlobalMethods.IO.GetFileLastModifyTime(lstLogFiles[index], ref dtFileModifyTime);
                if (DateTime.Compare(dtFileModifyTime, dtOverdueDate) <= 0)
                    GlobalMethods.IO.DeleteFile(lstLogFiles[index]);
            }
        }

        /// <summary>
        /// 释放拥有的资源,同时清理过期日志
        /// </summary>
        public void Dispose()
        {
            if (this.m_clsEventLog != null)
                this.CloseEventLog();
            this.m_clsEventLog = null;

            if (this.m_bAutoCleanLog)
            {
                this.CleanOverdueLog(string.Format(@"{0}\Backup", this.LogFilePath), "*.evt", this.m_nAutoCleanPeriod);
                this.CleanOverdueLog(string.Format(@"{0}\TxtLog", this.LogFilePath), "*.log", this.m_nAutoCleanPeriod);
            }
        }
    }
}
