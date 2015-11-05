// ***********************************************************
// ��װһЩ����IO���ʷ�������
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Windy.Common.Libraries
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// ��װ����IO���ʷ���
        /// </summary>
        public struct IO
        {
            /// <summary>
            /// ����ָ���ı���Ŀ¼
            /// </summary>
            /// <param name="dirPath">Ŀ¼·��</param>
            /// <returns>true:�����ɹ�;false:����ʧ��</returns>
            public static bool CreateDirectory(string dirPath)
            {
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                    if (dirInfo.Exists)
                        return true;
                    Directory.CreateDirectory(dirPath);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.CreateDirectory"
                        , new string[] { "dirPath" }, new string[] { dirPath }, "����ָ���ı���Ŀ¼ʧ��!", ex);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// ɾ��ָ���ı���Ŀ¼
            /// </summary>
            /// <param name="dirPath">Ŀ¼·��</param>
            /// <param name="recursive">�Ƿ�ݹ���Ŀ¼</param>
            /// <returns>true:ɾ���ɹ�;false:ɾ��ʧ��</returns>
            public static bool DeleteDirectory(string dirPath, bool recursive)
            {
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                    if (!dirInfo.Exists)
                        return true;
                    Directory.Delete(dirPath, recursive);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.DeleteDirectory"
                        , new string[] { "dirPath" }, new string[] { dirPath }, "ɾ��ָ���ı���Ŀ¼ʧ��!", ex);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// ����ָ��Ŀ¼�µ�������Ŀ¼����(���ݹ�)
            /// </summary>
            /// <param name="rootPath">ָ����Ŀ¼</param>
            /// <returns>���ҵ���Ŀ¼����</returns>
            public static DirectoryInfo[] GetDirectories(string dirPath)
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(dirPath);
                    if (!directory.Exists)
                        return new DirectoryInfo[0];
                    return directory.GetDirectories();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetDirectories"
                        , new string[] { "dirPath" }, new string[] { dirPath }, "��ȡָ��Ŀ¼�µ�������Ŀ¼����ʧ��!", ex);
                    return new DirectoryInfo[0];
                }
            }

            /// <summary>
            /// ����ָ��Ŀ¼�µ������ļ�����(���ݹ�)
            /// </summary>
            /// <param name="directory">ָ����Ŀ¼</param>
            /// <returns>���ҵ����ļ�����</returns>
            public static FileInfo[] GetFiles(string dirPath)
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(dirPath);
                    if (!directory.Exists)
                        return new FileInfo[0];
                    return directory.GetFiles();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetFiles"
                        , new string[] { "dirPath" }, new string[] { dirPath }, "��ȡָ��Ŀ¼�µ������ļ�����ʧ��!", ex);
                    return new FileInfo[0];
                }
            }

            /// <summary>
            /// ��ָ����Ŀ¼�²��Ұ���ָ��ͨ����������ļ�
            /// </summary>
            /// <param name="directory">ָ����Ŀ¼</param>
            /// <param name="filemask">ͨ���</param>
            /// <returns>���ҵ����ļ�����</returns>
            public static List<string> SearchDirectory(string directory, string filemask)
            {
                return SearchDirectory(directory, filemask, true, false);
            }

            /// <summary>
            /// ��ָ����Ŀ¼�²��Ұ���ָ��ͨ����������ļ�
            /// </summary>
            /// <param name="directory">ָ����Ŀ¼</param>
            /// <param name="filemask">ͨ���</param>
            /// <param name="recursive">�Ƿ�ݹ�</param>
            /// <returns>���ҵ����ļ�����</returns>
            public static List<string> SearchDirectory(string directory, string filemask
                , bool recursive)
            {
                return SearchDirectory(directory, filemask, recursive, false);
            }

            /// <summary>
            /// ��ָ����Ŀ¼�²��Ұ���ָ��ͨ����������ļ�
            /// </summary>
            /// <param name="directory">ָ����Ŀ¼</param>
            /// <param name="filemask">ͨ���</param>
            /// <param name="recursive">�Ƿ�ݹ�</param>
            /// <param name="ignoreHidden">�Ƿ���������ļ�</param>
            /// <returns>���ҵ����ļ�����</returns>
            public static List<string> SearchDirectory(string directory, string filemask
                , bool recursive, bool ignoreHidden)
            {
                return SearchDirectory(directory, filemask, recursive, ignoreHidden, false);
            }

            /// <summary>
            /// ��ָ����Ŀ¼�²��Ұ���ָ��ͨ����������ļ�
            /// </summary>
            /// <param name="directory">ָ����Ŀ¼</param>
            /// <param name="filemask">ͨ���</param>
            /// <param name="recursive">�Ƿ�ݹ�</param>
            /// <param name="ignoreHidden">�Ƿ���������ļ�</param>
            /// <param name="returnNameOnly">�Ƿ���������ļ�</param>
            /// <returns>���ҵ����ļ�����</returns>
            public static List<string> SearchDirectory(string directory, string filemask
                , bool recursive, bool ignoreHidden, bool returnNameOnly)
            {
                List<string> files = new List<string>();
                SearchDirectory(directory, filemask, recursive, ignoreHidden, returnNameOnly, files);
                return files;
            }

            /// <summary>
            /// ��ָ����Ŀ¼�²��Ұ���ָ��ͨ����������ļ�
            /// </summary>
            /// <param name="directory">ָ����Ŀ¼</param>
            /// <param name="filemask">ͨ���</param>
            /// <param name="recursive">�Ƿ�ݹ�</param>
            /// <param name="ignoreHidden">�Ƿ���������ļ�</param>
            /// <param name="returnNameOnly">�Ƿ���������ļ�</param>
            /// <param name="files">���ҵ����ļ�����</param>
            private static void SearchDirectory(string directory, string filemask, bool recursive
                , bool ignoreHidden, bool returnNameOnly, List<string> files)
            {
                if (files == null) files = new List<string>();
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(directory);
                    if (!dirInfo.Exists)
                        return;
                    FileInfo[] fileInfos = dirInfo.GetFiles(filemask);
                    if (fileInfos == null)
                        return;

                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        if (ignoreHidden &&
                            (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                        {
                            continue;
                        }
                        if (returnNameOnly)
                            files.Add(fileInfo.Name);
                        else
                            files.Add(fileInfo.FullName);
                    }

                    if (!recursive)
                        return;

                    DirectoryInfo[] arrCurrDirs = dirInfo.GetDirectories();
                    foreach (DirectoryInfo currDir in arrCurrDirs)
                    {
                        if (ignoreHidden &&
                            (currDir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                        {
                            continue;
                        }
                        SearchDirectory(currDir.FullName, filemask, recursive, ignoreHidden, returnNameOnly, files);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.SearchDirectory"
                        , new string[] { "directory", "filemask", "recursive", "ignoreHidden", "returnNameOnly" }
                        , new object[] { directory, filemask, recursive, ignoreHidden, returnNameOnly }, ex);
                }
            }

            /// <summary>
            /// ��ָ�����ļ�������ָ����Ŀ¼��,������
            /// </summary>
            /// <param name="szSouFile">Դ�ļ�ȫ·��</param>
            /// <param name="szDestFile">Ŀ���ļ�ȫ·��</param>
            /// <returns>true:�����ɹ�;false:����ʧ��</returns>
            public static bool CopyFile(string szSouFile, string szDestFile)
            {
                try
                {
                    if (!File.Exists(szSouFile))
                        return false;
                    File.Copy(szSouFile, szDestFile, true);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.CopyFile", new string[] { "szSouFile", "szDestFile" }
                        , new string[] { szSouFile, szDestFile }, "�ļ�������Ŀ��Ŀ¼ʧ��!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ����ָ��Ŀ¼�µ��ļ�
            /// </summary>
            /// <param name="fileName">�ļ�ȫ·��</param>
            /// <returns>true:�����ɹ�;false:����ʧ��</returns>
            /// <remarks>ע�⣺���Զ����������ڵ�Ŀ¼</remarks>
            public static bool CreateFile(string fileName)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(fileName);
                    if (fileInfo.Exists)
                    {
                        if (fileInfo.Attributes != FileAttributes.Normal)
                            fileInfo.Attributes = FileAttributes.Normal;
                        return true;
                    }
                    //����Ŀ¼
                    bool success = CreateDirectory(fileInfo.DirectoryName);
                    if (!success)
                        return false;
                    //�����ļ�
                    FileStream fileStream = fileInfo.Create();
                    fileStream.Close();
                    fileStream.Dispose();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.CreateFile"
                        , new string[] { "fileName" }, new string[] { fileName }, "����ָ��Ŀ¼�µ��ļ�ʧ��!", ex);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// ���ı�����д��ָ��Ŀ¼�µ��ļ�(gb2312����).
            /// </summary>
            /// <param name="szFilePath">�ļ�ȫ·��</param>
            /// <param name="szTextData">�ı�����</param>
            /// <returns>true:д��ɹ�;false:д��ʧ��</returns>
            /// <remarks>ע�⣺���Զ����������ڵ�Ŀ¼</remarks>
            public static bool WriteFileText(string szFilePath, string szTextData)
            {
                return WriteFileText(szFilePath, szTextData, Convert.GetDefaultEncoding());
            }

            /// <summary>
            /// ���ı�����д��ָ��Ŀ¼�µ��ļ�
            /// </summary>
            /// <param name="szFilePath">�ļ�ȫ·��</param>
            /// <param name="szTextData">�ı�����</param>
            /// <param name="encoding">�ַ�����</param>
            /// <returns>true:д��ɹ�;false:д��ʧ��</returns>
            /// <remarks>ע�⣺���Զ����������ڵ�Ŀ¼</remarks>
            public static bool WriteFileText(string szFilePath, string szTextData, Encoding encoding)
            {
                if (!CreateDirectory(GetFilePath(szFilePath)))
                    return false;
                try
                {
                    File.WriteAllText(szFilePath, szTextData, encoding);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.WriteFileText"
                        , new string[] { "szFilePath" }, new string[] { szFilePath }, "�ı�����д���ļ�ʧ��!", ex);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// ���ı�����д��ָ��Ŀ¼�µ��ļ�
            /// </summary>
            /// <param name="szFilePath">�ļ�ȫ·��</param>
            /// <param name="arrTextLines">�ı�����</param>
            /// <param name="encoding">�ַ�����</param>
            /// <returns>true:д��ɹ�;false:д��ʧ��</returns>
            /// <remarks>ע�⣺���Զ����������ڵ�Ŀ¼</remarks>
            public static bool WriteFileText(string szFilePath, string[] arrTextLines, Encoding encoding)
            {
                if (!CreateDirectory(GetFilePath(szFilePath)))
                    return false;
                try
                {
                    File.WriteAllLines(szFilePath, arrTextLines, encoding);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.WriteFileText"
                        , new string[] { "szFilePath" }, new string[] { szFilePath }, "�ı�����д���ļ�ʧ��!", ex);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// ��ȡָ�����ı��ļ�����(gb2312����)
            /// </summary>
            /// <param name="szFilePath">�ļ�ȫ·��</param>
            /// <param name="szTextData">�ļ�����</param>
            /// <returns>true:д��ɹ�;false:д��ʧ��</returns>
            public static bool GetFileText(string szFilePath, ref string szTextData)
            {
                return GetFileText(szFilePath, Convert.GetDefaultEncoding(), ref szTextData);
            }

            /// <summary>
            /// ��ȡָ�����ı��ļ�����
            /// </summary>
            /// <param name="szFilePath">�ļ�ȫ·��</param>
            /// <param name="encoding">����</param>
            /// <param name="szTextData">�ļ�����</param>
            /// <returns>true:д��ɹ�;false:д��ʧ��</returns>
            public static bool GetFileText(string szFilePath, Encoding encoding, ref string szTextData)
            {
                try
                {
                    szTextData = File.ReadAllText(szFilePath, encoding);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetFileText"
                        , new string[] { "szFilePath" }, new string[] { szFilePath }, "�ı��ļ����ݶ�ȡʧ��!", ex);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// ɾ��ָ��Ŀ¼�µ��ļ�
            /// </summary>
            /// <param name="fileName">�ļ�ȫ·��</param>
            /// <returns>true:ɾ���ɹ�;false:ɾ��ʧ��</returns>
            public static bool DeleteFile(string fileName)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(fileName);
                    if (!fileInfo.Exists)
                        return true;
                    if (fileInfo.Attributes != FileAttributes.Normal)
                        fileInfo.Attributes = FileAttributes.Normal;
                    fileInfo.Delete();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.DeleteFile"
                        , new string[] { "fileName" }, new string[] { fileName }, ex);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// ɾ��ָ��Ŀ¼�µ��ļ�
            /// </summary>
            /// <param name="fileName">�ļ�ȫ·��</param>
            /// <returns>true:ɾ���ɹ�;false:ɾ��ʧ��</returns>
            public static bool BackupFile(string fileName, string newFileName)
            {
                try
                {
                    System.IO.File.Copy(fileName, newFileName);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.BackupFile"
                        , new string[] { "fileName", "newFileName" }, new string[] { fileName, newFileName }, ex);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// �Ƚ������ļ��ĳ��ȴ�С�Ƿ����
            /// </summary>
            /// <param name="szFileName1">�ļ�ȫ·��1</param>
            /// <param name="szFileName2">�ļ�ȫ·��2</param>
            /// <returns>true:���;false:�����</returns>
            public static bool IsFileLengthEqual(string szFileName1, string szFileName2)
            {
                try
                {
                    System.IO.FileInfo fileInfo1 = new System.IO.FileInfo(szFileName1);
                    System.IO.FileInfo fileInfo2 = new System.IO.FileInfo(szFileName2);
                    if (!fileInfo1.Exists || !fileInfo2.Exists)
                        return false;
                    return (fileInfo1.Length == fileInfo2.Length);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.CompareFileLength"
                        , new string[] { "szFileName1", "szFileName2" }, new string[] { szFileName1, szFileName2 }, ex);
                    return false;
                }
            }

            /// <summary>
            /// ���ļ�ȫ·����ȡ�ļ�����
            /// </summary>
            /// <param name="szFilePath">�ļ�ȫ·��</param>
            /// <param name="bHasExt">�Ƿ������չ��</param>
            /// <returns>�ļ�����</returns>
            public static string GetFileName(string szFilePath, bool bHasExt)
            {
                if (szFilePath == null)
                    return string.Empty;
                string szFileName = null;
                int index = szFilePath.LastIndexOf(Path.DirectorySeparatorChar);
                if (index < 0)
                    index = szFilePath.LastIndexOf(Path.AltDirectorySeparatorChar);
                if (index < 0)
                    szFileName = szFilePath;
                else
                    szFileName = szFilePath.Substring(++index);
                if (!bHasExt && !string.IsNullOrEmpty(szFileName))
                {
                    index = szFileName.LastIndexOf(".");
                    if (index > 0)
                        szFileName = szFileName.Substring(0, index);
                }
                return szFileName;
            }

            /// <summary>
            /// ���ļ�ȫ·����ȡ�ļ���·��
            /// </summary>
            /// <param name="szFileFullPath">�ļ�ȫ·��</param>
            /// <returns>�ļ���·��</returns>
            public static string GetFilePath(string szFileFullPath)
            {
                if (string.IsNullOrEmpty(szFileFullPath))
                    return "";
                int index = szFileFullPath.LastIndexOf(Path.DirectorySeparatorChar);
                if (index < 0)
                    index = szFileFullPath.LastIndexOf(Path.AltDirectorySeparatorChar);
                return (index < 0) ? szFileFullPath : szFileFullPath.Substring(0, index);
            }

            /// <summary>
            /// ��ȡָ���ļ����ϴ��޸�ʱ��
            /// </summary>
            /// <param name="fileName">�ļ�ȫ·��</param>
            /// <returns>true:��ȡ�ɹ�;false:��ȡʧ��</returns>
            public static bool GetFileLastModifyTime(string fileName, ref DateTime dtLastModifyTime)
            {
                try
                {
                    System.IO.FileInfo fileInfo = new FileInfo(fileName);
                    if (!fileInfo.Exists)
                        return false;
                    dtLastModifyTime = fileInfo.LastWriteTime;
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetFileLastModifyTime"
                        , new string[] { "fileName" }, new string[] { fileName }, ex);
                    return false;
                }
            }

            /// <summary>
            /// �����ݶ�ȡ����ָ���ֶζ�ȡ��Ӧ���ֽ�����
            /// </summary>
            /// <param name="reader">���ݶ�ȡ��</param>
            /// <param name="column">ָ���ֶ�</param>
            /// <param name="byteData">�ֽ�����</param>
            /// <returns>short</returns>
            public static bool GetBytes(System.Data.IDataReader reader, int column, ref byte[] byteData)
            {
                MemoryStream memoryStream = null;
                BinaryWriter binaryWriter = null;
                try
                {
                    if (reader.IsDBNull(column))
                    {
                        byteData = new byte[0];
                        return true;
                    }

                    memoryStream = new MemoryStream();
                    binaryWriter = new BinaryWriter(memoryStream);

                    int nStartIndex = 0;
                    int nBufferSize = 2048;
                    byte[] byteBuffer = new byte[nBufferSize];
                    long nRetLen = reader.GetBytes(column, 0, byteBuffer, 0, nBufferSize);
                    while (nRetLen == nBufferSize)
                    {
                        binaryWriter.Write(byteBuffer);
                        binaryWriter.Flush();

                        nStartIndex += nBufferSize;
                        nRetLen = reader.GetBytes(column, nStartIndex, byteBuffer, 0, nBufferSize);
                    }
                    binaryWriter.Write(byteBuffer, 0, (int)nRetLen);
                    binaryWriter.Flush();
                    byteBuffer = null;

                    byteData = memoryStream.ToArray();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (binaryWriter != null)
                    {
                        binaryWriter.Close();
                    }
                    if (memoryStream != null)
                    {
                        memoryStream.Close();
                        memoryStream.Dispose();
                    }
                }
            }

            /// <summary>
            /// ���ر����ļ����ֽ�������Ϣ
            /// </summary>
            /// <param name="szFileFullPath">�����ļ�ȫ·��</param>
            /// <param name="byteFileData">�ļ����ֽ�����</param>
            /// <returns>bool</returns>
            public static bool GetFileBytes(string szFileFullPath, ref byte[] byteFileData)
            {
                FileInfo fileInfo = null;
                try
                {
                    fileInfo = new FileInfo(szFileFullPath);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetFileBytes", new string[] { "szFileFullPath" }
                        , new object[] { szFileFullPath }, "�޷������ļ���Ϣ����!", ex);
                    return false;
                }

                if (!fileInfo.Exists)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetFileBytes", new string[] { "szFileFullPath" }
                        , new object[] { szFileFullPath }, "�ļ�δ�ҵ�!");
                    return false;
                }

                try
                {
                    byteFileData = File.ReadAllBytes(szFileFullPath);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetFileBytes", new string[] { "szFileFullPath" }
                        , new object[] { szFileFullPath }, "�޷���ȡ�ļ�!", ex);
                    return false;
                }
            }

            /// <summary>
            /// ���ֽ�����д��ָ���ļ�
            /// </summary>
            /// <param name="szFileFullPath">�����ļ�ȫ·��</param>
            /// <param name="byteFileData">�ļ��ֽ�����</param>
            /// <returns>bool</returns>
            /// <remarks>ע�⣺���Զ����������ڵ�Ŀ¼</remarks>
            public static bool WriteFileBytes(string szFileFullPath, byte[] byteFileData)
            {
                if (byteFileData == null)
                    byteFileData = new byte[0];

                if (!CreateDirectory(GetFilePath(szFileFullPath)))
                    return false;
                try
                {
                    File.WriteAllBytes(szFileFullPath, byteFileData);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.WriteFileBytes", new string[] { "szFileFullPath" }
                        , new object[] { szFileFullPath }, "�޷�д���ļ�����!", ex);
                    return false;
                }
            }
        }
    }
}
