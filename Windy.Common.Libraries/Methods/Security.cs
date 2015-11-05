// ***********************************************************
// ��װһЩϵͳ��ȫ��ط�������
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.IO;

namespace Windy.Common.Libraries
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// ��װϵͳ��ȫ��ط���
        /// </summary>
        public struct Security
        {
            #region"���ܽ��ܷ���"
            /// <summary>
            /// ����һ���ı���ժҪ��Ϣ
            /// </summary>
            /// <param name="szOriginalText">ԭʼ�ı�</param>
            /// <returns>ժҪ��Ϣ�ı�</returns>
            public static string GetSummaryMD5(string szOriginalText)
            {
                try
                {
                    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(szOriginalText, "MD5");
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetSummaryMD5", ex);
                    return null;
                }
            }

            /// <summary>
            /// ����һ���ı���ժҪ��Ϣ
            /// </summary>
            /// <param name="szOriginalText">ԭʼ�ı�</param>
            /// <returns>ժҪ��Ϣ�ı�</returns>
            public static string GetSummarySHA1(string szOriginalText)
            {
                try
                {
                    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(szOriginalText, "SHA1");
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetSummarySHA1", ex);
                    return null;
                }
            }

            /// <summary>
            /// ����ָ�����ֽ�����
            /// </summary>
            /// <param name="originalData">ԭʼ�ֽ�����</param>
            /// <param name="keyData">������Կ</param>
            /// <param name="ivData"></param>
            /// <returns>���ܺ������</returns>
            private static byte[] Encrypt(byte[] originalData, byte[] keyData, byte[] ivData)
            {
                MemoryStream memoryStream = new MemoryStream();
                //����Rijndael�����㷨
                System.Security.Cryptography.Rijndael rijndael = System.Security.Cryptography.Rijndael.Create();
                rijndael.Key = keyData;
                rijndael.IV = ivData;

                System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(
                    memoryStream, rijndael.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
                try
                {
                    cryptoStream.Write(originalData, 0, originalData.Length);
                    cryptoStream.Close();
                    cryptoStream.Dispose();
                    return memoryStream.ToArray();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Encrypt", ex);
                    return null;
                }
                finally
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }
            }

            /// <summary>
            /// ����ָ�����ֽ�����
            /// </summary>
            /// <param name="originalData">���ܵ��ֽ�����</param>
            /// <param name="keyData">������Կ</param>
            /// <param name="ivData"></param>
            /// <returns>ԭʼ�ı�</returns>
            private static byte[] Decrypt(byte[] encryptedData, byte[] keyData, byte[] ivData)
            {
                MemoryStream memoryStream = new MemoryStream();
                //����Rijndael�����㷨
                System.Security.Cryptography.Rijndael rijndael = System.Security.Cryptography.Rijndael.Create();
                rijndael.Key = keyData;
                rijndael.IV = ivData;

                System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(
                    memoryStream, rijndael.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
                try
                {
                    cryptoStream.Write(encryptedData, 0, encryptedData.Length);
                    cryptoStream.Close();
                    cryptoStream.Dispose();
                    return memoryStream.ToArray();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.Decrypt", ex);
                    return null;
                }
                finally
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }
            }

            /// <summary>
            /// ����һ���ı�
            /// </summary>
            /// <param name="szOriginalText">ԭʼ�ı�</param>
            /// <param name="szKey">������Կ</param>
            /// <returns>���ܺ������</returns>
            public static string EncryptText(string szOriginalText, string szKey)
            {
                try
                {
                    byte[] originalData = System.Text.Encoding.Unicode.GetBytes(szOriginalText);

                    System.Security.Cryptography.PasswordDeriveBytes pdb = new System.Security.Cryptography.PasswordDeriveBytes(szKey
                        , new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                    byte[] encryptedData = GlobalMethods.Security.Encrypt(originalData, pdb.GetBytes(32), pdb.GetBytes(16));

                    return System.Convert.ToBase64String(encryptedData);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.EncryptText", ex);
                    return null;
                }
            }

            /// <summary>
            /// ����һ������
            /// </summary>
            /// <param name="szOriginalText">�����ı�</param>
            /// <param name="szKey">������Կ</param>
            /// <returns>ԭʼ�ı�</returns>
            public static string DecryptText(string szEncryptedText, string szKey)
            {
                try
                {
                    byte[] encryptedData = System.Convert.FromBase64String(szEncryptedText);

                    System.Security.Cryptography.PasswordDeriveBytes pdb = new System.Security.Cryptography.PasswordDeriveBytes(szKey
                        , new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                    byte[] originalData = GlobalMethods.Security.Decrypt(encryptedData, pdb.GetBytes(32), pdb.GetBytes(16));

                    return System.Text.Encoding.Unicode.GetString(originalData);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.DecryptText", ex);
                    return null;
                }
            }
            #endregion
        }
    }
}
