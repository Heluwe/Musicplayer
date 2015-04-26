using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace XiMusic
{
    /// <summary>
    /// XML Element属性集合
    /// </summary>
    public class Attribute
    {
        private string attributeName;

        public string AttributeName
        {
            get { return attributeName; }
            set { attributeName = value; }
        }

        private string attributeValue;

        public string AttributeValue
        {
            get { return attributeValue; }
            set { attributeValue = value; }
        }
    }

    public class XmlHelper
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Read(path, "/Node", "")
         * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public static string Read(string path, string node, string attribute)
        {
            string value = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch { }
            return value;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Read(path, "/Node", "")
         * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public static XmlNodeList Read(string path, string node)
        {
            XmlNodeList xmlNodeList = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                xmlNodeList = doc.SelectNodes(node);
            }
            catch
            {

            }
            return xmlNodeList;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Read(path, "/Node", "")
         * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public static string ReadElementAttributeValue(string path, string node, string attributeName, string attributeValue, string attribute)
        {
            using (XmlReader reader = XmlReader.Create(path))
            {
                while (reader.Read())
                {
                    if (reader.Name == node && reader.NodeType == XmlNodeType.Element && !string.IsNullOrEmpty(reader.GetAttribute(attributeName)) && reader.GetAttribute(attributeName) == attributeValue)
                    {
                        return attribute = reader.GetAttribute(attribute);
                    }
                }
            }

            return attribute;
        }
        public static string ReadElementAttributeValueOnline(string path, string node, string attributeName, string attribute)
        {
            using (XmlReader reader = XmlReader.Create(path))
            {
                while (reader.Read())
                {
                    if (reader.Name == node && reader.NodeType == XmlNodeType.Element && !string.IsNullOrEmpty(reader.GetAttribute(attributeName)))
                    {
                        return attribute = reader.GetAttribute(attribute);
                    }
                }
            }

            return attribute;
        }

        /// <summary>
        /// 判断某个节点是否存在
        /// </summary>
        /// <param name="xmlPath">Xml路径</param>
        /// <param name="node">节点名称</param>
        /// <returns></returns>
        public static bool IsSelectNode(string xmlPath, string node)
        {
            using (XmlReader reader = XmlReader.Create(xmlPath))
            {
                while (reader.Read())
                {
                    if (reader.Name == node && reader.NodeType == XmlNodeType.Element)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断某个节点属性是否存在
        /// </summary>
        /// <param name="xmlPath">Xml路径</param>
        /// <param name="node">节点名称</param>
        /// <param name="attributeName">属性名称</param>
        /// <returns></returns>
        public static bool IsSelectNode(string xmlPath, string node, string attributeName)
        {
            //定义XmlDocument对象Load xml文件  
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            //取得总结点  
            XmlNode xmlNod = xmlDoc.SelectSingleNode(node);
            //string str = xmlNod["Item"].ChildNodes[1].Attributes[0].Value;  
            //取得其所有子结点  
            XmlNodeList xnl = xmlNod.ChildNodes;

            foreach (XmlNode xn in xnl)
            {
                if (!string.IsNullOrEmpty(xn.Attributes[attributeName].Value))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断某个节点属性是否存在
        /// </summary>
        /// <param name="xmlPath">Xml路径</param>
        /// <param name="node">节点名称</param>
        /// <param name="attributeName">属性名称</param>
        /// <param name="arttributeValue">属性值</param>
        /// <returns></returns>
        public static bool IsSelectNode(string xmlPath, string node, string attributeName, string arttributeValue)
        {
            //定义XmlDocument对象Load xml文件  
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            //取得总结点  
            XmlNode xmlNod = xmlDoc.SelectSingleNode(node);
            //string str = xmlNod["Item"].ChildNodes[1].Attributes[0].Value;  
            //取得其所有子结点  
            XmlNodeList xnl = xmlNod.ChildNodes;

            foreach (XmlNode xn in xnl)
            {
                if (xn.Attributes[attributeName].Value == arttributeValue)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
         * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
         ************************************************/
        public static void Insert(string path, string node, string element, IList<Attribute> list)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (string.IsNullOrEmpty(element))
                {
                    if (list.Count > 0)
                    {
                        XmlElement xe = (XmlElement)xn;
                        foreach (var item in list)
                        {
                            xe.SetAttribute(item.AttributeName, item.AttributeValue);
                        }
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            xe.SetAttribute(item.AttributeName, item.AttributeValue);
                        }
                    }
                    xn.AppendChild(xe);
                }
                doc.Save(path);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
         ************************************************/
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                doc.Save(path);
            }
            catch { }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Delete(path, "/Node", "")
         * XmlHelper.Delete(path, "/Node", "Attribute")
         ************************************************/
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                doc.Save(path);
            }
            catch { }
        }

    }
}
