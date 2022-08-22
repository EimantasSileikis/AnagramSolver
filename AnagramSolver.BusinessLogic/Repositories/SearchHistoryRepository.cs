using AnagramSolver.Contracts.Interfaces.Repositories;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class SearchHistoryRepository : Repository<SearchHistory>, ISearchHistoryRepository
    {
        public CodeFirstContext CodeFirstContext { get { return (CodeFirstContext)Context; } }
        public SearchHistoryRepository(CodeFirstContext context) : base(context)
        {
        }
    }
}
