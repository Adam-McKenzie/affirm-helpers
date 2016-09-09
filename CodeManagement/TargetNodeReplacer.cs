using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CodeManagement
{
    class TargetNodeReplacer
    {
        const string importProjCommand = @"$(MSBuildProjectDirectory)\..\AdditionalFiles\AffirmBuild\AffirmBuild.Targets";

        public void ReplaceTargetWithImport(string projectCommand, string filePath)
        {
            //get file into an XDocument
            XDocument projectFile = null;
            using (System.IO.TextReader reader = new System.IO.StreamReader(filePath))
            {
                projectFile = XDocument.Parse(reader.ReadToEnd());
            }


            //get the target node --- TODO: Make sure this node is unique, may need to filter out others
            //check for multiple target nodes TODO 


            //TODO: should probably filter out files that do not meet this criteria 
            //if afterbuild predicate and where something references the nuget condition
            IEnumerable<XElement> possibleTargetNodes = projectFile.Root.Elements()
                                                        .Where(x => x.Name.LocalName == "Target" && x.Attributes("Condition").Select(a => a.Value).FirstOrDefault() == @"$(Configuration)==NuGet");

            //  <Target Name="AfterBuild" Condition="$(Configuration)==NuGet">

            //if count of node collection is zero, we don't need to bother
            if (possibleTargetNodes.Count() > 0)
            {
                if (possibleTargetNodes.Count() > 1)
                {
                    
                }
                else
                {
                    //output to 
                    XElement targetNode = null;
                    //build import node
                    XElement importNode = BuildImportNode(projectCommand);

                    //replace target with import
                    targetNode = importNode;
                    //save the document to the same file path
                    projectFile.Save(filePath);
                }
            }
            
        }

        private XElement BuildImportNode(string importProjCommand)
        {
            XElement importNode = new XElement("Import",
                                                new XAttribute("Project", importProjCommand));
            return importNode;
        }
        //need to get all project files (will end with .csproj, but not .vspscc)
            //How to do this? LEVERAGE POWERSHELL

    }
}
