using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IDbWordRepository : IWordRepository
    {
        List<Word> SearchWord(string word);
        bool StoreToCachedTable(string inputWord, List<string> anagrams);
        bool AnagramsFound(string word);
        List<string> GetCachedAnagrams(string inputWord);
        void StoreSearchData(string ipAddress, string inputWord, List<string> anagrams);
        List<SearchHistory> GetSearchHistory();
        void DeleteTableData(string tableName);
    }
}
