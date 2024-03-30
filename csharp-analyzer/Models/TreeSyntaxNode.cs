using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace csharp_analyzer.Modals
{
    public class TreeSyntaxNode
    {
        [JsonPropertyName("syntaxData")]
        public Data SyntaxData { get; set; } = new Data();

        [JsonPropertyName("children")]
        public IEnumerable<TreeSyntaxNode> Children { get; set; } = Array.Empty<TreeSyntaxNode>();

        public static TreeSyntaxNode From(SyntaxNode node)
        {
            var displayName = Regex.Replace(node.ToString(), "[\"\\\\\n\r\t ]", "");
            if (displayName.Length > 20)
            {
                displayName = displayName.Substring(0, 20) + "...";
            }

            return new TreeSyntaxNode
            {
                SyntaxData = new Data
                {
                    DisplayName = displayName,
                    TokenKind = node.Kind().ToString(),
                },
                Children = node.ChildNodes().Select(node => TreeSyntaxNode.From(node))
            };
        }

        public class Data
        {
            [JsonPropertyName("displayName")]
            public string DisplayName { get; set; } = string.Empty;

            [JsonPropertyName("tokenKind")]
            public string TokenKind { get; set; } = string.Empty;
        }
    }
}
