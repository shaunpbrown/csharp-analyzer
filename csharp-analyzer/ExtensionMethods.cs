using csharp_analyzer.CodeAnalysis;
using csharp_analyzer.Modals;
using Microsoft.CodeAnalysis;
using System.Collections;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace csharp_analyzer
{
    public static class ExtensionMethods
    {
        public static string ToJson(this SyntaxTree node)
        {
            try{
                return JsonSerializer.Serialize(SyntaxDisplayNode.From(node.GetRoot()));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new SyntaxDisplayNode{
                    SyntaxData = new SyntaxDisplayNode.Data
                    {
                        DisplayName = "Error",
                        TokenKind = e.Message,
                    }
                });
            }
        }

        public static string ToJsonTrimmed(this SyntaxTree node)
        {
            try
            {
                var trimmed = SyntaxDisplayNode.FromTrimmed(Enumerable.Repeat(node.GetRoot(), 1)).First();
                return JsonSerializer.Serialize(trimmed);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new SyntaxDisplayNode
                {
                    SyntaxData = new SyntaxDisplayNode.Data
                    {
                        DisplayName = "Error",
                        TokenKind = e.Message,
                    }
                });
            }
        }

        [Obsolete("Use ToJson instead")]
        public static string ToJsonNoObject(this SyntaxNode node)
        {
            var nodeName = Regex.Replace(node.ToString(), "[\"\\\\\n\r\t ]", "");
            if (nodeName .Length > 20)
            {
                nodeName = nodeName.Substring(0, 20) + "...";
            }
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"syntaxNodeData\" : {");
            sb.Append($"\"displayName\": \"{nodeName}\"");
            sb.Append("},");
            sb.Append("\"children\": [");
            foreach (var child in node.ChildNodes())
            {
                sb.Append(child.ToJsonNoObject());
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
