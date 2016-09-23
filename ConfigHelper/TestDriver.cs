using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace ConfigHelper
{
    class TestDriver
    {
        //TODO: This project is getting too big to test this way.  These tests really should be their own methods
        static void Main(string[] args)
        {
            //XDocument transform = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("configuration"));
            //ConfigBaseBuilder myBuilder = new ConfigBaseBuilder();
            ////XElement baseApps = new XElement("AppSettings",
            ////new XElement("add", new XAttribute("key", "key1"), new XAttribute("value", "value1")),
            ////new XElement("add", new XAttribute("key", "key2"), new XAttribute("value", "value2")),
            ////new XElement("add", new XAttribute("key", "key3"), new XAttribute("value", "value3")),
            ////new XElement("add", new XAttribute("key", "key4"), new XAttribute("value", "value4"))
            ////);

            ////XElement nextApps = new XElement("AppSettings",
            ////new XElement("add", new XAttribute("key", "key5"), new XAttribute("value", "value1")),
            ////new XElement("add", new XAttribute("key", "key2"), new XAttribute("value", "value5")),
            ////new XElement("add", new XAttribute("key", "key3"), new XAttribute("value", "value3")),
            ////new XElement("add", new XAttribute("key", "key6"), new XAttribute("value", "value6"))
            ////);
            ////XElement myElement = myBuilder.RemoveUnmatchedAppSettingKeysFromBase(baseApps, nextApps);



            ///************************  base Config builder stuff *****************
            // * */
            //const string baseFile = @"C:\TestFiles\MSSB\BaseConfigBuilder\Web.DEV.config";
            //const string nextFile = @"C:\TestFiles\MSSB\BaseConfigBuilder\Web.prod.config";
            //string baseConfigurationFile;
            //string nextConfigurationFile;
            //using (System.IO.TextReader baseReader = new System.IO.StreamReader(baseFile))
            //{
            //    baseConfigurationFile = baseReader.ReadToEnd();
            //}
            //using (System.IO.TextReader nextReader = new System.IO.StreamReader(nextFile))
            //{

            //    nextConfigurationFile = nextReader.ReadToEnd();
            //}
            //XDocument baseConfigXML = XDocument.Parse(baseConfigurationFile);
            //XDocument nextConfigXML = XDocument.Parse(nextConfigurationFile);
            //XDocument dummy = myBuilder.BuildBaseDocument(baseConfigXML.Root, nextConfigXML.Root);

            ///*************** End base Config builder stuff *******************/
            ////get argument input
            ////create base config and output to file
            ////create 


            ///******************** Config Transform builder stuff *********************/
            ////get base file
            ////get working config file
            //ConfigTransformBuilder myTransformBuilder = new ConfigTransformBuilder();
            //XDocument transformedConfig = myTransformBuilder.BuildDocument(baseConfigXML.Root, nextConfigXML.Root);

            ////quick hack to replace namespace alias
            
                       
            ///******************** End Config Transform builder stuff *********************/
            //const string baseFile = @"C:\TestFiles\MSSB\BaseConfigBuilder\Web.DEV.config";
            //const string nextFile = @"C:\TestFiles\MSSB\BaseConfigBuilder\Web.prod.config";
            //string baseConfigurationFile;
            //string nextConfigurationFile;
            //using (System.IO.TextReader baseReader = new System.IO.StreamReader(baseFile))
            //{
            //    baseConfigurationFile = baseReader.ReadToEnd();
            //}
            //using (System.IO.TextReader nextReader = new System.IO.StreamReader(nextFile))
            //{

            //    nextConfigurationFile = nextReader.ReadToEnd();
            //}
            //XDocument baseConfigXML = XDocument.Parse(baseConfigurationFile);
            //XDocument nextConfigXML = XDocument.Parse(nextConfigurationFile);

            //ConfigTransformBuilder transformbuilder = new ConfigTransformBuilder();
            //ConfigBaseBuilder baseBuilder = new ConfigBaseBuilder();
            ////build base config
            //XDocument baseConfigFile = baseBuilder.BuildBaseDocument(baseConfigXML.Root, nextConfigXML.Root);
            //XDocument transformFile = transformbuilder.BuildDocument(baseConfigFile.Root, nextConfigXML.Root);
            
            //pull files from directory
            //get config files only (do we really need dr-prod?)
            //save to file


            /*********************  BASE CONFIG BUILDER SERVICE STUFF *****************/

            //const string inputPath = @"C:\ADMServer2008\Engagements\2129_0001_Morgan_Stanley_AFFIRM_2_1\AdditionalFiles\Configuration\FMS_Custom";
            //const string outputPath = @"C:\TestFiles\MSSB\BaseConfigBuilder\outputPath\fmscustom";
            //string basePath = string.Format(@"{0}\{1}", outputPath, "BASE.config");

            //ConfigBuilderService builderService = new ConfigBuilderService();
            //builderService.CreateBaseFile(inputPath, outputPath);
            //List<string> environments = new List<string> { "dev", "qa", "uat", "prod" };
            //foreach (string env in environments)
            //{
            //    builderService.CreateTransform(inputPath, outputPath, basePath, env);
            //}
            
            /***************************************************************************/



            /********************  Config Compare stuff **************************************/
            //string transformPath = @"C:\TestFiles\MSSB\BaseConfigBuilder\outputPath\fmscustom\prodtransform.config";
            //string oldConfigPath = @"C:\ADMServer2008\Engagements\2129_0001_Morgan_Stanley_AFFIRM_2_1\AdditionalFiles\Configuration\FMS_Custom\FMSProcess.exe.prod.config";
            ////XDocument transformed = null;
            ////XDocument oldConfig = null;
            ////using (System.IO.TextReader transformReader = new System.IO.StreamReader(transformPath))
            ////using (System.IO.TextReader oldConfigReader = new System.IO.StreamReader(oldConfigPath))
            ////{
            ////    transformed = XDocument.Parse(transformReader.ReadToEnd());
            ////    oldConfig = XDocument.Parse(oldConfigReader.ReadToEnd());
            ////}
            //////pull out comments 
            ////oldConfig.Root.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Remove();
            ////transformed.Root.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Remove();

            ////try to sort the child elements to test deepEquals
            //oldConfig = XDocument.Parse(oldConfig.Root.Elements().OrderBy(x => x.Name.LocalName).FirstOrDefault().ToString());
            //transformed = XDocument.Parse(transformed.Root.Elements().OrderBy(x => x.Name.LocalName).FirstOrDefault().ToString());
            //.OrderBy(e=>e.Attribute("name").Value)
            

            //get all files from 
            ConfigCompare comparer = new ConfigCompare();

            List<string> environments = new List<string> { "dev", "qa", "uat", "prod" };

            string targetFolder = @"C:\TestFiles\MSSB\BaseConfigBuilder\outputPath\dm";
            string workingFolder = @"C:\ADMServer2008\Engagements\2129_0001_Morgan_Stanley_AFFIRM_2_1\AdditionalFiles\Configuration\DistributorMaster";
            string resultOutputPath = @"C:\TestFiles\MSSB\BaseConfigBuilder\outputPath\resultFromTransforms.txt";

            foreach (string env in environments)
            {
                //create working config path
                string workingConfigPath = string.Format(@"{0}\Web.{1}.config", workingFolder, env);
                //create transformed config path
                string transformedConfigPath = string.Format(@"{0}\Transform.{1}.config", targetFolder, env);

                using (System.IO.TextWriter resultWriter = new System.IO.StreamWriter(resultOutputPath, true))
                {
                    if (comparer.CompareConfigs(workingConfigPath, transformedConfigPath))
                    {
                        resultWriter.WriteLine("{0} - MATCHED {1}", transformedConfigPath, workingConfigPath);
                    }
                    else
                    {
                        resultWriter.WriteLine("{0} - DOES NOT MATCH!!!! {1}", transformedConfigPath, workingConfigPath);
                    }
                }
            }
            



            //ConfigBuilderService builderService = new ConfigBuilderService();
            //builderService.CreateBaseFile(inputPath, outputPath);
            //List<string> environments = new List<string> { "dev", "qa", "uat", "prod" };
            //foreach (string env in environments)
            //{
            //    builderService.CreateTransform(inputPath, outputPath, basePath, env);
            //}
           
            /************************************************************************************/
        }
    }
}
