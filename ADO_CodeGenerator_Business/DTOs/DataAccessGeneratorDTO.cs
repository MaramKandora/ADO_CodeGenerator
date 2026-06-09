using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayerCodeGenerator;
using BusinessLayerCodeGenerator.Enums;

namespace GeneratorBusiness.DTOs
{
    public class DataAccessGeneratorDTO
    {
        public string FolderName;
        public string DataAccessProjectPath;
        //"Server=.; Database=LibraryDB; User Id=sa; Password=123456; TrustServerCertificate = true;"
        public string ConnectionString = "Data Source=(local);Initial Catalog=MySchool;Integrated Security=True;Asynchronous Processing=true;";

        public enDataAccessMode DataAccessMode = enDataAccessMode.Queries;

        public enTargetPlatform TargetPlatform;

        public string PresentationProjectPath;

        //public string 
    }
}
