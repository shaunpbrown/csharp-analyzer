using csharp_analyzer.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Concurrent;
using WebUI4CSharp;

namespace csharp_analyzer
{
    public static class WebUI_Events
    {
        public static void GetBirdSyntaxTree(ref webui_event_t e)
        {
            WebUIEvent lEvent = new WebUIEvent(e);
            var fileString = File.ReadAllText("C:\\csharp-analyzer\\csharp-analyzer\\TestFiles\\Bird.cs");
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(fileString);
            var root = syntaxTree.GetRoot();
            Console.WriteLine("GetBirdSyntaxTree called");
            var json = root.ToJson();
            lEvent.ReturnString(json);
        }

        public static void LoadSyntaxTreesFromDirectory(ref webui_event_t e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileNames = Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.cs", SearchOption.AllDirectories);
                    var syntaxTrees = new ConcurrentDictionary<string, SyntaxTree>();

                    Parallel.ForEach(fileNames, fileName =>
                    {
                        using StreamReader reader = new(fileName);
                        var data = reader.ReadToEnd();
                        var syntaxTree = CSharpAnalyzer.GenerateSyntaxTree(data);
                        syntaxTrees.GetOrAdd(fileName, syntaxTree);

                        if (AnalyzerConfig.ConsoleLogTrees)
                        {
                            lock (Console.Out)
                            {
                                Console.WriteLine(fileName);
                                syntaxTree.LogToConsole();
                            }
                        }
                    });
                }
            }
        }

        public static void LoadSyntaxTreeFromFile(ref webui_event_t e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "C# files (*.cs)|*.cs|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WebUIEvent lEvent = new WebUIEvent(e);
                    var fileString = File.ReadAllText(openFileDialog.FileName);
                    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(fileString);
                    var root = syntaxTree.GetRoot();
                    if (AnalyzerConfig.ConsoleLogTrees)
                    {
                        syntaxTree.LogToConsole();
                    }
                    var json = root.ToJson();
                    lEvent.ReturnString(json);
                }
            }
        }
    }
}
