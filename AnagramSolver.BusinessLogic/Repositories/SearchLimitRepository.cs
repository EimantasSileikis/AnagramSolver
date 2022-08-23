using AnagramSolver.Contracts.Interfaces.Repositories;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class SearchLimitRepository : Repository<SearchLimit>, ISearchLimitRepository
    {
        public CodeFirstContext CodeFirstContext { get { return (CodeFirstContext)Context; } }
        public SearchLimitRepository(CodeFirstContext context) : base(context)
        {
        }

        public async Task<SearchLimit?> GetByIpAsync(string ip)
        {
            return await CodeFirstContext.SearchLimits.FirstOrDefaultAsync(limit => limit.Ip == ip);
        }
    }
}
