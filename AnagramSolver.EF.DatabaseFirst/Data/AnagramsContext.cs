using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AnagramSolver.EF.DatabaseFirst.Models;

namespace AnagramSolver.EF.DatabaseFirst.Data
{
    public partial class AnagramsContext : DbContext
    {
        public AnagramsContext()
        {
        }

        public AnagramsContext(DbContextOptions<AnagramsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Anagram> Anagrams { get; set; } = null!;
        public virtual DbSet<CachedWord> CachedWords { get; set; } = null!;
        public virtual DbSet<SearchHistory> SearchHistories { get; set; } = null!;
        public virtual DbSet<Word> Words { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
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

            modelBuilder.Entity<Word>(entity =>
            {
                entity.ToTable("Word");

                entity.Property(e => e.PartOfSpeech)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Word1)
                    .HasMaxLength(255)
                    .HasColumnName("Word");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
