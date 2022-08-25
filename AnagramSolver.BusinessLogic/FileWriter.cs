using AnagramSolver.Contracts.Interfaces;

namespace AnagramSolver.BusinessLogic
{
    public class FileWriter : IFileWriter
    {
        public void WriteLine(string path, string line)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("File doesnt exist");
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(line);
            }
        }
    }
}
