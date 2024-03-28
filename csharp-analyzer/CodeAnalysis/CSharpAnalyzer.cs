using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace csharp_analyzer.CodeAnalysis
{
    public class CSharpAnalyzer
    {
        public static SyntaxTree GenerateSyntaxTree(string code)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            return syntaxTree;
        }

        public static void DisplayNodesInConsole(SyntaxNode syntaxNode, int indent = 0)
        {
            Console.Write("{0}{1} {2}", new string('\u00A0', 4 * indent), syntaxNode.Kind(), syntaxNode.Span);

            IEnumerable<SyntaxNode> childNodes = syntaxNode.ChildNodes();

            if (!childNodes.Any())
            {
                Console.WriteLine(" {0}", syntaxNode);
            }
            else
            {
                Console.WriteLine();   // end the line displayed with Write
                foreach (SyntaxNode childNode in childNodes)
                {
                    DisplayNodesInConsole(childNode, indent + 1);
                }
            }
        }

        public static void DisplayNodesAndTokensInConsole(SyntaxNodeOrToken syntaxNodeOrToken, int indent = 0)
        {
            Console.Write("{0}{1} [{2}..{3})", new string('\u00A0', 4 * indent), syntaxNodeOrToken.Kind(), syntaxNodeOrToken.Span.Start, syntaxNodeOrToken.Span.End);

            IEnumerable<SyntaxNodeOrToken> childNodesOrTokens = syntaxNodeOrToken.ChildNodesAndTokens();

            if (!childNodesOrTokens.Any())
            {
                Console.WriteLine(" {0}", syntaxNodeOrToken);
            }
            else
            {
                Console.WriteLine();
                foreach (SyntaxNodeOrToken childNodeOrToken in childNodesOrTokens)
                {
                    DisplayNodesAndTokensInConsole(childNodeOrToken, indent + 1);
                }
            }
        }
    }
}
