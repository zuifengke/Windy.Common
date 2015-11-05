// ***********************************************************
// 封装一些基本的XML操作相关方法集合
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
        /// 封装XML操作相关方法
        /// </summary>
        public struct Xml
        {
            /// <summary>
            /// 创建一个指定根节点名的XML文件
            /// </summary>
            /// <param name="szFileName">XML文件</param>
            /// <param name="szRootName">根节点名</param>
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
                        , new string[] { szFileName, szRootName }, "XML文件的父目录没有创建成功!", null);
                    return false;
                }

                XmlDocument xmlDoc = Xml.CreateXmlDocument(szRootName);
                return Xml.SaveXmlDocument(xmlDoc, szFileName);
            }

            /// <summary>
            /// 创建一个指定根节点名的XML文档对象
            /// </summary>
            /// <param name="szRootName">根节点名</param>
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
            /// 得到XML文件对应的XmlDocument对象
            /// </summary>
            /// <param name="szXmlFile">本地XML文件路径</param>
            /// <returns>System.Xml.XmlDocument</returns>
            public static XmlDocument GetXmlDocument(string szXmlFile)
            {
                if (szXmlFile == null || szXmlFile.Trim() == "")
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetXmlDocument", new string[] { "szXmlFile" }
                        , new string[] { szXmlFile }, "参数不能为空!", null);
                    return null;
                }
                if (!System.IO.File.Exists(szXmlFile))
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetXmlDocument", new string[] { "szXmlFile" }
                        , new string[] { szXmlFile }, "本地文件不存在!", null);
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
            ///  得到XML字符串对应的XmlDocument对象
            /// </summary>
            /// <param name="szXmlData">XML格式的字符串</param>
            /// <returns>System.Xml.XmlDocument</returns>
            public static XmlDocument GetXmlDocumentByData(string szXmlData)
            {
                if (szXmlData == null || szXmlData.Trim() == "")
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetXmlDocumentByData", new string[] { "szXmlData" }
                        , new string[] { szXmlData }, "参数不能为空!", null);
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
            /// 将XML文档对象保存为XML文件
            /// </summary>
            /// <param name="xmlDoc">XML文档对象</param>
            /// <returns>string:保存结果</returns>
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
            /// 将XML文档对象保存为XML文件
            /// </summary>
            /// <param name="xmlDoc">XML文档对象</param>
            /// <param name="xmlFile">XML文件</param>
            /// <returns>bool:保存结果</returns>
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
            /// 选择XPath指定的一个单一节点
            /// </summary>
            /// <param name="clsRootNode">XPath所在的节点</param>
            /// <param name="szXPath">XPath</param>
            /// <returns>查询到的XML节点</returns>
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
            /// 选择XPath指定的一个节点集
            /// </summary>
            /// <param name="clsRootNode">XPath根节点</param>
            /// <param name="szXPath">XPath</param>
            /// <returns>查询到的XML节点集</returns>
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
            /// 获取节点的InnerText属性值
            /// </summary>
            /// <param name="clsRootNode">XPath指向的根节点</param>
            /// <param name="szXPath">节点的XPath</param>
            /// <param name="szValue">返回的节点值</param>
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
            /// 设置节点的InnerText属性值,如果节点不存在则创建
            /// </summary>
            /// <param name="clsRootNode">XPath指向的根节点</param>
            /// <param name="szXPath">节点的XPath</param>
            /// <param name="szValue">节点值</param>
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
                        , "XPath指定的节点创建失败!没有找到节点!", null);
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
                        , new object[] { clsRootNode, szXPath, szValue }, "XPath指定的节点创建失败!", ex);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 获取指定XML节点的InnerXml属性值
            /// </summary>
            /// <param name="clsRootNode">XPath指向的根节点</param>
            /// <param name="szXPath">节点的XPath</param>
            /// <param name="szXmlText">返回的节点XML数据</param>
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
            /// 创建指定名称的节点
            /// </summary>
            /// <param name="xmlDoc">XmlDocument</param>
            /// <param name="parentNode">父节点</param>
            /// <param name="szName">节点名</param>
            /// <param name="szValue">节点值</param>
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
            /// 在父节点内移动指定XML节点到引用的节点的前面,如果引用节点为空,那么将移动到末尾
            /// </summary>
            /// <param name="movedNode">被移动的节点</param>
            /// <param name="refNode">引用的节点</param>
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
            /// 删除指定的XML节点
            /// </summary>
            /// <param name="clsRootNode">被删除的节点</param>
            /// <returns>是否删除成功</returns>
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
            /// 给指定节点添加XML Schema声明
            /// </summary>
            /// <param name="szSchemaFilePath">Schema文件URI路径</param>
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
            /// 设置指定节点中指定属性的值
            /// </summary>
            /// <param name="clsXmlNode">XML节点</param>
            /// <param name="szAttrName">属性名</param>
            /// <param name="szAttrValue">属性值</param>
            /// <returns>bool</returns>
            public static bool SetXmlAttrValue(XmlNode clsXmlNode, string szAttrName, string szAttrValue)
            {
                return GlobalMethods.Xml.SetXmlAttrValue(clsXmlNode, null, szAttrName, szAttrValue);
            }

            /// <summary>
            /// 设置指定节点中指定属性的值
            /// </summary>
            /// <param name="clsXmlNode">XML节点</param>
            /// <param name="szPrefix">属性的前缀</param>
            /// <param name="szAttrName">属性名</param>
            /// <param name="szAttrValue">属性值</param>
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
                        , new object[] { clsXmlNode, szPrefix, szAttrName, szAttrValue }, "无法设置XML节点的属性值!", ex);
                    return false;
                }
            }

            /// <summary>
            ///  删除传入路径下面所有节点含有的InnerText，可以有多个路径
            /// </summary>
            /// <param name="xmlDoc">XmlDocument对象</param>
            /// <param name="strListXPath"> 路径的List</param>
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
                    LogManager.Instance.WriteLog("GlobalMethods.ClearElementContent执行失败", ex);
                    return false;
                }
                return true;

            }

            /// <summary>
            ///  递归删除节点的InnerText
            /// </summary>
            /// <param name="oNode">要删除的节点</param>
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
            /// 设定指定路径下的InnerText
            /// </summary>
            /// <param name="xmlDoc">XmlDocument对象</param>
            /// <param name="strXPath">路径</param>
            /// <param name="szData">内容</param>
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
                        , new object[] { strXPath, szData }, "无法设置XML节点的值!", ex);
                    return false;
                }
                return true;
            }
        }
    }
}
