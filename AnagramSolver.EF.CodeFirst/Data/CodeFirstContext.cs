using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.EF.CodeFirst.Data
{
    public class CodeFirstContext : DbContext
    {
        public DbSet<WordModel> Words { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<CachedWordEntity> CachedWords { get; set; } = null!;
        public DbSet<AnagramsEntity> Anagrams { get; set; } = null!;


        public CodeFirstContext(DbContextOptions<CodeFirstContext> options) : base(options) { }


    }
}
