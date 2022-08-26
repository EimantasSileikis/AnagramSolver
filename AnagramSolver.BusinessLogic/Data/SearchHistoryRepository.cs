using AnagramSolver.Contracts.Interfaces.Repositories;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;

namespace AnagramSolver.BusinessLogic.Data
{
    public class SearchHistoryRepository : Repository<SearchHistory>, ISearchHistoryRepository
    {
        public CodeFirstContext CodeFirstContext { get { return (CodeFirstContext)Context; } }
        public SearchHistoryRepository(CodeFirstContext context) : base(context)
        {
        }

        public async Task AddSearchHistoryAsync(string? ipAddress, string searchWord,
                                        IEnumerable<string> anagrams, int timeSpent)
        {
            await CodeFirstContext.SearchHistories.AddAsync(new SearchHistory
            {
                IpAddress = ipAddress ?? "",
                SearchWord = searchWord,
                Anagrams = string.Join(",", anagrams.ToArray()),
                TimeSpent = timeSpent
            });
        }
    }
}
