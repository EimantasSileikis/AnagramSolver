using AnagramSolver.Contracts.Interfaces.Repositories;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.BusinessLogic.Data
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

        public async Task<bool> ModifySearchLimit(string? ipAddress, uint increaseBy, uint seachLimit, bool checkSearchLimits = false)
        {
            if (ipAddress == null)
                return false;

            if (await Exist(x => x.Ip == ipAddress))
            {
                var userByIp = await GetByIpAsync(ipAddress);

                if (checkSearchLimits && CodeFirstContext.SearchHistories.Where(x => x.IpAddress == ipAddress).Count() >= userByIp?.Limit)
                    return false;

                if (userByIp != null)
                {
                    if (checkSearchLimits)
                        userByIp.Limit -= increaseBy;
                    else
                        userByIp.Limit += increaseBy;
                }
            }
            else
            {
                if (checkSearchLimits)
                    await CodeFirstContext.SearchLimits.AddAsync(new SearchLimit
                    { Ip = ipAddress, Limit = seachLimit - increaseBy });
                else
                    await CodeFirstContext.SearchLimits.AddAsync(new SearchLimit
                    { Ip = ipAddress, Limit = seachLimit + increaseBy });
            }
            return true;
        }
    }
}
