using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Concurrent;

namespace csharp_analyzer.Models
{
    public static class GlobalStorage
    {
        public static ConcurrentDictionary<string, SyntaxTree> SyntaxTrees { get; set; } = new ();

        public static CSharpCompilation? Compilation { get; set; }
    }
}
