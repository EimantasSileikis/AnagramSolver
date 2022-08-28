using AnagramSolver.BusinessLogic.Files;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.Contracts.Interfaces.Repositories;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.Extensions.Configuration;

namespace AnagramSolver.BusinessLogic.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CodeFirstContext _context;

        public IDbWordRepository Words { get; private set; }
        public ICachedWordRepository CachedWords { get; private set; }
        public ISearchHistoryRepository SearchHistory { get; private set; }

        public ISearchLimitRepository SearchLimit { get; private set; }

        public UnitOfWork(CodeFirstContext context, IConfiguration config)
        {
            _context = context;
            Words = new DbWordRepository(_context, new FileManager(), config);
            CachedWords = new CachedWordRepository(_context);
            SearchHistory = new SearchHistoryRepository(_context);
            SearchLimit = new SearchLimitRepository(_context);
            //Words.SeedData().Wait();
            //CompleteAsync().Wait();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
