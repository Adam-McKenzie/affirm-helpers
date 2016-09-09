using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace ConfigHelper
{
    class ConfigTransformBuilder
    {
        //public XElement addAppSettings(XElement baseFile, XElement currentFile)
        //{
        //    //add XDeclaration, Configuration as root
        //    XElement baseAppSettings = baseFile.Elements().Where(x => x.Name.LocalName == "appSettings").FirstOrDefault();
        //    XElement currentAppSettings = currentFile.Elements().Where(x => x.Name.LocalName == "appSettings").FirstOrDefault();

        //    XElement transformAppSettings = CreateAppSettings(baseAppSettings, currentAppSettings);
        //    return transformAppSettings;
        //}

        public XDocument BuildDocument(XElement baseFile, XElement currentFile)
        {
            baseFile.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Remove();
            currentFile.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Remove();
            //doc.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Remove();
            //XDocument xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
            //new XElement("project",
            //    new XAttribute("number", project.ProjectNumber),
            //    new XElement("store",zx
            //        new XAttribute("number", project.StoreNumber)
            //    ),
            XDocument transform = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("configuration"));
            //remove all comments?TODO

            
            //if the baseFile does not have a tag in it then Insert
            //else if the baseFile does have a tag but the nodes do not match exactly then Replace
            IEnumerable<XElement> baseElements = baseFile.Elements().Where(x => x.Name.LocalName != "appSettings");
            IEnumerable<XElement> currentElements = currentFile.Elements().Where(x => x.Name.LocalName != "appSettings");
            IEnumerable<string> baseTagNames = baseElements.Select(ele => ele.Name.LocalName);
            //TODO: Account for names with multiple tags (like <location>)  
            //FOR NOW: ignore location nodes

            //for each node in container of currentElements
                //HACK For now.... TODO: Get namespaces working
                //if the element name is location, skip it
                //if the element is NOT in the base
                    //add as insert
                //else if the element name IS in the base AND the nodes do not match
                    //add as Replace
            foreach (XElement nodeToAdd in currentElements)
            {
                if (nodeToAdd.Name.LocalName == "location")
                {
                    continue;
                }
                
                if (!baseTagNames.Contains(nodeToAdd.Name.LocalName))
                {
                    //add as insert
                    XAttribute insert = new XAttribute("HackTransformHack", "Insert");
                    nodeToAdd.Add(insert);
                    transform.Root.AddFirst(nodeToAdd);
                }
                else
                {
                    XElement baseNode = baseElements.Where(x => x.Name.LocalName == nodeToAdd.Name.LocalName).FirstOrDefault();
                    if (!XNode.DeepEquals(baseNode, nodeToAdd))
                    {
                        //add as replace
                        XAttribute insert = new XAttribute("HackTransformHack", "Replace");
                        nodeToAdd.Add(insert);
                        transform.Root.AddFirst(nodeToAdd);
                    }
                }
            }

            XElement baseAppSettings = baseFile.Elements().Where(x => x.Name.LocalName == "appSettings").FirstOrDefault();
            XElement currentAppSettings = currentFile.Elements().Where(x => x.Name.LocalName == "appSettings").FirstOrDefault();
            XElement transformAppSettings = CreateAppSettings(baseAppSettings, currentAppSettings);

            transform.Root.AddFirst(transformAppSettings);



            string copy = transform.ToString();
            copy = Regex.Replace(copy, "HackTransformHack", "xdt:Transform");
            copy = Regex.Replace(copy, "HackLocatorHack", "xdt:Locator");
            copy = Regex.Replace(copy, "<configuration>", "<configuration xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\">");
            //<configuration
            //configuration xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform"

            XDocument returnDoc = XDocument.Parse(copy);
            //Option 1: Location will always be in base --> ignore in transform if node name is location
            //Option 2: do a bunch of logic to figure out double ups --> not much time for this

            //IEnumerable<XElement> baseFile.Elements();
            return returnDoc;
        }

        private XElement CreateAppSettings(XElement baseAppSettings, XElement currentAppSettings)
        {
            /*  <?xml version="1.0" encoding="utf-8"?>
                <!-- For more information on using web.config transformation visit http://go.microsoft.com/fwlink/?LinkId=125889 -->
                <configuration xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform">
             * 
             * Easy Examples for each tranform method type
             * <httpRuntime maxRequestLength="102400" xdt:Transform="Insert" />
             * <compilation defaultLanguage="c#" debug="false" xdt:Transform="Replace" />
             * <add key="BrandPATH" value="\\mdcvbrand01p\Branding\LPL_1_0\A_V1_0" xdt:Transform="SetAttributes" xdt:Locator="Match(key)" />
             * 
             *      IEnumerable<XElement> baseNonAppSettingsElements = baseFile.Elements().Where(x => x.Name.LocalName != "appSettings");
                    IEnumerable<string> nextAppKeyNames = nextFile.Elements().Where(x => x.Name.LocalName != "appSettings").Select(ele => ele.Name.LocalName);
                    baseNonAppSettingsElements.Where(x => !nextAppKeyNames.Contains(x.Name.LocalName)).Remove();
            */
            //create new AppSettings tag for transform file 
            XElement transformTags = new XElement("appSettings");
            //get a collection of keynames from currentAppSettings file
            IEnumerable<string> baseKeyNames = baseAppSettings.Descendants().Where(x => x.Name.LocalName == "add")
                                        .Attributes().Where(x => x.Name.LocalName == "key")
                                        .Select(attr => attr.Value);
            IEnumerable<XElement> keysToAddToTransform = currentAppSettings.Elements().Where(x => x.Name.LocalName == "add");
            IEnumerable<XElement> baseKeysToCompare = baseAppSettings.Elements().Where(x => x.Name.LocalName == "add");
            //XNamespace ns = "http://schemas.microsoft.com/XML-Document-Transform";
            //foreach key in the current config
            foreach (XElement addKeyValueTag in keysToAddToTransform)
            {
                XAttribute addKey = addKeyValueTag.Attributes("key").FirstOrDefault();
                XAttribute addValue = addKeyValueTag.Attributes("value").FirstOrDefault();
                if (!baseKeyNames.Contains(addKey.Value))
                {
                    //add as insert tag
                    XAttribute insert = new XAttribute("HackTransformHack","Insert");
                    addKeyValueTag.Add(insert);
                    transformTags.Add(addKeyValueTag);
                }
                else
                {
                    //if the base key value doesn't match this key value, add as setAttributes
                    XElement baseAddKey = baseKeysToCompare.Where(x => x.Attributes("key").FirstOrDefault().Value == addKey.Value).FirstOrDefault();
                    XAttribute baseAddValue = baseAddKey.Attributes("value").FirstOrDefault();
                    if (addValue.Value != baseAddValue.Value)
                    {
                        /*
                         * XNamespace ns = "http://purl.org/dc/elements/1.1/";
                    var document = new XDocument(
                                new XDeclaration("1.0", "utf-8", null),
                                new XElement("rss", new XAttribute(XNamespace.Xmlns + "dc", ns)
                                            new XElement("channel",
                                                        new XElement("title", "test"),
                                                        new XElement(ns + "creator", "test")
                         */
                        //add as setAttributes
                        XAttribute xFormMethod = new XAttribute("HackTransformHack", "SetAttributes");
                        XAttribute locator = new XAttribute("HackLocatorHack", "Match(key)");
                        addKeyValueTag.Add(xFormMethod);
                        addKeyValueTag.Add(locator);
                        transformTags.Add(addKeyValueTag);
                    }
                }
            }

            //if a key does not exist in the baseAppSettings
                //add an Insert attribute
            //else if a key exists, but the values do not match
                //add a setAttributes attribute with match locator attribute
            //else if a key exists, and the values do match
                //do nothing, it will get taken care of by the base when leftDoc
            //get a container of keynames
            return transformTags;
        }
    }
}
