namespace csharp_analyzer
{
    public static class AnalyzerConfig
    {
#if DEBUG
        public static bool ConsoleLogTrees { get; } = true;
#else
        public static bool ConsoleLogTrees { get; } = false;
#endif
    }
}
