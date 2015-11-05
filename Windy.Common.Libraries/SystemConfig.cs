// ***********************************************************
// 病历文档系统配置文件管理器.用于读写本地配置文件
// Creator:YangMingkun  Date:2009-7-6
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Windy.Common.Libraries
{
    public class SystemConfig
    {
        private static SystemConfig m_instance = null;
        /// <summary>
        /// 本类可以单实例运行
        /// </summary>
        public static SystemConfig Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new SystemConfig();
                return m_instance;
            }
        }
        private SystemConfig()
        {
        }

        private XmlDocument m_configXmlDoc = null;
        private DateTime m_lastModifyTime;

        private string m_configFile = null;
        /// <summary>
        /// 获取或设置配置文件名称
        /// </summary>
        public string ConfigFile
        {
            get { return this.m_configFile; }
            set
            {
                this.m_configFile = value;
                this.m_configXmlDoc = null;
            }
        }

        /// <summary>
        ///  读取指定的配置文件中指定Key的值
        /// </summary>
        /// <param name="szKeyName">读取的Key名称</param>
        /// <param name="defaultValue">指定的Key不存在时,返回的值</param>
        /// <returns>Key值</returns>
        public System.Drawing.Color Get(string szKeyName, System.Drawing.Color defaultValue)
        {
            try
            {
                int nColorValue = this.Get(szKeyName, defaultValue.ToArgb());
                return System.Drawing.Color.FromArgb(nColorValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///  读取指定的配置文件中指定Key的值
        /// </summary>
        /// <param name="szKeyName">读取的Key名称</param>
        /// <param name="szDefaultValue">指定的Key不存在时,返回的值</param>
        /// <returns>Key值</returns>
        public int Get(string szKeyName, int nDefaultValue)
        {
            string szValue = this.Get(szKeyName, nDefaultValue.ToString());
            try
            {
                return int.Parse(szValue);
            }
            catch
            {
                return nDefaultValue;
            }
        }

        /// <summary>
        ///  读取指定的配置文件中指定Key的值
        /// </summary>
        /// <param name="szKeyName">读取的Key名称</param>
        /// <param name="szDefaultValue">指定的Key不存在时,返回的值</param>
        /// <returns>Key值</returns>
        public float Get(string szKeyName, float fDefaultValue)
        {
            string szValue = this.Get(szKeyName, fDefaultValue.ToString());
            try
            {
                return float.Parse(szValue);
            }
            catch
            {
                return fDefaultValue;
            }
        }

        /// <summary>
        ///  读取指定的配置文件中指定Key的值
        /// </summary>
        /// <param name="szKeyName">读取的Key名称</param>
        /// <param name="szDefaultValue">指定的Key不存在时,返回的值</param>
        /// <returns>Key值</returns>
        public bool Get(string szKeyName, bool bDefaultValue)
        {
            string szValue = this.Get(szKeyName, bDefaultValue.ToString());
            try
            {
                return bool.Parse(szValue);
            }
            catch
            {
                return bDefaultValue;
            }
        }

        /// <summary>
        ///  读取指定的配置文件中指定Key的值
        /// </summary>
        /// <param name="szKeyName">读取的Key名称</param>
        /// <param name="szDefaultValue">指定的Key不存在时,返回的值</param>
        /// <returns>Key值</returns>
        public decimal Get(string szKeyName, decimal bDefaultValue)
        {
            string szValue = this.Get(szKeyName, bDefaultValue.ToString());
            try
            {
                return decimal.Parse(szValue);
            }
            catch
            {
                return bDefaultValue;
            }
        }

        /// <summary>
        ///  读取指定的配置文件中指定Key的值
        /// </summary>
        /// <param name="szKeyName">读取的Key名称</param>
        /// <param name="dtDefaultValue">指定的Key不存在时,返回的值</param>
        /// <returns>Key值</returns>
        public DateTime Get(string szKeyName, DateTime dtDefaultValue)
        {
            string szValue = this.Get(szKeyName, dtDefaultValue.ToString());
            try
            {
                return DateTime.Parse(szValue);
            }
            catch
            {
                return dtDefaultValue;
            }
        }

        /// <summary>
        ///  读取指定的配置文件中指定Key的值
        /// </summary>
        /// <param name="szKeyName">读取的Key名称</param>
        /// <param name="szDefaultValue">指定的Key不存在时,返回的值</param>
        /// <returns>Key值</returns>
        public string Get(string szKeyName, string szDefaultValue)
        {
            if (this.m_configXmlDoc == null)
            {
                if (!File.Exists(this.m_configFile))
                    return szDefaultValue;
                this.m_configXmlDoc = GlobalMethods.Xml.GetXmlDocument(this.m_configFile);

                //把配置文件最后修改时间记录下来
                GlobalMethods.IO.GetFileLastModifyTime(this.m_configFile, ref this.m_lastModifyTime);
            }
            if (this.m_configXmlDoc == null)
                return szDefaultValue;

            string szXPath = string.Format(".//key[@name='{0}']", szKeyName);
            XmlNode clsXmlNode = GlobalMethods.Xml.SelectXmlNode(this.m_configXmlDoc, szXPath);
            if (clsXmlNode == null)
            {
                return szDefaultValue;
            }

            XmlNode clsValueAttr = clsXmlNode.Attributes.GetNamedItem("value");
            if (clsValueAttr == null)
                return szDefaultValue;
            return clsValueAttr.Value;
        }

        /// <summary>
        ///  保存指定Key的值到指定的配置文件中
        /// </summary>
        /// <param name="szKeyName">要被修改值的Key名称</param>
        /// <param name="szValue">新修改的值</param>
        public bool Write(string szKeyName, string szValue)
        {
            if (!File.Exists(this.m_configFile))
            {
                if (!GlobalMethods.Xml.CreateXmlFile(this.m_configFile, "SystemConfig"))
                    return false;
            }

            //检查配置文件是否已被其他程序修改
            DateTime dtModifyTime = DateTime.Now;
            bool bFileChanged = true;
            if (this.m_configXmlDoc == null)
                bFileChanged = true;
            else if (!GlobalMethods.IO.GetFileLastModifyTime(this.m_configFile, ref dtModifyTime))
                bFileChanged = false;
            else if (GlobalMethods.SysTime.CompareTime(dtModifyTime, this.m_lastModifyTime))
                bFileChanged = false;

            //如果已经被修改,则重新读取后再更新
            if (bFileChanged)
                this.m_configXmlDoc = GlobalMethods.Xml.GetXmlDocument(this.m_configFile);
            if (this.m_configXmlDoc == null)
                return false;

            string szXPath = string.Format(".//key[@name='{0}']", szKeyName);
            XmlNode clsXmlNode = GlobalMethods.Xml.SelectXmlNode(this.m_configXmlDoc, szXPath);
            if (clsXmlNode == null)
            {
                clsXmlNode = GlobalMethods.Xml.CreateXmlNode(this.m_configXmlDoc, null, "key", null);
            }
            if (!GlobalMethods.Xml.SetXmlAttrValue(clsXmlNode, "name", szKeyName))
                return false;
            if (!GlobalMethods.Xml.SetXmlAttrValue(clsXmlNode, "value", szValue))
                return false;
            //
            bool success = GlobalMethods.Xml.SaveXmlDocument(this.m_configXmlDoc, this.m_configFile);
            GlobalMethods.IO.GetFileLastModifyTime(this.m_configFile, ref this.m_lastModifyTime);
            return success;
        }
    }
}
