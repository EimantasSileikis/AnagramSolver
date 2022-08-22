using AnagramSolver.Contracts.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IDbWordRepository Words { get; }
        ICachedWordRepository CachedWords { get; }
        ISearchHistoryRepository SearchHistory { get; }
        Task<int> CompleteAsync();
    }
}
