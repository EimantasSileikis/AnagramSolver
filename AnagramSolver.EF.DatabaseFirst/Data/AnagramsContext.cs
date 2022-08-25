using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.DatabaseFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.EF.DatabaseFirst.Data
{
    public partial class AnagramsContext : DbContext
    {
        public AnagramsContext(DbContextOptions<AnagramsContext> options) : base(options) { }

        public virtual DbSet<Anagram> Anagrams { get; set; } = null!;
        public virtual DbSet<CachedWord> CachedWords { get; set; } = null!;
        public virtual DbSet<SearchHistory> SearchHistories { get; set; } = null!;
        public virtual DbSet<WordModel> Words { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=Anagrams;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anagram>(entity =>
            {
                entity.Property(e => e.Anagram1).HasColumnName("Anagram");

                entity.HasOne(d => d.Word)
                    .WithMany(p => p.Anagrams)
                    .HasForeignKey(d => d.WordId)
                    .HasConstraintName("FK__Anagrams__WordId__412EB0B6");
            });

            modelBuilder.Entity<CachedWord>(entity =>
            {
                entity.ToTable("CachedWord");

                entity.Property(e => e.Word).HasMaxLength(255);
            });

            modelBuilder.Entity<SearchHistory>(entity =>
            {
                entity.ToTable("SearchHistory");

                entity.Property(e => e.IpAddress).IsUnicode(false);
            });

            modelBuilder.Entity<WordModel>(entity =>
            {
                entity.ToTable("Word");

                entity.Property(e => e.PartOfSpeech)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Word)
                    .HasMaxLength(255)
                    .HasColumnName("Word");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
