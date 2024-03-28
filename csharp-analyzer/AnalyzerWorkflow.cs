using csharp_analyzer.CodeAnalysis;
using csharp_analyzer.FileProcessing;
using Microsoft.CodeAnalysis;

namespace csharp_analyzer
{
    public class AnalyzerWorkflow
    {
        public static SyntaxTree AnalyzeTestFileAsync()
        {
            var fileData = FileProcessor.GetSingleTestFile();

            var syntaxTree = CSharpAnalyzer.GenerateSyntaxTree(fileData.Content);

            if (AnalyzerConfig.ConsoleLogTrees)
            {
                DisplayInConsole(syntaxTree, fileData.Name);
            }

            return syntaxTree;
        }

        public static async Task<IEnumerable<SyntaxTree>> AnalyzeCodeBaseAsync()
        {
            var fileNames = await FileProcessor.GetCSharpFileNamesAsync();

            var syntaxTrees = new List<SyntaxTree>();

            foreach (var fileName in fileNames)
            {
                var fileData = await FileProcessor.ExtractFileData(fileName);

                var syntaxTree = CSharpAnalyzer.GenerateSyntaxTree(fileData.Content);

                syntaxTrees.Add(syntaxTree);

                if (AnalyzerConfig.ConsoleLogTrees)
                {
                    DisplayInConsole(syntaxTree, fileData.Name);
                }
            }

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
