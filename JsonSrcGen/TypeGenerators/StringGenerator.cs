using System;
using System.Text;
using JsonSrcGen;

namespace JsonSrcGen.TypeGenerators
{
    public class StringGenerator : IJsonGenerator
    {
        public string GeneratorId => "String";

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter, JsonFormat format)
        {
            string propertyValueName = $"property{UniqueNumberGenerator.UniqueNumber}Value";
            codeBuilder.AppendLine(indentLevel, $"json = json.Read(out string? {propertyValueName});");
            codeBuilder.AppendLine(indentLevel, valueSetter(propertyValueName));
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter, bool canBeNull, JsonFormat format)
        {
            if(canBeNull)
            {
                codeBuilder.AppendLine(indentLevel, $"if({valueGetter} == null)");
                codeBuilder.AppendLine(indentLevel, "{");
                var nullAppendBuilder = new StringBuilder(appendBuilder.ToString());
                nullAppendBuilder.Append("null");
                codeBuilder.MakeAppend(indentLevel+1, nullAppendBuilder, format);
                codeBuilder.AppendLine(indentLevel, "}");

                codeBuilder.AppendLine(indentLevel, "else");
                codeBuilder.AppendLine(indentLevel, "{");
                indentLevel++;
            }
            
            appendBuilder.Append($"\\\"");
            codeBuilder.MakeAppend(indentLevel, appendBuilder, format);
            codeBuilder.AppendLine(indentLevel, $"builder.AppendEscaped({valueGetter});");
            appendBuilder.Append($"\\\"");
            codeBuilder.MakeAppend(indentLevel, appendBuilder, format);
            
            if(canBeNull)
            {
                indentLevel--;
                codeBuilder.AppendLine(indentLevel, "}");
            }
        }

        public CodeBuilder ClassLevelBuilder => null;

        public string OnNewObject(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter)
        {
            codeBuilder.AppendLine(indentLevel, valueSetter("null"));
            return null;
        }

        public void OnObjectFinished(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter, string wasSetVariable)
        {
            
        }
    }
}