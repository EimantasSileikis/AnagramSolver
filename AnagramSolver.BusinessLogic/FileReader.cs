using AnagramSolver.Contracts;
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
            return File.ReadAllLines(path);
        }
    }
}
