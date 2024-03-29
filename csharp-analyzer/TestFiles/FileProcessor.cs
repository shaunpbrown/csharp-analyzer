namespace csharp_analyzer.Temp
{
    public class FileProcessor
    {
        public static FileData GetSingleTestFile()
        {
            var filePath = "C:\\Users\\nbuli\\source\\repos\\csharp-analyzer\\csharp-analyzer\\TestFiles\\Bird.cs";

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

        public static IEnumerable<string> GetCSharpFileNames()
        {
            var directoryPath = "C:\\Users\\nbuli\\source\\repos\\csharp-analyzer\\csharp-analyzer\\TestFiles";

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory does not exist.");
                return Array.Empty<string>();
            }

            return Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);
        }

        public static FileData ExtractFileData(string filePath)
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
                Content = reader.ReadToEnd()
            };
        }
    }
}
