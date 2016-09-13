using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeManagement
{
    class CMTestDriver
    {
        const string replaceNodeInputPath = @"C:\TestFiles\CodeManagement\outputPath\fileNames.txt";
        public void TestReplaceNodeBehavior(string inputPath)
        {
            TargetNodeReplacer replacer = new TargetNodeReplacer();
            string fileName = null;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(inputPath))
            {
                while (!reader.EndOfStream)
                {
                    //get each file and replace target node if applicable
                    fileName = reader.ReadLine();
                    replacer.ReplaceTargetsWithImports(fileName);
                }
            }
        }
        static void Main(string[] args)
        {
            CMTestDriver driver = new CMTestDriver();
            driver.TestReplaceNodeBehavior(replaceNodeInputPath);
        }
    }
}
