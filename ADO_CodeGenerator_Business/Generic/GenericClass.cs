using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorBusiness
{
    public class GenericClass
    {
        protected static string GetNamespace(string folderName, string ProjectPath)
        {
         
            StringBuilder NameSpace = new StringBuilder(GetProjectNameFromPath(ProjectPath));

            if (string.IsNullOrWhiteSpace(folderName))
                return NameSpace.ToString();

            //Attach folder path to the namespace

            if (!folderName.Contains("\\"))
            {
                return NameSpace.Append('.' + folderName).ToString();
            }

            string[] SplitFolderPath = folderName.Split(new[] { '\\' });

            foreach (string Folder in SplitFolderPath)
            {
                NameSpace.Append('.' + Folder);
            }

            return NameSpace.ToString();
        }
        public static string GetProjectNameFromPath(string projectPath)
        {

            if (string.IsNullOrWhiteSpace(projectPath))
                return null;

            string[] SplitPath = projectPath.Split(new[] { '\\' });

            return SplitPath[SplitPath.Length - 1];
        }

        protected string GetClassSignature(string ClassName)
        {
            return "public class " + ClassName;
        }
    }
}
