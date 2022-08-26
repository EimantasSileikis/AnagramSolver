using AnagramSolver.Contracts.Models;

namespace AnagramSolver.Contracts.Interfaces.Repositories
{
    public interface ICachedWordRepository : IRepository<CachedWord>
    {
        bool WordExists(string word);
        IEnumerable<string> GetCachedWordWithAnagrams(string word);
    }
}
