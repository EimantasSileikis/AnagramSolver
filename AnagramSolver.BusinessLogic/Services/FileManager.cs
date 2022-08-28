using AnagramSolver.Contracts.Interfaces.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class FileManager : IFileManager
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
