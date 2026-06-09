using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneratorBusiness.DTOs;

namespace GeneratorBusiness
{
    public class GenerateBusiness : GenericClass
    {
        StringBuilder _Template = new StringBuilder();

        BusinessGeneratorDTO _dto;
        public GenerateBusiness(BusinessGeneratorDTO dto)
        {
            _dto = dto;
        }
      

       

        string GetDefaultValueOfType(string DataType)
        {
            if (DataType[DataType.Length - 1] == '?') //Nullable types
            {
                return "null";
            }

            if (DataType == "object")
            {
                return "null";
            }

            if (DataType == "string")
            {
                return "\"\"";
            }

            if (DataType == "DateTime")
            {
                return "DateTime.MinValue";
            }

            if (DataType == "char")
            {
                return "\' \'";
            }

            if (DataType == "bool")
            {
                return "false";
            }

            //reaching here means the type is Non-nullable numeric type
            return "-1";

        }
        string GetDefaultConstructorText()
        {
            StringBuilder sbConstructor = new StringBuilder();
            sbConstructor.AppendLine($"        {_dto.ClassName}()");
            sbConstructor.AppendLine("        {");

            foreach (var property in _dto.PropertiesInfo)
            {
                sbConstructor.AppendLine($"            {property.Name} = {GetDefaultValueOfType(property.DataType)}");
            }
            
            sbConstructor.AppendLine("        }");
            return sbConstructor.ToString();

        }

        string GetParameters()
        {
            StringBuilder Parameters = new StringBuilder();
            foreach (var property in _dto.PropertiesInfo)
            {
                Parameters.Append($"{property.DataType} {property.Name.ToLower()}, ");
            }

            Parameters = Parameters.Remove(Parameters.Length - 2, 2);

            return Parameters.ToString();
        }

        string GetParameterizedConstructorText()
        {
            StringBuilder sbConstructor = new StringBuilder();
            sbConstructor.AppendLine($"        {_dto.ClassName}({GetParameters()})");
            sbConstructor.AppendLine("        {");
            foreach (var property in _dto.PropertiesInfo)
            {
                sbConstructor.AppendLine($"            this.{property.Name} = {property.Name.ToLower()};");
            }
            sbConstructor.AppendLine("        }");

            return sbConstructor.ToString();
        }

       
        public void Generate()
        {
            _Template.AppendLine("using System;\n");

            _Template.AppendLine($"namespace {GetNamespace(_dto.FolderName, _dto.ProjectPath)}");
            _Template.AppendLine("{\n");
            _Template.AppendLine($"    {GetClassSignature(_dto.ClassName)}");
            _Template.AppendLine("    {");
         
            // default constructor
            _Template.AppendLine(GetDefaultConstructorText());
            //parameterized constructor
            _Template.AppendLine(GetParameterizedConstructorText());

            _Template.ToString();

            //TODO: complete the class
            //TODO: fix namespace issue

        }

    }
}
