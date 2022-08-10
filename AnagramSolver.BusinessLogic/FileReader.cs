using AnagramSolver.Contracts.Interfaces;

namespace AnagramSolver.BusinessLogic
{
    public class FileReader : IFileReader
    {
        public string[] ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("File doesnt exist");
                return new string[0];
            }

            return File.ReadAllLines(path);
        }
    }
}
