using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces.Repositories
{
    public interface ISearchLimitRepository : IRepository<SearchLimit>
    {
        Task<SearchLimit?> GetByIpAsync(string ip);
    }

    
}
