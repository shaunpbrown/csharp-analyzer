using csharp_analyzer;
using System.Runtime.InteropServices;
using WebUI4CSharp;

#if DEBUG
// Opens a console window for debugging purposes
[DllImport("kernel32.dll", SetLastError = true)]
[return: MarshalAs(UnmanagedType.Bool)]
static extern bool AllocConsole();
AllocConsole();
#endif

var html = File.ReadAllText("resources\\index.html");
WebUIWindow window = new WebUIWindow();
window.Bind("GetBirdSyntaxTree", WebUI_Events.GetBirdSyntaxTree);
window.Bind("LoadSyntaxTreeFromFile", WebUI_Events.LoadSyntaxTreeFromFile);
window.Bind("LoadSyntaxTreeFromDirectory", WebUI_Events.LoadSyntaxTreesFromDirectory);
window.Show(html);
Console.WriteLine("Application is running...");
WebUI.Wait();
WebUI.Clean();
