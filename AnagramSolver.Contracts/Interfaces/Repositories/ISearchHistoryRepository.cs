using AnagramSolver.Contracts.Models;

namespace AnagramSolver.Contracts.Interfaces.Repositories
{
    public interface ISearchHistoryRepository : IRepository<SearchHistory>
    {
        Task AddSearchHistoryAsync(string? ipAddress, string searchWord, IEnumerable<string> anagrams, int timeSpent);
    }
}
