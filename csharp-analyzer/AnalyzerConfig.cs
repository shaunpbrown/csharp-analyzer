namespace csharp_analyzer
{
    public static class AnalyzerConfig
    {
        public static string BirdTestPath { get; } = "C:\\csharp-analyzer\\csharp-analyzer\\TestFiles\\Bird.cs";
        //public static string BirdTestPath { get; } = "C:\\Users\\nbuli\\source\\repos\\csharp-analyzer\\csharp-analyzer\\TestFiles\\Bird.cs";

#if DEBUG
        public static bool ConsoleLogTrees { get; } = true;
#else
        public static bool ConsoleLogTrees { get; } = false;
#endif
    }
}
