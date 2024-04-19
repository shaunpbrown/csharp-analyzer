using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace csharp_analyzer.Modals
{
    public class SyntaxDisplayNode
    {
        [JsonPropertyName("syntaxData")]
        public Data SyntaxData { get; set; } = new Data();

        [JsonPropertyName("children")]
        public IEnumerable<SyntaxDisplayNode> Children { get; set; } = Array.Empty<SyntaxDisplayNode>();

        public static SyntaxDisplayNode From(SyntaxNode node)
        {
            var displayName = Regex.Replace(node.ToString(), "[\"\\\\\n\r\t ]", "");
            if (displayName.Length > 20)
            {
                displayName = displayName.Substring(0, 20) + "...";
            }

            return new SyntaxDisplayNode
            {
                SyntaxData = new Data
                {
                    DisplayName = displayName,
                    TokenKind = node.Kind().ToString(),
                },
                Children = node.ChildNodes().Select(node => SyntaxDisplayNode.From(node))
            };
        }

        public static IEnumerable<SyntaxDisplayNode> FromTrimmed(IEnumerable<SyntaxNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (GetDisplayName(node, out var displayName)) 
                {
                    yield return new SyntaxDisplayNode
                    {
                        SyntaxData = new Data
                        {
                            DisplayName = displayName,
                            TokenKind = node.Kind().ToString(),
                        },
                        Children = FromTrimmed(node.ChildNodes()),
                    };
                }
                else 
                {
                    foreach (var child in FromTrimmed(node.ChildNodes()))
                    {
                        yield return child;
                    }
                }
            }
        }

        public static bool GetDisplayName(SyntaxNode node, out string displayName)
        {
            displayName = string.Empty;
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
                    displayName = $"{expressionStatement.Expression};";
                    break;

                case SyntaxKind.MethodDeclaration:
                    var methodDeclaration = (MethodDeclarationSyntax)node;
                    displayName = $"{methodDeclaration.Identifier}{string.Join(',', methodDeclaration.ParameterList)}";
                    break;

                case SyntaxKind.ClassDeclaration:
                    var classDeclaration = (ClassDeclarationSyntax)node;
                    displayName = $"class {classDeclaration.Identifier}";
                    break;

                case SyntaxKind.PropertyDeclaration:
                    var propertyDeclaration = (PropertyDeclarationSyntax)node;
                    displayName = $"{propertyDeclaration.Type} {propertyDeclaration.Identifier}";
                    break;

                case SyntaxKind.ConstructorDeclaration:
                    var constructorDeclaration = (ConstructorDeclarationSyntax)node;
                    displayName = $"{constructorDeclaration.Identifier}{string.Join(',', constructorDeclaration.ParameterList)}";
                    break;

                case SyntaxKind.InvocationExpression:
                    var invocationExpression = (InvocationExpressionSyntax)node;
                    displayName = $"{invocationExpression.Expression}{invocationExpression.ArgumentList}";
                    break;

                default:
                    var nodeKind = node.Kind().ToString();
                    if (nodeKind.Contains("Statement"))
                    {
                        displayName = node.ToString();
                    }
                    break;
            }

            return !string.IsNullOrEmpty(displayName);
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
