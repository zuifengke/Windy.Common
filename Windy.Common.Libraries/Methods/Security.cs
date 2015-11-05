// ***********************************************************
// 封装一些系统安全相关方法集合
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
        /// 封装系统安全相关方法
        /// </summary>
        public struct Security
        {
            #region"加密解密方法"
            /// <summary>
            /// 计算一段文本的摘要信息
            /// </summary>
            /// <param name="szOriginalText">原始文本</param>
            /// <returns>摘要信息文本</returns>
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
            /// 计算一段文本的摘要信息
            /// </summary>
            /// <param name="szOriginalText">原始文本</param>
            /// <returns>摘要信息文本</returns>
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
            /// 加密指定的字节数据
            /// </summary>
            /// <param name="originalData">原始字节数据</param>
            /// <param name="keyData">加密密钥</param>
            /// <param name="ivData"></param>
            /// <returns>加密后的密文</returns>
            private static byte[] Encrypt(byte[] originalData, byte[] keyData, byte[] ivData)
            {
                MemoryStream memoryStream = new MemoryStream();
                //创建Rijndael加密算法
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
            /// 解密指定的字节数据
            /// </summary>
            /// <param name="originalData">加密的字节数据</param>
            /// <param name="keyData">解密密钥</param>
            /// <param name="ivData"></param>
            /// <returns>原始文本</returns>
            private static byte[] Decrypt(byte[] encryptedData, byte[] keyData, byte[] ivData)
            {
                MemoryStream memoryStream = new MemoryStream();
                //创建Rijndael加密算法
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
            /// 加密一段文本
            /// </summary>
            /// <param name="szOriginalText">原始文本</param>
            /// <param name="szKey">加密密钥</param>
            /// <returns>加密后的密文</returns>
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
            /// 解密一段密文
            /// </summary>
            /// <param name="szOriginalText">密文文本</param>
            /// <param name="szKey">解密密钥</param>
            /// <returns>原始文本</returns>
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
