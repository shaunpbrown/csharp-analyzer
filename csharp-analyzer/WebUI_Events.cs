using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
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
    }
}
