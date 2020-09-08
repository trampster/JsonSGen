using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.TypeGenerators
{
    public class DateTimeGenerator : IJsonGenerator
    {
        public string TypeName => "DateTime";

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonProperty property)
        {
            codeBuilder.AppendLine(indentLevel, $"json = json.ReadDateTime(out DateTime property{property.CodeName}Value);");
            codeBuilder.AppendLine(indentLevel, $"value.{property.CodeName} = property{property.CodeName}Value;");
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonProperty property)
        {
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
            codeBuilder.AppendLine(indentLevel, $"builder.AppendDate(value.{property.CodeName});");
        }
    }
}