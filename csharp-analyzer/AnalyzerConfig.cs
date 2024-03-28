namespace csharp_analyzer
{
    public static class AnalyzerConfig
    {
        // Single test file
        public static string FilePath { get; } = "C:\\Users\\nbuli\\source\\repos\\csharp-analyzer\\csharp-analyzer\\TestFiles\\Bird.cs";

        // Directory of test files
        public static string DirectoryPath { get; } = "C:\\Users\\nbuli\\source\\repos\\csharp-analyzer\\csharp-analyzer\\TestFiles";

        public static bool ConsoleLogTrees { get; } = true;

        public static bool UseBatchProcess { get; } = true;

        public static int BatchSize { get; } = 100;

        //public AnalyzerConfig()
        //{
        //    FilePath = string.Empty;
        //    DirectoryPath = string.Empty;
        //    ConsoleLogTrees = true;
        //    UseBatchProcess = true;
        //    BatchSize = 100;
        //}
    }
}
