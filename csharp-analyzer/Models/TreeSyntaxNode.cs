using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public static IEnumerable<TreeSyntaxNode> FromTrimmed(IEnumerable<SyntaxNode> nodes)
        {
            foreach (var node in nodes)
            {
                // Define a display for each node
                var displayName = string.Empty;
                switch (node.Kind())
                {
                    case SyntaxKind.CompilationUnit:
                        displayName = "CompilationUnit";
                        break;

                    case SyntaxKind.IfStatement:
                        var ifStatement = (IfStatementSyntax)node;
                        displayName = $"if({ifStatement.Condition})";
                        break;

                    case SyntaxKind.ElseClause:
                        displayName = "else";
                        break;

                    case SyntaxKind.ExpressionStatement:
                        var expressionStatement = (ExpressionStatementSyntax)node;
                        displayName = expressionStatement.Expression.ToString();
                        break;

                    case SyntaxKind.MethodDeclaration:
                        var methodDeclaration = (MethodDeclarationSyntax)node;
                        displayName = $"{methodDeclaration.Identifier}";
                        break;

                    case SyntaxKind.ClassDeclaration:
                        var classDeclaration = (ClassDeclarationSyntax)node;
                        displayName = $"{classDeclaration.Identifier}";
                        break;

                    case SyntaxKind.PropertyDeclaration:
                        var propertyDeclaration = (PropertyDeclarationSyntax)node;
                        displayName = $"{propertyDeclaration.Identifier}";
                        break;

                    case SyntaxKind.ConstructorDeclaration:
                        var constructorDeclaration = (ConstructorDeclarationSyntax)node;
                        displayName = $"{constructorDeclaration.Identifier}";
                        break;

                    default:
                        var nodeKind = node.Kind().ToString();
                        if (nodeKind.Contains("Statement"))
                        {
                            displayName = node.ToString();
                        }
                        break;
                }

                if (string.IsNullOrEmpty(displayName)) // No display found for node skip
                {
                    foreach (var child in FromTrimmed(node.ChildNodes()))
                    {
                        yield return child;
                    }
                }
                else // Else return the node
                {
                    yield return new TreeSyntaxNode
                    {
                        SyntaxData = new Data
                        {
                            DisplayName = displayName,
                            TokenKind = node.Kind().ToString(),
                        },
                        Children = FromTrimmed(node.ChildNodes()),
                    };
                }
            }
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
