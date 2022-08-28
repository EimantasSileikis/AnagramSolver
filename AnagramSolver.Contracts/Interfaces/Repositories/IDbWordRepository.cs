using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces.Repositories
{
    public interface IDbWordRepository : IRepository<WordModel>, IWordRepository
    {
        IEnumerable<WordModel> GetSearchWords(string word);
        Task SeedData();
        IEnumerable<WordModel> GetWordList(int pageIndex, int pageSize);
        int GetDictionaryCount();
        void Edit(WordModel word);
    }
}
