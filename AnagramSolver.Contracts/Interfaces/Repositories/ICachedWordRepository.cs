using AnagramSolver.Contracts.Models;

namespace AnagramSolver.Contracts.Interfaces.Repositories
{
    public interface ICachedWordRepository : IRepository<CachedWord>
    {
        IEnumerable<string> GetCachedWordWithAnagrams(string word);
    }
}
