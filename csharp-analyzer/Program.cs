using WebUI4CSharp;
using csharp_analyzer;

var html = File.ReadAllText("resources\\index.html");
WebUIWindow window = new WebUIWindow();
window.Bind("GetBirdSyntaxTree", WebUI_Events.GetBirdSyntaxTree);
window.Show(html);
Console.WriteLine("Application is running...");
WebUI.Wait();
WebUI.Clean();
