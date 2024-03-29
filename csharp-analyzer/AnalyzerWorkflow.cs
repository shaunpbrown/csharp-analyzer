using csharp_analyzer.CodeAnalysis;
using csharp_analyzer.FileProcessing;
using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;

namespace csharp_analyzer
{
    public class AnalyzerWorkflow
    {
        public static SyntaxTree AnalyzeTestFile()
        {
            var fileData = FileProcessor.GetSingleTestFile();

            var syntaxTree = CSharpAnalyzer.GenerateSyntaxTree(fileData.Content);

            if (AnalyzerConfig.ConsoleLogTrees)
            {
                DisplayInConsole(syntaxTree, fileData.Name);
            }

            return syntaxTree;
        }

        public static IEnumerable<SyntaxTree> AnalyzeCodeBase()
        {
            var fileNames = FileProcessor.GetCSharpFileNames();

            var syntaxTrees = new ConcurrentBag<SyntaxTree>();

            Parallel.ForEach(fileNames, fileName =>
            {
                var fileData = FileProcessor.ExtractFileData(fileName);
                var syntaxTree = CSharpAnalyzer.GenerateSyntaxTree(fileData.Content);

                syntaxTrees.Add(syntaxTree);

                if (AnalyzerConfig.ConsoleLogTrees)
                {
                    lock (Console.Out)
                    {
                        DisplayInConsole(syntaxTree, fileData.Name);
                    }
                }
            });

            return syntaxTrees;
        }

        private static void DisplayInConsole(SyntaxTree syntaxTree, string fileName)
        {
            Console.WriteLine();
            Console.WriteLine($"Abstract Syntax Tree for {fileName}");
            Console.WriteLine("-----------------------------------------");
            CSharpAnalyzer.DisplayNodesAndTokensInConsole(syntaxTree.GetRoot());
            Console.WriteLine();
        }
    }
}
