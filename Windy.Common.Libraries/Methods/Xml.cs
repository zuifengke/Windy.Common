// ***********************************************************
// ��װһЩ������XML������ط�������
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Windy.Common.Libraries
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// ��װXML������ط���
        /// </summary>
        public struct Xml
        {
            /// <summary>
            /// ����һ��ָ�����ڵ�����XML�ļ�
            /// </summary>
            /// <param name="szFileName">XML�ļ�</param>
            /// <param name="szRootName">���ڵ���</param>
            /// <returns>bool</returns>
            public static bool CreateXmlFile(string szFileName, string szRootName)
            {
                if (szFileName == null || szFileName.Trim() == "")
                    return false;
                if (szRootName == null || szRootName.Trim() == "")
                    return false;

                string szParentDir = IO.GetFilePath(szFileName);
                if (!IO.CreateDirectory(szParentDir))
                {
                    LogManager.Instance.WriteLog("GlobalMethods.CreateXmlFile", new string[] { "szFileName", "szRootName" }
                        , new string[] { szFileName, szRootName }, "XML�ļ��ĸ�Ŀ¼û�д����ɹ�!", null);
                    return false;
                }

                XmlDocument xmlDoc = Xml.CreateXmlDocument(szRootName);
                return Xml.SaveXmlDocument(xmlDoc, szFileName);
            }

            /// <summary>
            /// ����һ��ָ�����ڵ�����XML�ĵ�����
            /// </summary>
            /// <param name="szRootName">���ڵ���</param>
            /// <returns>bool</returns>
            public static XmlDocument CreateXmlDocument(string szRootName)
            {
                if (szRootName == null || szRootName.Trim() == "")
                    return null;

                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    if (xmlDoc.PreserveWhitespace)
                        xmlDoc.PreserveWhitespace = false;
                    xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "gbk", null));
                    xmlDoc.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, szRootName, ""));
                    return xmlDoc;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.CreateXmlDocument", new string[] { "szRootName" }
                        , new string[] { szRootName }, ex);
                    return null;
                }
            }

            /// <summary>
            /// �õ�XML�ļ���Ӧ��XmlDocument����
            /// </summary>
            /// <param name="szXmlFile">����XML�ļ�·��</param>
            /// <returns>System.Xml.XmlDocument</returns>
            public static XmlDocument GetXmlDocument(string szXmlFile)
            {
                if (szXmlFile == null || szXmlFile.Trim() == "")
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetXmlDocument", new string[] { "szXmlFile" }
                        , new string[] { szXmlFile }, "��������Ϊ��!", null);
                    return null;
                }
                if (!System.IO.File.Exists(szXmlFile))
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetXmlDocument", new string[] { "szXmlFile" }
                        , new string[] { szXmlFile }, "�����ļ�������!", null);
                    return null;
                }
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (xmlDoc.PreserveWhitespace)
                        xmlDoc.PreserveWhitespace = false;
                    xmlDoc.Load(szXmlFile);
                    return xmlDoc;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetXmlDocument", new string[] { "szXmlFile" }
                        , new string[] { szXmlFile }, null, ex);
                    return null;
                }
            }

            /// <summary>
            ///  �õ�XML�ַ�����Ӧ��XmlDocument����
            /// </summary>
            /// <param name="szXmlData">XML��ʽ���ַ���</param>
            /// <returns>System.Xml.XmlDocument</returns>
            public static XmlDocument GetXmlDocumentByData(string szXmlData)
            {
                if (szXmlData == null || szXmlData.Trim() == "")
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetXmlDocumentByData", new string[] { "szXmlData" }
                        , new string[] { szXmlData }, "��������Ϊ��!", null);
                    return null;
                }

                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (xmlDoc.PreserveWhitespace)
                        xmlDoc.PreserveWhitespace = false;
                    xmlDoc.LoadXml(szXmlData);
                    return xmlDoc;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetXmlDocumentByData", new string[] { "szXmlData" }
                        , new string[] { szXmlData }, null, ex);
                    return null;
                }
            }

            public class GBKStringWriter : StringWriter
            {
                public override Encoding Encoding
                {
                    get { return Convert.GetDefaultEncoding(); }
                }
            }

            /// <summary>
            /// ��XML�ĵ����󱣴�ΪXML�ļ�
            /// </summary>
            /// <param name="xmlDoc">XML�ĵ�����</param>
            /// <returns>string:������</returns>
            public static string SaveXmlText(XmlDocument xmlDoc)
            {
                if (xmlDoc == null)
                    return null;
                try
                {
                    GBKStringWriter textWriter = new GBKStringWriter();
                    XmlTextWriter xmlWriter = new XmlTextWriter(textWriter);

                    xmlWriter.Formatting = Formatting.Indented;
                    if (xmlDoc != null)
                        xmlDoc.WriteTo(xmlWriter);

                    string xmlText = textWriter.ToString();
                    xmlWriter.Close();
                    textWriter.Close();
                    textWriter.Dispose();
                    return xmlText;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.SaveXmlText", new string[] { "xmlDoc" }
                        , new object[] { xmlDoc }, null, ex);
                    return null;
                }
            }

            /// <summary>
            /// ��XML�ĵ����󱣴�ΪXML�ļ�
            /// </summary>
            /// <param name="xmlDoc">XML�ĵ�����</param>
            /// <param name="xmlFile">XML�ļ�</param>
            /// <returns>bool:������</returns>
            public static bool SaveXmlDocument(XmlDocument xmlDoc, string xmlFile)
            {
                if (GlobalMethods.Misc.IsEmptyString(xmlFile))
                    return false;
                if (xmlDoc == null)
                    return false;

                if (!GlobalMethods.IO.CreateDirectory(GlobalMethods.IO.GetFilePath(xmlFile)))
                    return false;
                try
                {
                    if (xmlDoc.PreserveWhitespace)
                        xmlDoc.PreserveWhitespace = false;
                    xmlDoc.Save(xmlFile);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.SaveXmlDocument", new string[] { "xmlFile" }
                        , new string[] { xmlFile }, null, ex);
                    return false;
                }
            }

            /// <summary>
            /// ѡ��XPathָ����һ����һ�ڵ�
            /// </summary>
            /// <param name="clsRootNode">XPath���ڵĽڵ�</param>
            /// <param name="szXPath">XPath</param>
            /// <returns>��ѯ����XML�ڵ�</returns>
            public static XmlNode SelectXmlNode(XmlNode clsRootNode, string szXPath)
            {
                if (clsRootNode == null)
                    return null;
                if (szXPath == null || szXPath.Trim() == "")
                    return null;
                XmlNode clsSelectedNode = null;
                try
                {
                    clsSelectedNode = clsRootNode.SelectSingleNode(szXPath);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.SelectXmlNode"
                        , new string[] { "clsRootNode", "szXPath" }, new object[] { clsRootNode, szXPath }, ex);
                    return null;
                }
                return clsSelectedNode;
            }

            /// <summary>
            /// ѡ��XPathָ����һ���ڵ㼯
            /// </summary>
            /// <param name="clsRootNode">XPath���ڵ�</param>
            /// <param name="szXPath">XPath</param>
            /// <returns>��ѯ����XML�ڵ㼯</returns>
            public static XmlNodeList SelectXmlNodes(XmlNode clsRootNode, string szXPath)
            {
                if (clsRootNode == null)
                    return null;
                if (szXPath == null || szXPath.Trim() == "")
                    return null;
                XmlNodeList lstSelectedNodes = null;
                try
                {
                    lstSelectedNodes = clsRootNode.SelectNodes(szXPath);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.SelectXmlNodes"
                        , new string[] { "clsRootNode", "szXPath" }, new object[] { clsRootNode, szXPath }, ex);
                    return null;
                }
                return lstSelectedNodes;
            }

            /// <summary>
            /// ��ȡ�ڵ��InnerText����ֵ
            /// </summary>
            /// <param name="clsRootNode">XPathָ��ĸ��ڵ�</param>
            /// <param name="szXPath">�ڵ��XPath</param>
            /// <param name="szValue">���صĽڵ�ֵ</param>
            /// <returns>bool</returns>
            public static bool GetXmlNodeValue(XmlNode clsRootNode, string szXPath, ref string szValue)
            {
                szValue = string.Empty;

                if (szXPath == null || szXPath.Trim() == string.Empty)
                    return false;

                if (clsRootNode == null)
                    return false;

                XmlNode clsSelectedNode = GlobalMethods.Xml.SelectXmlNode(clsRootNode, szXPath);
                if (clsSelectedNode == null)
                    return false;
                szValue = clsSelectedNode.InnerText;
                return true;
            }

            /// <summary>
            /// ���ýڵ��InnerText����ֵ,����ڵ㲻�����򴴽�
            /// </summary>
            /// <param name="clsRootNode">XPathָ��ĸ��ڵ�</param>
            /// <param name="szXPath">�ڵ��XPath</param>
            /// <param name="szValue">�ڵ�ֵ</param>
            /// <returns>bool</returns>
            public static bool SetXmlNodeValue(XmlNode clsRootNode, string szXPath, string szValue)
            {
                if (GlobalMethods.Misc.IsEmptyString(szXPath))
                    return false;

                if (clsRootNode == null)
                    return false;

                XmlNode clsSelectedNode = GlobalMethods.Xml.SelectXmlNode(clsRootNode, szXPath);
                if (clsSelectedNode == null)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.SetXmlNodeValue"
                        , new string[] { "clsRootNode", "szXPath", "szValue" }
                        , new object[] { clsRootNode, szXPath, szValue }
                        , "XPathָ���Ľڵ㴴��ʧ��!û���ҵ��ڵ�!", null);
                    return false;
                }
                try
                {
                    clsSelectedNode.InnerText = szValue;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.SetXmlNodeValue"
                        , new string[] { "clsRootNode", "szXPath", "szValue" }
                        , new object[] { clsRootNode, szXPath, szValue }, "XPathָ���Ľڵ㴴��ʧ��!", ex);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// ��ȡָ��XML�ڵ��InnerXml����ֵ
            /// </summary>
            /// <param name="clsRootNode">XPathָ��ĸ��ڵ�</param>
            /// <param name="szXPath">�ڵ��XPath</param>
            /// <param name="szXmlText">���صĽڵ�XML����</param>
            /// <returns>bool</returns>
            public static bool GetXmlNodeXmlText(XmlNode clsRootNode, string szXPath, ref string szXmlText)
            {
                szXmlText = string.Empty;

                if (szXPath == null || szXPath.Trim() == string.Empty)
                    return false;

                if (clsRootNode == null)
                    return false;

                XmlNode clsSelectedNode = GlobalMethods.Xml.SelectXmlNode(clsRootNode, szXPath);
                if (clsSelectedNode == null)
                    return false;

                try
                {
                    szXmlText = clsSelectedNode.InnerXml;
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetXmlNodeXmlText"
                        , new string[] { "clsRootNode", "szXPath" }, new object[] { clsRootNode, szXPath }, ex);
                    return false;
                }
            }

            /// <summary>
            /// ����ָ�����ƵĽڵ�
            /// </summary>
            /// <param name="xmlDoc">XmlDocument</param>
            /// <param name="parentNode">���ڵ�</param>
            /// <param name="szName">�ڵ���</param>
            /// <param name="szValue">�ڵ�ֵ</param>
            /// <returns>XmlNode</returns>
            public static XmlNode CreateXmlNode(XmlDocument xmlDoc, XmlNode parentNode, string szName, string szValue)
            {
                if (xmlDoc == null || GlobalMethods.Misc.IsEmptyString(szName))
                    return null;

                szName = szName.Trim();
                XmlNode createdNode = null;
                try
                {
                    if (szName.StartsWith("@"))
                    {
                        createdNode = xmlDoc.CreateAttribute(szName.Remove(0, 1));
                        if (parentNode != null && createdNode != null)
                            parentNode.Attributes.Append((XmlAttribute)createdNode);
                    }
                    else
                    {
                        createdNode = xmlDoc.CreateElement(szName);
                        if (createdNode != null && parentNode != null)
                        {
                            parentNode.AppendChild(createdNode);
                        }
                        if (createdNode != null && parentNode == null)
                        {
                            if (xmlDoc.DocumentElement == null)
                                xmlDoc.AppendChild(createdNode);
                            else
                                xmlDoc.DocumentElement.AppendChild(createdNode);
                        }
                    }
                    if (createdNode != null && !GlobalMethods.Misc.IsEmptyString(szValue))
                        createdNode.InnerText = szValue.Trim();
                    return createdNode;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.CreateXmlNode"
                        , new string[] { "xmlDoc", "parentNode", "szName", "szValue" }, new object[] { xmlDoc, parentNode, szName, szValue }, ex);
                    return null;
                }
            }

            /// <summary>
            /// �ڸ��ڵ����ƶ�ָ��XML�ڵ㵽���õĽڵ��ǰ��,������ýڵ�Ϊ��,��ô���ƶ���ĩβ
            /// </summary>
            /// <param name="movedNode">���ƶ��Ľڵ�</param>
            /// <param name="refNode">���õĽڵ�</param>
            /// <returns>bool</returns>
            public static bool MoveXmlNode(XmlNode movedNode, XmlNode refNode)
            {
                if (movedNode == null || movedNode.ParentNode == null)
                    return false;
                XmlNode parentNode = movedNode.ParentNode;
                if (refNode != null && parentNode != refNode.ParentNode)
                    return false;
                try
                {
                    parentNode.RemoveChild(movedNode);
                    if (refNode == null)
                        parentNode.AppendChild(movedNode);
                    else
                        parentNode.InsertBefore(movedNode, refNode);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.MoveXmlNode"
                        , new string[] { "movedNode", "refNode" }, new object[] { movedNode, refNode }, ex);
                    return false;
                }
            }

            /// <summary>
            /// ɾ��ָ����XML�ڵ�
            /// </summary>
            /// <param name="clsRootNode">��ɾ���Ľڵ�</param>
            /// <returns>�Ƿ�ɾ���ɹ�</returns>
            public static bool DeleteXmlNode(XmlNode clsDeletedNode)
            {
                if (clsDeletedNode == null)
                    return true;
                try
                {
                    if (clsDeletedNode.ParentNode != null)
                        clsDeletedNode.ParentNode.RemoveChild(clsDeletedNode);
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.DeleteXmlNode"
                        , new string[] { "clsDeletedNode" }, new string[] { clsDeletedNode.ToString() }, ex);
                    return false;
                }
            }

            /// <summary>
            /// ��ָ���ڵ����XML Schema����
            /// </summary>
            /// <param name="szSchemaFilePath">Schema�ļ�URI·��</param>
            /// <returns>bool</returns>
            public static bool AddSchemaDeclaration(XmlDocument xmlDoc, string szSchemaFilePath)
            {
                if (xmlDoc == null || xmlDoc.DocumentElement == null)
                    return false;
                XmlElement rootElement = xmlDoc.DocumentElement;
                if (!SetXmlAttrValue(rootElement, "xmlns", "xsi", "http://www.w3.org/2001/XMLSchema-instance"))
                    return false;
                if (!SetXmlAttrValue(rootElement, "xsi", "noNamespaceSchemaLocation", szSchemaFilePath))
                    return false;
                return true;
            }

            /// <summary>
            /// ����ָ���ڵ���ָ�����Ե�ֵ
            /// </summary>
            /// <param name="clsXmlNode">XML�ڵ�</param>
            /// <param name="szAttrName">������</param>
            /// <param name="szAttrValue">����ֵ</param>
            /// <returns>bool</returns>
            public static bool SetXmlAttrValue(XmlNode clsXmlNode, string szAttrName, string szAttrValue)
            {
                return GlobalMethods.Xml.SetXmlAttrValue(clsXmlNode, null, szAttrName, szAttrValue);
            }

            /// <summary>
            /// ����ָ���ڵ���ָ�����Ե�ֵ
            /// </summary>
            /// <param name="clsXmlNode">XML�ڵ�</param>
            /// <param name="szPrefix">���Ե�ǰ׺</param>
            /// <param name="szAttrName">������</param>
            /// <param name="szAttrValue">����ֵ</param>
            /// <returns>bool</returns>
            public static bool SetXmlAttrValue(XmlNode clsXmlNode, string szPrefix, string szAttrName, string szAttrValue)
            {
                if (clsXmlNode == null)
                    return false;
                XmlDocument clsXmlDoc = clsXmlNode.OwnerDocument;
                if (clsXmlDoc == null)
                    return false;
                if (szAttrName == null || szAttrName.Trim() == "")
                    return false;
                if (szAttrValue == null)
                    szAttrValue = string.Empty;
                try
                {
                    XmlAttribute clsAttrNode = clsXmlNode.Attributes.GetNamedItem(szAttrName) as XmlAttribute;
                    if (clsAttrNode == null)
                    {
                        string szNamespaceUri = null;
                        if (szPrefix == "xmlns")
                            szNamespaceUri = "http://www.w3.org/2000/xmlns/";
                        else if (!GlobalMethods.Misc.IsEmptyString(szPrefix))
                            szNamespaceUri = clsXmlNode.GetNamespaceOfPrefix(szPrefix);
                        clsAttrNode = clsXmlDoc.CreateAttribute(szPrefix, szAttrName, szNamespaceUri);
                        clsXmlNode.Attributes.Append(clsAttrNode);
                    }
                    clsAttrNode.InnerText = szAttrValue;
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.SetXmlAttrValue"
                       , new string[] { "clsXmlNode", "szPrefix", "szAttrName", "szAttrValue" }
                        , new object[] { clsXmlNode, szPrefix, szAttrName, szAttrValue }, "�޷�����XML�ڵ������ֵ!", ex);
                    return false;
                }
            }

            /// <summary>
            ///  ɾ������·���������нڵ㺬�е�InnerText�������ж��·��
            /// </summary>
            /// <param name="xmlDoc">XmlDocument����</param>
            /// <param name="strListXPath"> ·����List</param>
            /// <returns></returns>
            public static bool ClearElementContent(XmlDocument xmlDoc, List<string> strListXPath)
            {
                try
                {
                    foreach (string strXPath in strListXPath)
                    {
                        XmlNode oNode = xmlDoc.SelectSingleNode(strXPath);
                        if (oNode == null)
                            return false;
                        ClearNodeElementContent(oNode);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.ClearElementContentִ��ʧ��", ex);
                    return false;
                }
                return true;

            }

            /// <summary>
            ///  �ݹ�ɾ���ڵ��InnerText
            /// </summary>
            /// <param name="oNode">Ҫɾ���Ľڵ�</param>
            public static void ClearNodeElementContent(XmlNode oNode)
            {
                if (oNode.ChildNodes.Count == 0)
                {
                    oNode.InnerText = string.Empty;
                }
                else
                {
                    foreach (XmlNode oChildNode in oNode.ChildNodes)
                    {
                        ClearNodeElementContent(oChildNode);
                    }
                }
            }

            /// <summary>
            /// �趨ָ��·���µ�InnerText
            /// </summary>
            /// <param name="xmlDoc">XmlDocument����</param>
            /// <param name="strXPath">·��</param>
            /// <param name="szData">����</param>
            /// <returns></returns>
            public static bool SetElementContent(XmlDocument xmlDoc, string strXPath, string szData)
            {
                try
                {
                    XmlNode oNode = xmlDoc.SelectSingleNode(strXPath);
                    if (oNode.ChildNodes.Count == 0)
                    {
                        oNode.InnerText = szData;
                    }
                    else
                    {
                        if (oNode.ChildNodes[0].NodeType == XmlNodeType.Text)
                        {
                            oNode.ChildNodes[0].Value = szData;
                        }
                        else
                        {
                            XmlText oNewText = xmlDoc.CreateTextNode(szData);
                            oNode.PrependChild(oNewText);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.SetElementContent"
                        , new string[] { "strXPath", "szData" }
                        , new object[] { strXPath, szData }, "�޷�����XML�ڵ��ֵ!", ex);
                    return false;
                }
                return true;
            }
        }
    }
}
