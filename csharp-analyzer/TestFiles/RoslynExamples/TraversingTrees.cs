// https://github.com/dotnet/roslyn/blob/main/docs/wiki/Getting-Started-C%23-Syntax-Analysis.md

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GettingStartedCS
{
    public class TraverseTrees
    {
        public static void Main()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(
@"using System;
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}");

            var root = (CompilationUnitSyntax)tree.GetRoot();

            var firstMember = root.Members[0];

            var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;

            var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];

            var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];

            var argsParameter = mainDeclaration.ParameterList.Parameters[0];

        }
    }
}