using AnagramSolver.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class FileReader : IFileReader
    {
        public string[] ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                return new string[0];
            }

            return File.ReadAllLines(path);
        }
    }
}
