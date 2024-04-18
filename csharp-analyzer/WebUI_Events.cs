using csharp_analyzer.CodeAnalysis;
using csharp_analyzer.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text.Json;
using WebUI4CSharp;

namespace csharp_analyzer
{
    public static class WebUI_Events
    {
        public static void GetBirdSyntaxTree(ref webui_event_t e)

        {
            var fileString = File.ReadAllText(AnalyzerConfig.BirdTestPath);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(fileString);
            Console.WriteLine("GetBirdSyntaxTree called");
            var json = syntaxTree.ToJson();

            WebUIEvent lEvent = new WebUIEvent(e);
            lEvent.ReturnString(json);
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
                    var fileString = File.ReadAllText(openFileDialog.FileName);
                    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(fileString);
                    if (AnalyzerConfig.ConsoleLogTrees)
                    {
                        syntaxTree.LogToConsole();
                    }
                    var json = syntaxTree.ToJson();

                    WebUIEvent lEvent = new WebUIEvent(e);
                    lEvent.ReturnString(json);
                }
            }
        }

        public static void LoadSyntaxTreesFromDirectory(ref webui_event_t e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    GlobalStorage.SyntaxTrees.Clear();

                    var fileNames = Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.cs", SearchOption.AllDirectories);

                    Parallel.ForEach(fileNames, fileName =>
                    {
                        using StreamReader reader = new(fileName);
                        var data = reader.ReadToEnd();
                        var syntaxTree = CSharpAnalyzer.GenerateSyntaxTree(data);
                        GlobalStorage.SyntaxTrees.GetOrAdd(fileName, syntaxTree);

                        if (AnalyzerConfig.ConsoleLogTrees)
                        {
                            lock (Console.Out)
                            {
                                Console.WriteLine(fileName);
                                syntaxTree.LogToConsole();
                            }
                        }
                    });

                    WebUIEvent lEvent = new WebUIEvent(e);
                    lEvent.ReturnString(JsonSerializer.Serialize(fileNames));
                }
            }
        }

        public static void GetSyntaxTreeWithFileName(ref webui_event_t e)
        {
            WebUIEvent lEvent = new WebUIEvent(e);
            string? fileName = lEvent.GetString();
            if (fileName is not null && GlobalStorage.SyntaxTrees.TryGetValue(fileName, out var syntaxTree) && syntaxTree is not null)
            {
                lEvent.ReturnString(syntaxTree.ToJson());
            }
            else  
            { 
                lEvent.ReturnString("Syntax tree not found");
            }
        }

        public static void GetSyntaxTreeWithFileNameTrimmed(ref webui_event_t e)
        {
            WebUIEvent lEvent = new WebUIEvent(e);
            string? fileName = lEvent.GetString();
            if (fileName is not null && GlobalStorage.SyntaxTrees.TryGetValue(fileName, out var syntaxTree) && syntaxTree is not null)
            {
                lEvent.ReturnString(syntaxTree.ToJsonTrimmed());
            }
            else
            {
                lEvent.ReturnString("Syntax tree not found");
            }
        }
    }
}
