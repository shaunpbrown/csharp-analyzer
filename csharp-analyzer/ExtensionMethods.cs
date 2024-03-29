using csharp_analyzer.CodeAnalysis;
using Microsoft.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace csharp_analyzer
{
    public static class ExtensionMethods
    {
        public static string ToJson(this SyntaxNode node)
        {
            var nodeName = Regex.Replace(node.ToString(), "[\"\\\\\n\r\t ]", "");
            if (nodeName .Length > 20)
            {
                nodeName = nodeName.Substring(0, 20) + "...";
            }
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"data\" : {");
            sb.Append($"\"id\": \"{nodeName}\"");
            sb.Append("},");
            sb.Append("\"children\": [");
            foreach (var child in node.ChildNodes())
            {
                sb.Append(child.ToJson());
                sb.Append(",");
            }
            if (node.ChildNodes().Any())
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }

        public static void LogToConsole(this SyntaxTree syntaxTree)
        {
            Console.WriteLine();
            Console.WriteLine($"Abstract Syntax Tree");
            Console.WriteLine("-----------------------------------------");
            CSharpAnalyzer.DisplayNodesAndTokensInConsole(syntaxTree.GetRoot());
            Console.WriteLine();
        }
    }
}
