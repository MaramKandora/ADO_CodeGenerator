
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCodeGenerator.Utility
{
    public class Utility
    {
        // ─────────────────────────────────────────────
        // Helper: GUID → string {XXXXXXXX-...-XXXX}
        // ─────────────────────────────────────────────
        public static string NewGuidString()
        {
            return Guid.NewGuid().ToString("B"); // B = braces
        }

        // ─────────────────────────────────────────────
        // Helper: write a UTF-8 text file
        // ─────────────────────────────────────────────
        public static bool WriteTextFile(string fullPath, string content)
        {
            try
            {
                File.WriteAllText(fullPath, content, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[ERROR] Failed to write file: {fullPath}\n{ex.Message}");
                return false;
            }
        }

        // ─────────────────────────────────────────────
        // Helper: create directory
        // (ignore if already exists)
        // ─────────────────────────────────────────────
        public static void EnsureDir(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[WARN] Failed to create directory: {path}\n{ex.Message}");
            }
        }
        public static string MapSqlToCSharpDataType(string sqlType)
        {
            if (sqlType == null)
                return "object";

            switch (sqlType.ToLower())
            {
                case "bigint": return "long";
                case "binary": return "byte[]";
                case "bit": return "bool";
                case "char": return "string";
                case "date": return "DateTime";
                case "datetime": return "DateTime";
                case "datetime2": return "DateTime";
                case "datetimeoffset": return "DateTimeOffset";
                case "decimal": return "decimal";
                case "float": return "double";
                case "image": return "byte[]";
                case "int": return "int";
                case "money": return "decimal";
                case "nchar": return "string";
                case "ntext": return "string";
                case "numeric": return "decimal";
                case "nvarchar": return "string";
                case "real": return "float";
                case "smalldatetime": return "DateTime";
                case "smallint": return "short";
                case "smallmoney": return "decimal";
                case "text": return "string";
                case "time": return "TimeSpan";
                case "timestamp": return "byte[]";
                case "tinyint": return "byte";
                case "uniqueidentifier": return "Guid";
                case "varbinary": return "byte[]";
                case "varchar": return "string";

                default: return "object";
            }
        }

        public static string MapDataTypeToReaderMethod(string csharpType)
        {
            switch (csharpType.ToLower())
            {
                case "int":
                case "int32":
                    return "GetInt32";

                case "long":
                case "int64":
                    return "GetInt64";

                case "short":
                case "int16":
                    return "GetInt16";

                case "byte":
                    return "GetByte";

                case "bool":
                case "boolean":
                    return "GetBoolean";

                case "float":
                    return "GetFloat";

                case "double":
                    return "GetDouble";

                case "decimal":
                    return "GetDecimal";

                case "string":
                    return "GetString";

                case "datetime":
                    return "GetDateTime";

                case "guid":
                    return "GetGuid";

                case "byte[]":
                    return "GetBytes";

                case "object":
                    return "GetValue";

                default:
                    return "GetValue"; // fallback
            }
        }

        public static string GetSingularName(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                return tableName;

            // Irregular words
            var irregulars = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "People", "Person" },
        { "Children", "Child" },
        { "Men", "Man" },
        { "Women", "Woman" },
        { "Teeth", "Tooth" },
        { "Feet", "Foot" }
    };

            if (irregulars.ContainsKey(tableName))
                return irregulars[tableName];

            // Categories -> Category
            if (tableName.EndsWith("ies", StringComparison.OrdinalIgnoreCase))
                return tableName.Substring(0, tableName.Length - 3) + "y";

            // Boxes -> Box
            if (tableName.EndsWith("xes", StringComparison.OrdinalIgnoreCase) ||
                tableName.EndsWith("ses", StringComparison.OrdinalIgnoreCase) ||
                tableName.EndsWith("zes", StringComparison.OrdinalIgnoreCase) ||
                tableName.EndsWith("ches", StringComparison.OrdinalIgnoreCase) ||
                tableName.EndsWith("shes", StringComparison.OrdinalIgnoreCase))
            {
                return tableName.Substring(0, tableName.Length - 2);
            }

            // Employees -> Employee
            if (tableName.EndsWith("s", StringComparison.OrdinalIgnoreCase))
                return tableName.Substring(0, tableName.Length - 1);

            return tableName;
        }
    }
}
