using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace TDC.SaveAndLoad
{
    public class CoreXml
    {
        #region Element

        private static void AddAttribute(XmlDocument xmlDoc, XmlNode userNode, string name, string value)
        {
            XmlAttribute attribute;

            attribute = xmlDoc.CreateAttribute(name);
            attribute.Value = value;
            userNode.Attributes.Append(attribute);
        }

        #endregion

        #region Data

        public class DataXml
        {
            public List<DataXmlAttribute> listAttribute;
            public int line { get; private set; }
            public string name { get; private set; }
            public string text { get; private set; }

            public void Initialization(string _name, string _text, List<DataXmlAttribute> _listAttribute = null)
            {
                name = _name;
                text = _text;

                if (_listAttribute != null)
                {
                    listAttribute = new List<DataXmlAttribute>(_listAttribute);
                }
                else
                {
                    listAttribute = new List<DataXmlAttribute>();
                }
            }

            public void SetName(string newName)
            {
                name = newName;
            }

            public void SetText(string newText)
            {
                text = newText;
            }
        }

        public class DataXmlAttribute
        {
            public string name { get; private set; }
            public string value { get; private set; }

            public void Initialization(string _name, string _value)
            {
                name = _name;
                value = _value;
            }
        }

        private const char delimentIO = '\\';
        private const string xmlFormat = ".xml";
        private const string nameAttribute = "Anttribute";

        #endregion

        #region Core

        private static void Initialization(string path, string fileXmlName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(path + delimentIO + fileXmlName))
            {
                File.Create(path + delimentIO + fileXmlName + xmlFormat).Close();
            }
        }

        public static void Save(string patch, string fileXmlName, List<DataXml> listData, string title = "Main")
        {
            Initialization(patch, fileXmlName);

            var xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(title);
            xmlDoc.AppendChild(rootNode);

            XmlNode userNode;

            for (var i = 0; i < listData.Count; i++)
            {
                userNode = xmlDoc.CreateElement(listData[i].name);
                userNode.InnerText = listData[i].text;

                if (listData[i].listAttribute.Count > 0)
                {
                    for (var w = 0; w < listData[i].listAttribute.Count; w++)
                        AddAttribute(xmlDoc, userNode, nameAttribute + w, listData[i].listAttribute[w].value);
                }

                rootNode.AppendChild(userNode);
            }

            xmlDoc.Save(patch + delimentIO + fileXmlName + xmlFormat);
            //UnityEditor.AssetDatabase.Refresh();
            Debug.Log("CoreXML: " + "Saved path " + patch + delimentIO + fileXmlName + xmlFormat);
        }

        public static List<DataXml> Load(string path, string fileXmlName)
        {
            if (!Directory.Exists(path))
            {
                return null;
            }

            if (!File.Exists(path + delimentIO + fileXmlName + xmlFormat))
            {
                return null;
            }

            var xmlReader = new XmlTextReader(path + delimentIO + fileXmlName + xmlFormat);

            var listDataXml = new List<DataXml>();

            DataXml data = null;

            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    data = new DataXml();
                    data.listAttribute = new List<DataXmlAttribute>();
                }

                if (data != null)
                {
                    if (xmlReader.LocalName != "")
                    {
                        data.SetName(xmlReader.LocalName);
                    }

                    if (xmlReader.Value != "")
                    {
                        data.SetText(xmlReader.Value);
                    }

                    if (xmlReader.AttributeCount > 0)
                    {
                        for (var i = 0; i < xmlReader.AttributeCount; i++)
                        {
                            var dataAttribute = new DataXmlAttribute();
                            dataAttribute.Initialization(nameAttribute + i, xmlReader.GetAttribute(i));

                            data.listAttribute.Add(dataAttribute);
                        }
                    }
                }

                if (xmlReader.NodeType == XmlNodeType.EndElement && data != null)
                {
                    listDataXml.Add(data);
                    data = null;
                }
            }

            return listDataXml;
        }

        #endregion
    }
}