namespace csharp_analyzer.FileProcessing
{
    public class FileProcessor
    {
        public static FileData GetSingleTestFile()
        {
            var filePath = AnalyzerConfig.FilePath;

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return new FileData();
            }

            return new FileData
            {
                Name = Path.GetFileName(filePath),
                Content = File.ReadAllText(filePath)
            };
        }

        public static async Task<IEnumerable<string>> GetCSharpFileNamesAsync()
        {
            var directoryPath = AnalyzerConfig.DirectoryPath;

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory does not exist.");
                return Array.Empty<string>();
            }

            return await Task.Run(() => Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories));
        }

        public static async Task<FileData> ExtractFileData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No file to extract data from.");
                return new FileData();
            }

            using StreamReader reader = new(filePath);
            return new FileData
            {
                Name = Path.GetFileName(filePath),
                Content = await reader.ReadToEndAsync()
            };
        }
    }
}
