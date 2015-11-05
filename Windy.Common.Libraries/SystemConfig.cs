// ***********************************************************
// �����ĵ�ϵͳ�����ļ�������.���ڶ�д���������ļ�
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
        /// ������Ե�ʵ������
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
        /// ��ȡ�����������ļ�����
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
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="defaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
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
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="szDefaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
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
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="szDefaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
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
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="szDefaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
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
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="szDefaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
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
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="dtDefaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
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
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="szDefaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
        public string Get(string szKeyName, string szDefaultValue)
        {
            if (this.m_configXmlDoc == null)
            {
                if (!File.Exists(this.m_configFile))
                    return szDefaultValue;
                this.m_configXmlDoc = GlobalMethods.Xml.GetXmlDocument(this.m_configFile);

                //�������ļ�����޸�ʱ���¼����
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
        ///  ����ָ��Key��ֵ��ָ���������ļ���
        /// </summary>
        /// <param name="szKeyName">Ҫ���޸�ֵ��Key����</param>
        /// <param name="szValue">���޸ĵ�ֵ</param>
        public bool Write(string szKeyName, string szValue)
        {
            if (!File.Exists(this.m_configFile))
            {
                if (!GlobalMethods.Xml.CreateXmlFile(this.m_configFile, "SystemConfig"))
                    return false;
            }

            //��������ļ��Ƿ��ѱ����������޸�
            DateTime dtModifyTime = DateTime.Now;
            bool bFileChanged = true;
            if (this.m_configXmlDoc == null)
                bFileChanged = true;
            else if (!GlobalMethods.IO.GetFileLastModifyTime(this.m_configFile, ref dtModifyTime))
                bFileChanged = false;
            else if (GlobalMethods.SysTime.CompareTime(dtModifyTime, this.m_lastModifyTime))
                bFileChanged = false;

            //����Ѿ����޸�,�����¶�ȡ���ٸ���
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
