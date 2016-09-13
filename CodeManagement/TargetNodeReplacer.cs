using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CodeManagement
{
    class TargetNodeReplacer
    {
        const string importProjCommand = @"$(MSBuildProjectDirectory)\..\AdditionalFiles\AffirmBuild\AffirmBuild.Targets";

        //TODO: Move this out as an input or member initialized by constructor
        string changedFileOutputPath = @"C:\TestFiles\CodeManagement\outputPath\ReplaceNodeTestOutput.txt";
        string errorFileOutputPath = @"C:\TestFiles\CodeManagement\outputPath\ReplaceNodeErrorTestOutput.txt";
        string unchangedFileOutputPath = @"C:\TestFiles\CodeManagement\outputPath\ReplaceNodeUnchangedOutput.txt";


        string savedFilePath = @"C:\Users\amckenzie\Documents\TestFiles\ADMServer2008\";

        public void ReplaceTargetsWithImports(string fileName)
        {
            string filePath = savedFilePath + fileName;
            //get file into an XDocument
            XDocument projectFile = null;
            using (System.IO.TextReader reader = new System.IO.StreamReader(filePath))
            {
                projectFile = XDocument.Parse(reader.ReadToEnd());
            }


            //get the target node --- TODO: Make sure this node is unique, may need to filter out others
            //check for multiple target nodes TODO 


            //TODO: should probably filter out files that do not meet this criteria before we send to this method
            //if afterbuild predicate and where something references the nuget condition
            IEnumerable<XElement> possibleTargetNodes = projectFile.Root.Elements()
                                                        .Where(x => x.Name.LocalName == "Target" 
                                                            && x.Attributes("Condition").Select(a => a.Value).FirstOrDefault() == @"$(Configuration)==NuGet"
                                                            && x.Attributes("Name").Select(a => a.Value).FirstOrDefault() == "AfterBuild");

            //  <Target Name="AfterBuild" Condition="$(Configuration)==NuGet">

            //if count of node collection is zero, we don't need to bother
            if (possibleTargetNodes.Count() > 0)
            {
                using (System.IO.TextWriter errorWriter = new System.IO.StreamWriter(errorFileOutputPath, true))
                using (System.IO.TextWriter goodWriter = new System.IO.StreamWriter(changedFileOutputPath, true))
                {
                    //output to a file and do nothing else
                    if (possibleTargetNodes.Count() > 1)
                    {
                        //output to file and flag as a problem
                        errorWriter.WriteLine(filePath);
                    }
                    else
                    {
                        //output to "replaced" file tracker
                        goodWriter.WriteLine(filePath);
                        XElement targetNode = possibleTargetNodes.FirstOrDefault();
                        ////build import node
                        XElement importNode = BuildImportNode(importProjCommand);
                        //add importnode after the target node
                        targetNode.AddBeforeSelf(importNode);
                        //comment out the target node
                        targetNode.ReplaceWith(new XComment(targetNode.ToString()));

                        //string targetRemovedNamespace = targetNode.ToString();
                        //targetRemovedNamespace = Regex.Replace(targetRemovedNamespace, "xmlns=\".*\"", "");

                        //targetNode = XElement.Parse(targetRemovedNamespace);
                        //save the document to the same file path
                        string removedNullNamespace = projectFile.ToString();
                        removedNullNamespace = Regex.Replace(removedNullNamespace, "xmlns=\"\"", "");
                        XDocument docToSave = XDocument.Parse(removedNullNamespace);
                        docToSave.Save(filePath);
                    }
                }
            }
            else
            {
                //for now we will track files that did not contain this node
                using (System.IO.TextWriter unchangedWriter = new System.IO.StreamWriter(unchangedFileOutputPath,true))
                {
                    unchangedWriter.WriteLine(filePath);
                }
            }
        }

        private XElement BuildImportNode(string importProjCommand)
        {
            XElement importNode = new XElement("Import",
                                                new XAttribute("Project", importProjCommand));
            string nameSpaceGone = importNode.ToString();

            //<Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003" ToolsVersion="4.0">

            string nameSpaceRegEx = "xmlns=\".*\"";
            nameSpaceGone = Regex.Replace(nameSpaceGone, nameSpaceRegEx, "");
            importNode = XElement.Parse(nameSpaceGone);
            return importNode;
        }
        //need to get all project files (will end with .csproj, but not .vspscc)
            //How to do this? LEVERAGE POWERSHELL

    }
}
