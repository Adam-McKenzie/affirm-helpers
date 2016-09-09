using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace ConfigHelper
{
    class ConfigCompare
    {
        //goal
        //build a tool that will compare xml documents for equality regardless of child order


        public bool CompareConfigs(string leftPath, string rightPath)
        {
            //get documents as XDocuments
            XDocument leftDoc = null;
            XDocument rightDoc = null;
            using (System.IO.TextReader leftReader = new System.IO.StreamReader(leftPath))
            using (System.IO.TextReader rightReader = new System.IO.StreamReader(rightPath))
            {
                leftDoc = XDocument.Parse(leftReader.ReadToEnd());
                rightDoc = XDocument.Parse(rightReader.ReadToEnd());
            }
            //remove all comments
            leftDoc.Root.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Remove();
            rightDoc.Root.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Remove();

            //sort both config elements before deep compare
            leftDoc = SortConfigDocument(leftDoc);
            rightDoc = SortConfigDocument(rightDoc);

            //leftDoc.Save(@"C:\TestFiles\CodeManagement\outputPath\left.config");
            //rightDoc.Save(@"C:\TestFiles\CodeManagement\outputPath\right.config");
            
            //check if the two configs are equal
            return XNode.DeepEquals(leftDoc, rightDoc);
        }

        private XDocument SortConfigDocument(XDocument doc)
        {
            //get all children into a sorted list
            List<XElement> childNodes = doc.Root.Elements().OrderBy(x => x.Name.LocalName).ToList();
            doc.Root.Elements().Remove();
            for (int i = 0; i < childNodes.Count(); i++)
            {
                doc.Root.Add(childNodes[i]);
            }
            //XDocument returnDoc = XDocument.Parse(root.ToString());
            //get appSettings node
            XElement appSettingsNode = doc.Root.Elements().FirstOrDefault(x => x.Name.LocalName == "appSettings");
            //sort appSettings node
            appSettingsNode = SortAppSettings(appSettingsNode);
            return doc;
        }

        private XElement SortAppSettings(XElement ele)
        {
            List<XElement> addNodes = ele.Elements().OrderBy(x => x.Attributes("key").Select(a => a.Value).FirstOrDefault()).ToList();
            ele.Elements().Remove();
            for (int i = 0; i < addNodes.Count(); i++)
            {
                ele.Add(addNodes[i]);
            }

            return ele;
        }
    }
}
