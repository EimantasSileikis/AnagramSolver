using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public interface IWordRepository
    {
        void LoadDictionary();
        Dictionary<string, WordInfo> GetWords();
    }
}
