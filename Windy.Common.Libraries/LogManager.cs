// ***********************************************************
// ҽ���ĵ�ϵͳ��־������.����д������־��Windows��־��
// �����ı��ļ�
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

        #region "��ʵ��ģʽ"
        private static LogManager m_Instance = null;
        /// <summary>
        /// ��ʵ��
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
        /// ��ȡ�������Ƿ��д�ı��ļ���־
        /// </summary>
        public bool TextLogOnly
        {
            get { return this.m_bTextLogOnly; }
            set { this.m_bTextLogOnly = value; }
        }

        /// <summary>
        /// ��ȡ�������Ƿ��Զ�������־�ļ�
        /// </summary>
        public bool AutoCleanLog
        {
            get { return this.m_bAutoCleanLog; }
            set { this.m_bAutoCleanLog = value; }
        }

        /// <summary>
        /// ��ȡ��������־�Զ���������
        /// </summary>
        public int AutoCleanPeriod
        {
            get { return this.m_nAutoCleanPeriod; }
            set { this.m_nAutoCleanPeriod = value; }
        }

        /// <summary>
        /// ��ȡ��������־����
        /// </summary>
        public string LogSourceName
        {
            get { return this.m_szLogSourceName; }
            set { this.m_szLogSourceName = value; }
        }

        /// <summary>
        /// ��ȡ��������־����·��
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

        #region "Windows��־"
        /// <summary>
        /// ��ʼ��Windows�¼���־����
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
                //�����Ǿ���־
                if (this.m_clsEventLog.OverflowAction != OverflowAction.DoNotOverwrite)
                    this.m_clsEventLog.ModifyOverflowPolicy(OverflowAction.DoNotOverwrite, -1);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// д��һ����־��Windows��־��
        /// </summary>
        /// <param name="szText">��־������Ϣ</param>
        public void WriteLog(string szText)
        {
            this.WriteLog(szText, null, LogType.Error);
        }

        /// <summary>
        /// д��һ����־��Windows��־��
        /// </summary>
        /// <param name="szText">��־������Ϣ</param>
        /// <param name="ex">.NET������Ϣ</param>
        public void WriteLog(string szText, Exception ex)
        {
            this.WriteLog(szText, ex, LogType.Error);
        }

        /// <summary>
        /// д����ִ��ʧ�ܵĴ�����Ϣ
        /// </summary>
        /// <param name="szMethodName">������</param>
        /// <param name="szExtraInfo">������Ϣ</param>
        public void WriteLog(string szMethodName, string szErrorInfo)
        {
            this.WriteLog(szMethodName, null, null, szErrorInfo, null);
        }

        /// <summary>
        /// д����ִ��ʧ�ܵĴ�����Ϣ
        /// </summary>
        /// <param name="szMethodName">������</param>
        /// <param name="szExtraInfo">������Ϣ</param>
        /// <param name="szExtraInfo">������Ϣ</param>
        public void WriteLog(string szMethodName, string szErrorInfo, Exception ex)
        {
            this.WriteLog(szMethodName, null, null, szErrorInfo, ex);
        }

        /// <summary>
        /// д����ִ��ʧ�ܵĴ�����Ϣ
        /// </summary>
        /// <param name="szMethodName">������</param>
        /// <param name="arrParamName">��������������б�</param>
        /// <param name="arrParamValue">���������ֵ�б�</param>
        /// <param name="ex">������Ϣ</param>
        public void WriteLog(string szMethodName, string[] arrParamName, object[] arrParamValue, Exception ex)
        {
            this.WriteLog(szMethodName, arrParamName, arrParamValue, null, ex);
        }

        /// <summary>
        /// д����ִ��ʧ�ܵĴ�����Ϣ
        /// </summary>
        /// <param name="szMethodName">������</param>
        /// <param name="arrParamName">��������������б�</param>
        /// <param name="arrParamValue">���������ֵ�б�</param>
        /// <param name="szErrorInfo">������Ϣ</param>
        public void WriteLog(string szMethodName, string[] arrParamName, object[] arrParamValue, string szErrorInfo)
        {
            this.WriteLog(szMethodName, arrParamName, arrParamValue, szErrorInfo, null);
        }

        /// <summary>
        /// д����ִ��ʧ�ܵĴ�����Ϣ
        /// </summary>
        /// <param name="szMethodName">������</param>
        /// <param name="arrParamName">��������������б�</param>
        /// <param name="arrParamValue">���������ֵ�б�</param>
        /// <param name="szExtraInfo">������Ϣ</param>
        /// <param name="ex">������Ϣ</param>
        public void WriteLog(string szMethodName, string[] arrParamName, object[] arrParamValue, string szExtraInfo, Exception ex)
        {
            StringBuilder sbErrorInfo = new StringBuilder();
            sbErrorInfo.Append("����");
            sbErrorInfo.Append(szMethodName);
            sbErrorInfo.AppendLine("ִ��ʧ��!");
            sbErrorInfo.AppendLine("���������");
            if (arrParamName == null || arrParamName.Length <= 0)
            {
                sbErrorInfo.AppendLine("    ��");
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
            sbErrorInfo.AppendLine("������Ϣ��");
            sbErrorInfo.Append("    ");
            sbErrorInfo.Append(szExtraInfo == null ? "��" : szExtraInfo);
            this.WriteLog(sbErrorInfo.ToString(), ex, LogType.Error);
            sbErrorInfo = null;
        }

        /// <summary>
        /// д��һ����־��Windows��־��
        /// </summary>
        /// <param name="szText">��־������Ϣ</param>
        /// <param name="ex">.NET������Ϣ</param>
        /// <param name="eLogType">��־����</param>
        public void WriteLog(string szText, Exception ex, LogType eLogType)
        {
            StringBuilder sbLogInfo = new StringBuilder();
            sbLogInfo.AppendLine("������Ϣ��");
            sbLogInfo.Append("    ");
            sbLogInfo.AppendLine(szText);
            sbLogInfo.AppendLine("������Ϣ��");
            sbLogInfo.Append("    ");
            sbLogInfo.Append((ex == null) ? "��" : ex.ToString());

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
                //��־����,���������ݻ���
                if (win32ex.NativeErrorCode == LOG_FULL_ERROR_CODE)
                {
                    //������־��
                    bool bSuccess = this.BackupToEvt();
                    if (!bSuccess)
                        bSuccess = this.BackupToTxt();
                    //�����־��
                    if (bSuccess)
                        bSuccess = this.ClearEventLog();
                    //��д��־
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
        /// ���Windowsϵͳ��־����MedDoc����
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
        /// �ر�Windowsϵͳ��־����MedDoc��־
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

        #region "�ı��ļ���־"
        /// <summary>
        /// ��ʼ��ָ�����ļ�����Ҫ��鸸Ŀ¼�Ĵ�����
        /// </summary>
        /// <param name="szFilePath">ָ�����ļ�</param>
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
        /// д��־���ı��ļ�,�ı��ļ��Զ���yyyyMMdd����
        /// </summary>
        /// <param name="szLogInfo">��־������Ϣ</param>
        /// <param name="ex">.NET������Ϣ</param>
        /// <param name="eLogType">��־����</param>
        /// <returns>bool</returns>
        private bool WriteTextLog(string szLogInfo, Exception ex, LogType eLogType)
        {
            string szTextLogFile = string.Format(@"{0}\TxtLog\{1}.log"
                , this.LogFilePath, DateTime.Now.ToString("yyyyMMdd"));

            //������Ŀ¼��ʧ��,��ȡ��д��־
            if (!this.CreateFilePath(szTextLogFile))
                return false;

            StreamWriter clsFileWriter = null;
            try
            {
                clsFileWriter = new StreamWriter(szTextLogFile, true);
                StringBuilder sbLogInfo = new StringBuilder();
                sbLogInfo.AppendLine("----------------------------------------------");
                sbLogInfo.Append("ʱ�䣺");
                sbLogInfo.AppendLine(DateTime.Now.ToString());
                sbLogInfo.Append("���ͣ�");
                sbLogInfo.AppendLine(eLogType.ToString());
                sbLogInfo.Append("������Ϣ��");
                sbLogInfo.AppendLine(szLogInfo);
                sbLogInfo.Append("������Ϣ��");
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

        #region "��־�Զ�����"
        /// <summary>
        /// ��Windowsϵͳ��־����MedDoc���ݱ��ݵ�����Ŀ¼(evt��ʽ)
        /// </summary>
        /// <returns>bool</returns>
        private bool BackupToEvt()
        {
            string szBackupFile = string.Format(@"{0}\Backup\{1}.evt"
                , this.LogFilePath
                , DateTime.Now.ToString("yyyyMMddHHmmss"));

            //������Ŀ¼��ʧ��,��ȡ��д��־
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
                    //���»�ȡ�ļ�������
                    this.BackupToEvt();
                }
            }
            bool success = NativeMethods.Advapi32.BackupEventLog(hLogHandle, szBackupFile);
            NativeMethods.Advapi32.CloseEventLog(hLogHandle);
            return success;
        }

        /// <summary>
        /// ��Windowsϵͳ��־����MedDoc���ݱ��ݵ�����Ŀ¼(txt��ʽ)
        /// </summary>
        /// <returns>bool</returns>
        private bool BackupToTxt()
        {
            if (this.m_clsEventLog == null)
                return false;
            string szBackupFile = string.Format(@"{0}\Backup\{1}.log"
                , this.LogFilePath, DateTime.Now.ToString("yyyyMMddHHmmss"));

            //������Ŀ¼��ʧ��,��ȡ��д��־
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
                    sbLogInfo.Append("ʱ�䣺");
                    sbLogInfo.AppendLine(clsLogEntry.TimeGenerated.ToString());
                    sbLogInfo.Append("���ͣ�");
                    sbLogInfo.AppendLine(clsLogEntry.EntryType.ToString());
                    sbLogInfo.AppendLine("��־��Ϣ��");
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
        /// �õ�MedDoc��̬������Ŀ¼
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
        /// ������ڵ��ı���־�ļ�
        /// </summary>
        /// <param name="szLogDir">��־Ŀ¼</param>
        /// <param name="szFilemask">�ļ�ͨ���</param>
        /// <param name="nOverdueDays">��������</param>
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
        /// �ͷ�ӵ�е���Դ,ͬʱ���������־
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
