using csharp_analyzer.Models;
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
                var displayNode = GetDisplayData(node);
                var children = GetChildren(node);
                if (displayNode is not null) 
                {
                    yield return new SyntaxDisplayNode
                    {
                        SyntaxData = displayNode,
                        Children = FromTrimmed(children),
                    };
                }
                else 
                {
                    foreach (var child in FromTrimmed(children))
                    {
                        yield return child;
                    }
                }
            }
        }

        public static Data? GetDisplayData(SyntaxNode node)
        {
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

            if (string.IsNullOrEmpty(displayName))
                return null;

            return new Data
            {
                DisplayName = displayName,
                TokenKind = node.Kind().ToString(),
            };
        }

        public static IEnumerable<SyntaxNode> GetChildren(SyntaxNode node)
        {
            if (node.IsKind(SyntaxKind.InvocationExpression) && GlobalStorage.Compilation is not null)
            {
                var methodSymbol = GlobalStorage.Compilation.GetSemanticModel(node.SyntaxTree).GetSymbolInfo(node).Symbol;
                if (methodSymbol is not null)
                {
                    var syntaxReference = methodSymbol.DeclaringSyntaxReferences.FirstOrDefault();
                    if (syntaxReference is not null)
                    {
                        var declaration = syntaxReference.GetSyntax();
                        if (declaration is not null)
                        {
                            yield return declaration;
                        }
                    }
                }
            }

            foreach (var child in node.ChildNodes())
            {
                yield return child;
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
