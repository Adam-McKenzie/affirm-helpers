using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace ConfigHelper
{
    class ConfigBuilderService
    {
        public void CreateBaseFile(string inputPath, string outputPath)
        {
            //get all files from the input path
            List<string> files = new List<string>(Directory.GetFiles(inputPath));
            //TODO: ugly - move to different container of configs only
            List<string> configFiles = new List<string>();
            //filter out only .config files that we need
            string baseFileName = null;
            string currentBaseConfig = null;
            //string devFile = null;
            foreach(string file in files)
            {
                if(file.ToLower().Contains(".config") && !file.ToLower().Contains("dr"))
                {
                    if (file.ToLower().Contains("dev"))
                    {
                        baseFileName = file;
                    }
                    else
                    {
                        //add to list of configs to compare to base
                        configFiles.Add(file);
                    }
                }
            }
            //grab DEV config as base
            if (!string.IsNullOrEmpty(baseFileName))
            {
                using (System.IO.TextReader baseReader = new System.IO.StreamReader(baseFileName))
                {
                    currentBaseConfig = baseReader.ReadToEnd();
                }
            }
            XDocument baseConfigDoc = XDocument.Parse(currentBaseConfig);
            foreach (string file in configFiles)
            {
                XDocument nextConfig;
                ConfigBaseBuilder baseBuilder = new ConfigBaseBuilder();
                using (System.IO.TextReader nextConfigReader = new System.IO.StreamReader(file))
                {
                    nextConfig = XDocument.Parse(nextConfigReader.ReadToEnd());
                }
                //get updated baseconfig
                baseConfigDoc = baseBuilder.BuildBaseDocument(baseConfigDoc.Root, nextConfig.Root);
            }
            
            //get output file name to save to 
            string savePath = string.Format(@"{0}\{1}",outputPath, "BASE.config");
            Console.WriteLine(string.Format("Base Config is built, saving to path {0}", savePath));
            //save document to outputPath with the correct filename
            baseConfigDoc.Save(savePath);
        }


        public void CreateTransform(string inputPath, string outputPath, string baseFile, string environment)
        {
            //create the base document

            XDocument baseDoc;
            using (System.IO.TextReader baseReader = new System.IO.StreamReader(baseFile))
            {
                baseDoc = XDocument.Parse(baseReader.ReadToEnd());
            }
            //grab the file we need from the directory
            List<string> files = new List<string>(Directory.GetFiles(inputPath));

            XDocument transformDoc = null;
            //find the env file we need
            foreach(string file in files)
            {
                if (file.ToLower().Contains("dr"))
                {
                    continue;
                }
                if (file.ToLower().Contains(".config") && file.ToLower().Contains(environment))
                {
                    //read this config
                    using (System.IO.TextReader workingConfig = new System.IO.StreamReader(file))
                    {
                        transformDoc = XDocument.Parse(workingConfig.ReadToEnd());
                    }
                    break;
                }
            }
            ConfigTransformBuilder transformBuilder = new ConfigTransformBuilder();
            transformDoc = transformBuilder.BuildDocument(baseDoc.Root, transformDoc.Root);
            string savePath = string.Format(@"{0}\{1}{2}", outputPath, environment.ToUpper(),".config");
            Console.WriteLine(string.Format("Transform Config is built, saving to path {0}", savePath));
            //save document to outputPath with the correct filename
            transformDoc.Save(savePath);
        }
    }
}
