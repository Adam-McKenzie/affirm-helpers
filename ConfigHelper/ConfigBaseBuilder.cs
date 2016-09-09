using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace ConfigHelper
{
    class ConfigBaseBuilder
    {
        //arguments - file path to directory?
        //method to build the base config
            //grab first file
            //for each file in the directory
                //pull in the file
                //compare appsettings (method will take care of all appsetting node)
                //compare the rest of the nodes
                    //for every node in the base config
                        //if the node doesn't exist in the compare, remove it from base
        //add <?xml version="1.0" encoding="utf-8"?> TODO

        public XDocument BuildBaseDocument(XElement baseFile, XElement nextFile)
        {
            baseFile.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Remove();
            nextFile.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Remove();
            //remove unused keys from current base for appSettings node
            //TODO: Refactor to using firstordefault in place of where clause
            XElement baseAppSettings = baseFile.Elements().Where(x => x.Name.LocalName == "appSettings").FirstOrDefault();
            XElement nextConfigApps = nextFile.Elements().Where(x => x.Name.LocalName == "appSettings").FirstOrDefault();
            baseAppSettings = RemoveUnmatchedAppSettingKeysFromBase(baseAppSettings, nextConfigApps);

            //Remove any nodes that are in the base but not in the compared file
            IEnumerable<XElement> baseNonAppSettingsElements = baseFile.Elements().Where(x => x.Name.LocalName != "appSettings");
            IEnumerable<string> nextAppKeyNames = nextFile.Elements().Where(x => x.Name.LocalName != "appSettings").Select(ele => ele.Name.LocalName);
            baseNonAppSettingsElements.Where(x => !nextAppKeyNames.Contains(x.Name.LocalName)).Remove();

            XDocument returnDoc = XDocument.Parse(baseFile.ToString());
            return returnDoc;
        }

        private XElement RemoveUnmatchedAppSettingKeysFromBase(XElement baseAppSettings, XElement newAppSettings) 
        {
            //container of add elements in config file
            IEnumerable<XElement> baseKeys = baseAppSettings.Descendants().Where(x => x.Name.LocalName == "add");
            //container of keys from each add element in base config
            IEnumerable<string> keyNames = newAppSettings.Descendants().Where(x => x.Name.LocalName == "add")
                                        .Attributes().Where(x => x.Name.LocalName == "key")
                                        .Select(attr => attr.Value);
            baseAppSettings.Descendants().Where(x => !keyNames.Contains(x.Attributes("key").FirstOrDefault().Value)).Remove();
        
            return baseAppSettings;
        }

        //TODO ADD LOCAL ITEM VALUES TO BASE SO DEV CONFIG WILL BE CORRECT
    }
}
