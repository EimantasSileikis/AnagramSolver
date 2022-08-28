using AnagramSolver.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.EF.CodeFirst.Data
{
    public class CodeFirstContext : DbContext
    {
        public virtual DbSet<WordModel> Words { get; set; } = null!;
        public virtual DbSet<SearchHistory> SearchHistories { get; set; } = null!;
        public virtual DbSet<CachedWord> CachedWords { get; set; } = null!;
        public virtual DbSet<Anagram> Anagrams { get; set; } = null!;
        public virtual DbSet<SearchLimit> SearchLimits { get; set; } = null!;

        public CodeFirstContext(DbContextOptions<CodeFirstContext> options) : base(options) { }
    }
}
