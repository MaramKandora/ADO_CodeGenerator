using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BusinessLayerCodeGenerator;
using BusinessLayerCodeGenerator.Business;
using BusinessLayerCodeGenerator.Enums;
using BusinessLayerCodeGenerator.Utility;
using GeneratorBusiness.DTOs;
using Microsoft.Data.SqlClient;

namespace GeneratorBusiness
{
    public class GenerateDataAccess : GenericClass
    {
        StringBuilder _Template = new StringBuilder();
        Dictionary<string, Dictionary<string, string>> _EntityNameToProperties = new Dictionary<string, Dictionary<string, string>>();
        // propertyName, DataType
        Dictionary<string, Dictionary<string,bool>> _EntityToPrimaryKey = new Dictionary<string, Dictionary<string, bool>>();
        // PkName , IsIdentity
        Dictionary<string, Dictionary<string, string>> _EntityToDtosClassNames = new Dictionary<string, Dictionary<string, string>>();
        //DtoFunction, DtoName
        Dictionary<string, string> _EntityToTableName = new Dictionary<string, string>();
        DataAccessGeneratorDTO _dto;

        string _ProjectName;

        string _EntitiesFolderName = "Models";
        string _DtosFolderName = "DTOs";
        public GenerateDataAccess(DataAccessGeneratorDTO dto)
        {
            _dto = dto;
            // _ProjectName = GetNamespace(_dto.FolderName, _dto.ProjectPath);
            _ProjectName = GetProjectNameFromPath(_dto.DataAccessProjectPath);


        }

        private void SaveEntitiesInFiles(Dictionary<string, string> EntityNameToDefinition, string FolderName)
        {

            if (!Directory.Exists(_dto.DataAccessProjectPath))
            {
                return;
            }
            string EntitiesDirectory = $"{_dto.DataAccessProjectPath}\\{FolderName}";
            Directory.CreateDirectory(EntitiesDirectory);

            foreach (var Pair in EntityNameToDefinition)
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(EntitiesDirectory, Pair.Key + ".cs")))
                {
                    writer.Write(Pair.Value);
                }
            }


        }

        private string ClassTextHelper(string NameSpace, string ClassName, List<string> LibrariesToInclude = null)
        {
            StringBuilder St = new StringBuilder();
            St.AppendLine("using System;");
            if (LibrariesToInclude != null)
            {
                foreach (var Library in LibrariesToInclude)
                {
                    St.AppendLine($"using {Library};");
                }
            }

            St.AppendLine($"\nnamespace {NameSpace}");
            St.AppendLine("{");
            St.AppendLine($"    public partial class {ClassName}"); //entityName
            St.AppendLine("    {");

            return St.ToString();
        }

        private string CloseClassTextHelper()
        {
            StringBuilder St = new StringBuilder();
            St.AppendLine("\n    }");
            St.AppendLine("}");

            return St.ToString();
        }

        private string PropertyTextHelper(string PropertyName, string DataType)
        {
            return $"        public {DataType} {PropertyName};";
        }


        void createEntitiesText()
        {
            Dictionary<string, string> EntitiesNameToDefinition = new Dictionary<string, string>();



            foreach (var EntityPair in _EntityNameToProperties)
            {
                _Template.Clear();

                _Template.AppendLine(ClassTextHelper($"{_ProjectName}.{_EntitiesFolderName}", EntityPair.Key));

                foreach (var PropertyPair in EntityPair.Value)
                {
                    _Template.AppendLine(PropertyTextHelper(PropertyPair.Key, PropertyPair.Value));
                    //result: public 'DataType' 'PropertyName'
                }
                _Template.Append(CloseClassTextHelper());


                EntitiesNameToDefinition.Add(EntityPair.Key, _Template.ToString());

            }

            SaveEntitiesInFiles(EntitiesNameToDefinition, _EntitiesFolderName);

        }
        private void GenerateEntities()
        {
            using (SqlConnection connection = new SqlConnection(_dto.ConnectionString))
            {
                connection.Open();

                var dtTables = connection.GetSchema("Tables");
                DataTable dtColumns;
                string TableName;

                String[] ColumnRestrictions = new String[4];


                foreach (DataRow row in dtTables.Rows)
                {

                    Dictionary<string, string> Properties = new Dictionary<string, string>();
                    TableName = row["TABLE_NAME"].ToString();
                    if (TableName == "sysdiagrams")
                        continue; //ignore system table

                    string EntityName = Utility.GetSingularName(TableName);
                    _EntityToTableName.Add(EntityName, TableName);

                    ColumnRestrictions[2] = TableName;
                    dtColumns = connection.GetSchema("Columns", ColumnRestrictions);


                    foreach (DataRow row2 in dtColumns.Rows)
                    {
                        Properties.Add(row2["COLUMN_NAME"].ToString(), Utility.MapSqlToCSharpDataType(row2["DATA_TYPE"].ToString()));

                    }


                    _EntityNameToProperties.Add(EntityName, Properties); // EntityName, EntityPorperties 


                }
                createEntitiesText();

            }


        }


        private void SavePrimaryKeysPerEntity()
        {

            using SqlConnection connection = new SqlConnection(_dto.ConnectionString);

            connection.Open();

            foreach (var Pair in _EntityToTableName)
            {
                string Query = @$"SELECT
                                    c.name AS ColumnName,
                                    c.is_identity
                                FROM sys.indexes i
                                JOIN sys.index_columns ic
                                    ON i.object_id = ic.object_id
                                   AND i.index_id = ic.index_id
                                JOIN sys.columns c
                                    ON ic.object_id = c.object_id
                                   AND ic.column_id = c.column_id
                                WHERE i.is_primary_key = 1
                                  AND i.object_id = OBJECT_ID('{Pair.Value}');"; //return primaryKeys of Table
                using SqlCommand Command = new SqlCommand(Query, connection);

                using SqlDataReader reader = Command.ExecuteReader();
               

                Dictionary<string, bool> PrimaryKeyToIsIdentity = new Dictionary<string, bool>();
                while(reader.Read()) 
                {

                    if (reader.GetBoolean(reader.GetOrdinal("is_identity")))
                    {
                        PrimaryKeyToIsIdentity.Add(reader.GetString(reader.GetOrdinal("ColumnName")), true);
                    }
                    else
                    {
                        PrimaryKeyToIsIdentity.Add(reader.GetString(reader.GetOrdinal("ColumnName")), false);
                    }
                    
                }

                _EntityToPrimaryKey.Add(Pair.Key, PrimaryKeyToIsIdentity);
            }



        }

        void GenerateDtoClasses(string EntityName, string FolderName)
        {
            if (!Directory.Exists(_dto.DataAccessProjectPath))
            {
                return;
            }


            string EntityDtosDirectory = $"{_dto.DataAccessProjectPath}\\{_DtosFolderName}\\{FolderName}";
            Directory.CreateDirectory(EntityDtosDirectory);

            string NameSpace = $"{_ProjectName}.{_DtosFolderName}.{FolderName}";


            Dictionary<string, string> DtoFunctionToClassName = new Dictionary<string, string>();
            DtoFunctionToClassName.Add(nameof(enDtoFunction.Get), $"{EntityName}DTO");
            DtoFunctionToClassName.Add(nameof(enDtoFunction.Add), $"Add{EntityName}DTO");
            DtoFunctionToClassName.Add(nameof(enDtoFunction.Update), $"Update{EntityName}DTO");
            //DtoFunctionToClassName.Add(nameof(enDtoFunction.Delete), $"Delete{EntityName}DTO");

            _EntityToDtosClassNames.Add(EntityName, DtoFunctionToClassName);


            StringBuilder GetDtoText = new StringBuilder(ClassTextHelper(NameSpace, DtoFunctionToClassName[nameof(enDtoFunction.Get)]));
            StringBuilder AddDtoText = new StringBuilder(ClassTextHelper(NameSpace, DtoFunctionToClassName[nameof(enDtoFunction.Add)]));
            StringBuilder UpdateDtoText = new StringBuilder(ClassTextHelper(NameSpace, DtoFunctionToClassName[nameof(enDtoFunction.Update)]));
           // StringBuilder DeleteDtoText = new StringBuilder(ClassTextHelper(NameSpace, DtoFunctionToClassName[nameof(enDtoFunction.Delete)]));

            Dictionary<string, string> Properties = _EntityNameToProperties[EntityName];
            Dictionary<string,bool> PrimaryKeyToIsIdentity = _EntityToPrimaryKey[EntityName];

            foreach (var PropertyPair in Properties)
            {
                GetDtoText.AppendLine(PropertyTextHelper(PropertyPair.Key, PropertyPair.Value));

                //check if property is a primary key and not identity
                if (PrimaryKeyToIsIdentity.Keys.ToList().Exists(x => x == PropertyPair.Key))
                {
                    if (PrimaryKeyToIsIdentity[PropertyPair.Key] == false)
                        AddDtoText.AppendLine(PropertyTextHelper(PropertyPair.Key, PropertyPair.Value));
                }
                else
                {
                    AddDtoText.AppendLine(PropertyTextHelper(PropertyPair.Key, PropertyPair.Value));
                    UpdateDtoText.AppendLine(PropertyTextHelper(PropertyPair.Key, PropertyPair.Value));
                }

            }
            GetDtoText.Append(CloseClassTextHelper());
            AddDtoText.Append(CloseClassTextHelper());
            UpdateDtoText.Append(CloseClassTextHelper());
            //DeleteDtoText.Append(CloseClassTextHelper());


            Dictionary<string, string> DTONametoText = new Dictionary<string, string>();
            DTONametoText.Add($"{EntityName}DTO", GetDtoText.ToString());
            DTONametoText.Add($"Add{EntityName}DTO", AddDtoText.ToString());
            DTONametoText.Add($"Update{EntityName}DTO", UpdateDtoText.ToString());
            //DTONametoText.Add($"Delete{EntityName}DTO", DeleteDtoText.ToString());

            foreach (var Pair in DTONametoText)
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(EntityDtosDirectory, Pair.Key + ".cs")))
                {
                    writer.Write(Pair.Value);
                }
            }

        }
        private void GenerateDTOs()
        {
            foreach (var Pair in _EntityToTableName)
            {
                GenerateDtoClasses(Pair.Key, Pair.Value);
            }


        }

        private void UpdateFunctionCodeBasedOnDataAccessMode(enDataAccessMode Mode, string EntityName
            , Dictionary<string, string> PropertyToVariable, Dictionary<string, string> PrimaryKeysToVariable)
        {
            string PropertiesToUpdate = String.Join(", ",
                            _EntityNameToProperties[EntityName].Keys.Where(E =>
                           !_EntityToPrimaryKey[EntityName].Keys.Contains(E)
                            ));


            string VariablesString = "";
            foreach (var pair in PropertyToVariable)
            {
                VariablesString += String.Join(" = ", new List<string> { pair.Key, pair.Value });
                VariablesString += ", ";
            }
            VariablesString = VariablesString.Remove(VariablesString.Length - 2, 2);

            string PrimaryKeysVariableString = "";
            foreach (var pair in PrimaryKeysToVariable)
            {
                PrimaryKeysVariableString += String.Join(" = ", new List<string> { pair.Key, pair.Value });
                PrimaryKeysVariableString += ", ";
            }
            PrimaryKeysVariableString = PrimaryKeysVariableString.Remove(PrimaryKeysVariableString.Length - 2, 2);

            switch (Mode)
            {
                case enDataAccessMode.Queries:
                    {


                        _Template.Append($"            string Query = @\"Update {_EntityToTableName[EntityName]} (");
                        _Template.AppendLine($"{PropertiesToUpdate})");
                        _Template.AppendLine($"                             SET {VariablesString}");
                        _Template.AppendLine($"                             WHERE {PrimaryKeysVariableString};\";\n");
                        _Template.AppendLine($"            using SqlCommand Command = new SqlCommand(Query, Connection);");
                        foreach (var pair in PropertyToVariable)
                        {
                            _Template.Append($"            Command.Parameters.AddWithValue(\"{pair.Value}\", ");
                            _Template.AppendLine($"dto.{pair.Key});");
                        }
                        foreach (var pair in PrimaryKeysToVariable)
                        {
                            _Template.Append($"            Command.Parameters.AddWithValue(\"{pair.Value}\", ");
                            _Template.AppendLine($"{pair.Key});");
                        }
                        _Template.AppendLine($"            try");
                        _Template.AppendLine($"            {{");
                        _Template.AppendLine($"                 Connection.Open();\n");
                        _Template.AppendLine($"                 AffectedRows = await Command.ExecuteNonQueryAsync();\n");

                        _Template.AppendLine($"            }}");
                        _Template.AppendLine($"            catch");
                        _Template.AppendLine($"            {{");
                        _Template.AppendLine($"            }}");

                        break;
                    }

                case enDataAccessMode.StoredProcedures:
                    {

                        break;
                    }


            }
        }
        private void AddFunctionCodeBasedOnDataAccessMode(enDataAccessMode Mode, string EntityName, bool AnyIdentityPK)
        {
            var PropertiesToAddList = _EntityNameToProperties[EntityName].Keys.Where(E =>

                !_EntityToPrimaryKey[EntityName].Any(pk => pk.Key == E && pk.Value == true)
            ).ToList();

            

            string PropertiesToAddString = String.Join(", ",
                            PropertiesToAddList);

            string VariablesString = String.Join(", ", $"@{PropertiesToAddString}");

            switch (Mode)
            {
                case enDataAccessMode.Queries:
                    {


                        _Template.Append($"            string Query = @\"INSERT INTO {_EntityToTableName[EntityName]} (");
                        _Template.AppendLine($"{PropertiesToAddString})");
                        _Template.Append($"                             VALUES ({VariablesString})");
                        if (AnyIdentityPK)
                            _Template.AppendLine($"\n                             SELECT SCOP_IDENTITY();\";\n");
                        else
                            _Template.AppendLine($";\";\n");

                        _Template.AppendLine($"            using SqlCommand Command = new SqlCommand(Query, Connection);");

                        foreach (var Property in PropertiesToAddList)
                        {
                            _Template.Append($"            Command.Parameters.AddWithValue(\"@{Property}\", ");
                            _Template.AppendLine($"dto.{Property});");
                        }
                        _Template.AppendLine($"            try");
                        _Template.AppendLine($"            {{");
                        _Template.AppendLine($"                 Connection.Open();\n");
                        if (AnyIdentityPK)
                        {
                            _Template.AppendLine($"                 var result = await Command.ExecuteScalarAsync();\n");
                            _Template.AppendLine($"                 if (result != null && int.TryParse(result.ToString(), out int InsertedId))");
                            _Template.AppendLine($"                 {{");
                            _Template.AppendLine($"                     NewId = InsertedId;");
                            _Template.AppendLine($"                 }}\n");
                        }
                        else
                        {
                            _Template.AppendLine($"                 AffectedRows = await Command.ExecuteNonQueryAsync();\n");

                        }
                        _Template.AppendLine($"            }}");
                        _Template.AppendLine($"            catch");
                        _Template.AppendLine($"            {{");
                        _Template.AppendLine($"            }}");

                        break;
                    }

                case enDataAccessMode.StoredProcedures:
                    {
                        //TODO:
                        break;
                    }


            }
        }
        private void GetFunctionCodeBasedOnDataAccessMode(enDataAccessMode Mode, string EntityName, string GetDto)
        {

            List<string> PKVariablesList = new List<string>();

            foreach (string PrimaryKeyName in _EntityToPrimaryKey[EntityName].Keys.ToList())
            {
                PKVariablesList.Add(String.Join(" = ", new[] { PrimaryKeyName, $"@{PrimaryKeyName}" }));
            }

            string PKVariablesString = String.Join(", ", PKVariablesList);

            switch (Mode)
            {
                case enDataAccessMode.Queries:
                    {


                        _Template.AppendLine($"            string Query = @\"SELECT * FROM {_EntityToTableName[EntityName]}");
                        _Template.AppendLine($"                             WHERE {PKVariablesString};\";\n");
                        _Template.AppendLine($"            using SqlCommand Command = new SqlCommand(Query, Connection);");

                        foreach (var PK in _EntityToPrimaryKey[EntityName].Keys.ToList())
                        {
                            _Template.AppendLine($"            Command.Parameters.AddWithValue(\"@{PK}\", {PK});");

                        }
                        _Template.AppendLine($"            try");
                        _Template.AppendLine($"            {{");
                        _Template.AppendLine($"                 Connection.Open();\n");
                        _Template.AppendLine($"                 SqlDataReader Reader = await Command.ExecuteReaderAsync();\n");
                        _Template.AppendLine($"                 if (Reader.Read())");
                        _Template.AppendLine($"                 {{");
                        _Template.AppendLine($"                     dto =");
                        _Template.AppendLine($"                         new {GetDto}");
                        _Template.AppendLine($"                         {{");
                        foreach (var PropertyPair in _EntityNameToProperties[EntityName])
                        {

                            _Template.AppendLine($"                             {PropertyPair.Key} = Reader.{Utility.MapDataTypeToReaderMethod(PropertyPair.Value)}(Reader.GetOrdinal(\"{PropertyPair.Key}\")),");


                        }
                        _Template.AppendLine($"                         }};");
                        _Template.AppendLine($"                 }}\n");
                        _Template.AppendLine($"            }}");
                        _Template.AppendLine($"            catch");
                        _Template.AppendLine($"            {{");
                        _Template.AppendLine($"            }}");
                       // _Template.AppendLine($"                }}");
                        break;
                    }

                case enDataAccessMode.StoredProcedures:
                    {

                        break;
                    }

            }
        }

        private void DeleteFunctionCodeBasedOnDataAccessMode(enDataAccessMode Mode, string EntityName, List<string> PksList)
        {

            List<string> PKVariablesList = new List<string>();

            foreach (string PrimaryKeyName in PksList)
            {
                PKVariablesList.Add(String.Join(" = ", new[] { PrimaryKeyName, $"@{PrimaryKeyName}" }));
            }

            string PKVariablesString = String.Join(", ", PKVariablesList);

            switch (Mode)
            {
                case enDataAccessMode.Queries:
                    {


                        _Template.AppendLine($"            string Query = @\"DELETE FROM {_EntityToTableName[EntityName]}");
                        _Template.AppendLine($"                             WHERE {PKVariablesString};\";\n");
                        _Template.AppendLine($"            using SqlCommand Command = new SqlCommand(Query, Connection);");
                        foreach (var PK in PksList)
                        {
                            _Template.AppendLine($"            Command.Parameters.AddWithValue(\"@{PK}\", {PK});");

                        }
                        _Template.AppendLine($"            try");
                        _Template.AppendLine($"            {{");
                        _Template.AppendLine($"                 Connection.Open();\n");
                        _Template.AppendLine($"                 AffectedRows = await Command.ExecuteNonQueryAsync();\n");

                        _Template.AppendLine($"            }}");
                        _Template.AppendLine($"            catch");
                        _Template.AppendLine($"            {{");
                        _Template.AppendLine($"            }}");
                        break;
                    }

                case enDataAccessMode.StoredProcedures:
                    {

                        break;
                    }

            }
        }

        private void AppendAddFunction(string EntityName, string ConnectionString)
        {
            string AddDtoName = _EntityToDtosClassNames[EntityName][nameof(enDtoFunction.Add)];
            bool isThereIdentityPK = _EntityToPrimaryKey[EntityName].Any(x => x.Value == true);

            if (isThereIdentityPK)
            {
                _Template.AppendLine($"        public async Task <int?> AddAsync ({AddDtoName} dto)");
                _Template.AppendLine($"        {{");
                _Template.AppendLine($"            int? NewId = null;");
            }
            else
            {
                _Template.AppendLine($"        public async Task <bool> AddAsync ({AddDtoName} dto)");
                _Template.AppendLine($"        {{");
                _Template.AppendLine($"            int AffectedRows = 0;");
            }
                
            _Template.AppendLine($"            using SqlConnection Connection = new SqlConnection({ConnectionString});");

            AddFunctionCodeBasedOnDataAccessMode(_dto.DataAccessMode, EntityName, isThereIdentityPK);

            if (isThereIdentityPK)
                _Template.AppendLine($"            return NewId;");
            else
                _Template.AppendLine($"            return AffectedRows != 0;");
            _Template.AppendLine($"        }}\n");
        }

        private void AppendUpdateFunction(string EntityName, string ConnectionString)
        {
            string UpdateDtoName = _EntityToDtosClassNames[EntityName][nameof(enDtoFunction.Update)];

            Dictionary<string, string> PropertyToVariable = new Dictionary<string, string>();
            Dictionary<string, string> PrimaryKeysToVariable = new Dictionary<string, string>();

            _Template.Append($"        public async Task <bool> UpdateAsync ({UpdateDtoName} dto");
            foreach (var PropertyPair in _EntityNameToProperties[EntityName])
            {
                if (_EntityToPrimaryKey[EntityName].Keys.Contains(PropertyPair.Key))
                {
                    _Template.Append($", {PropertyPair.Value} {PropertyPair.Key}");
                    PrimaryKeysToVariable.Add(PropertyPair.Key, $"@{PropertyPair.Key}");
                }
                else
                {

                    PropertyToVariable.Add(PropertyPair.Key, $"@{PropertyPair.Key}"); //Example: Name, @Name
                }
            }
            _Template.AppendLine(")");
            _Template.AppendLine($"        {{");
            _Template.AppendLine($"            int AffectedRows = 0;");
            _Template.AppendLine($"            using SqlConnection Connection = new SqlConnection({ConnectionString});");
            UpdateFunctionCodeBasedOnDataAccessMode(_dto.DataAccessMode, EntityName, PropertyToVariable, PrimaryKeysToVariable);
            _Template.AppendLine($"            return AffectedRows != 0;");
            _Template.AppendLine($"        }}\n");
        }


        private void AppendGetByIdFunction(string EntityName, string ConnectionString)
        {
            string GetDtoName = _EntityToDtosClassNames[EntityName][nameof(enDtoFunction.Get)];
            List<string> PkNames = _EntityToPrimaryKey[EntityName].Keys.ToList();
            _Template.Append($"        public async Task <{GetDtoName}?> GetAsync (");
            foreach (var PkName in PkNames)
            {
                string PkDataType = _EntityNameToProperties[EntityName][PkName];
                _Template.Append($"{PkDataType} {PkName}, ");

            }
            _Template = _Template.Remove(_Template.Length - 2, 2);
            _Template.AppendLine(")");
            _Template.AppendLine($"        {{");
            _Template.AppendLine($"            {GetDtoName}? dto = null;");
            _Template.AppendLine($"            using SqlConnection Connection = new SqlConnection({ConnectionString});");
            GetFunctionCodeBasedOnDataAccessMode(_dto.DataAccessMode, EntityName, GetDtoName);
            _Template.AppendLine($"            return dto;");
            _Template.AppendLine($"        }}\n");
        }

        private void AppendDeleteFunction(string EntityName, string ConnectionString)
        {
            List<string> PkNames = _EntityToPrimaryKey[EntityName].Keys.ToList();
            _Template.Append($"        public async Task <bool> DeleteAsync (");

            foreach (var PkName in PkNames)
            {
                string PkDataType = _EntityNameToProperties[EntityName][PkName];
                _Template.Append($"{PkDataType} {PkName}, ");

            }
            _Template = _Template.Remove(_Template.Length - 2, 2);
            _Template.AppendLine(")");
            _Template.AppendLine($"        {{");
            _Template.AppendLine($"            int AffectedRows = 0;");
            _Template.AppendLine($"            using SqlConnection Connection = new SqlConnection({ConnectionString});");
            DeleteFunctionCodeBasedOnDataAccessMode(_dto.DataAccessMode, EntityName,PkNames);
            _Template.AppendLine($"            return AffectedRows > 0;");
            _Template.AppendLine($"        }}\n");
        }

        private void SaveRepoInFile(string RepoName, string FolderName, string RepoContent)
        {
            if (!Directory.Exists(_dto.DataAccessProjectPath))
            {
                return;
            }
            string ReposDirectory = $"{_dto.DataAccessProjectPath}\\{FolderName}";
            Directory.CreateDirectory(ReposDirectory);


            using (StreamWriter writer = new StreamWriter(Path.Combine(ReposDirectory, RepoName + ".cs")))
            {
                writer.Write(RepoContent);
            }

        }
        private void GenerateRepo(string EntityName, string ReposFolderName)
        {
            string RepoName = $"{EntityName}Repository";
            string NameSpace = $"{_ProjectName}.{RepoName}";
            string ConnectionStringVName = "_ConnectionString";
            _Template.Clear();
            _Template.Append(ClassTextHelper(NameSpace, RepoName,
                new List<string> { GetNamespace(_EntitiesFolderName, _dto.DataAccessProjectPath)
                                    , GetNamespace($"{_DtosFolderName}\\{_EntityToTableName[EntityName]}", _dto.DataAccessProjectPath)
                                    ,"Microsoft.Data.SqlClient"}));
            _Template.AppendLine($"        string {ConnectionStringVName} = \"\";");
            _Template.AppendLine($"        public {RepoName} (string connectionstring)");
            _Template.AppendLine($"        {{");
            _Template.AppendLine($"             {ConnectionStringVName} = connectionstring;");
            _Template.AppendLine($"        }}\n");
            AppendAddFunction(EntityName, ConnectionStringVName);
            AppendUpdateFunction(EntityName, ConnectionStringVName);
            AppendGetByIdFunction(EntityName, ConnectionStringVName);
            AppendDeleteFunction(EntityName, ConnectionStringVName);
            _Template.AppendLine("    }");
            _Template.AppendLine("}");
            SaveRepoInFile(RepoName, ReposFolderName, _Template.ToString());

        }


       public enum enInjectPackageStatus { Success,MoreThanOneCsprojFile, csprojFileloadFailed, PackagesAlreadyExists  }
        public (bool status, enInjectPackageStatus Details) InjectPackage(string Projectpath, string PackageName, string Version)
        {
            var csprojFile = Directory.GetFiles($"{Projectpath}", "*.csproj");
            if (csprojFile.Length > 1)
            { //TODO: solve this issue
                return (false, enInjectPackageStatus.MoreThanOneCsprojFile); //there are more than one .csproj file...user should specify which one to write the references in
            }

            XDocument csprojFileContent = XDocument.Load(csprojFile[0]);
            if (csprojFileContent.Root == null)
            {
                return (false, enInjectPackageStatus.csprojFileloadFailed);
            }


            XElement ItemGroup = csprojFileContent.Root.Element("ItemGroup");
            if (ItemGroup == null)
            {
                ItemGroup = new XElement("ItemGroup");
                csprojFileContent.Root.Add(ItemGroup);
            }

            XElement PackageReference = new XElement("PackageReference");
            XAttribute IncludeAttr = new XAttribute("Include", PackageName);
            XAttribute VersionAttr = new XAttribute("Version", Version);
            PackageReference.Add(new[] { IncludeAttr, VersionAttr });
            if (ItemGroup.Elements().FirstOrDefault(x => x == PackageReference) == default)
                return (false, enInjectPackageStatus.PackagesAlreadyExists); //we already added the package
            ItemGroup.Add(PackageReference);



            csprojFileContent.Save(csprojFile[0]);

            return (true, enInjectPackageStatus.Success);
        }
        private void GenerateRepos()
        {
            string ReposFolder = "Repositories";
            foreach (var pair in _EntityToTableName)
            {
                GenerateRepo(pair.Key, ReposFolder);
            }
        }
        public void AddConnectionStringToConfigFile()
        {


        }

        public void Generate()
        {
            GenerateEntities();
            SavePrimaryKeysPerEntity();
            GenerateDTOs();
            GenerateRepos();
            if (_dto.TargetPlatform == enTargetPlatform.NetCore)
                InjectPackage(_dto.DataAccessProjectPath, "Microsoft.Data.SqlClient", "7.0.1");
           
            //TODO: Add code of Stored procedures mode
            //TODO: Inject created classes so they can be identifed in .Net framework project
        }
    }

}
