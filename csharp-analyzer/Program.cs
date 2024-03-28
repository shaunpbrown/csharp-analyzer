using csharp_analyzer;
using WebUI4CSharp;

var html = File.ReadAllText("resources\\index.html");
WebUIWindow window = new WebUIWindow();
window.Bind("GetBirdSyntaxTree", WebUI_Events.GetBirdSyntaxTree);
window.Show(html);
Console.WriteLine("Application is running...");
WebUI.Wait();
WebUI.Clean();

//var html = File.ReadAllText("resources\\index.html");
//WebUIWindow window = new();
//window.Bind("GetBirdSyntaxTree", WebUI_Events.GetTestFileSyntaxTree);
//window.Show(html);
//Console.WriteLine("Application is running...");
//WebUI.Wait();
//WebUI.Clean();