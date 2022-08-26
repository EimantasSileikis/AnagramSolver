using AnagramSolver.Contracts.Models;

namespace AnagramSolver.Contracts.Interfaces.Repositories
{
    public interface IRawDbWordRepository : IWordRepository
    {
        List<WordModel> SearchWord(string word);
        bool StoreToCachedTable(string inputWord, List<string> anagrams);
        bool AnagramsFound(string word);
        List<string> GetCachedAnagrams(string inputWord);
        void StoreSearchData(string ipAddress, string inputWord, List<string> anagrams, int timeSpent);
        List<SearchHistory> GetSearchHistory();
        void DeleteTableData(string tableName);
    }
}
