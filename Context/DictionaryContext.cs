using Microsoft.EntityFrameworkCore;
using StaApi.Models;
using StaApi.Models.Dictionary;

namespace StaApi.Context
{
    public class DictionaryContext : DbContext
    {
        public DictionaryContext(DbContextOptions<DictionaryContext> options) : base(options)
        {
            Database.EnsureCreated();   
        }
        public DbSet<DictionaryExplanatoryModel> DictionaryExplanatoryItem { get; set; }
        public DbSet<DictionaryMorphologyModel> DictionaryMorphologyItem { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DictionaryMorphologyModel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Wordform).IsRequired(false);
                entity.Property(e => e.Lemma).IsRequired(false);
                entity.Property(e => e.Msd).IsRequired(false);
                entity.Property(e => e.Type).IsRequired(false);
                entity.Property(e => e.Upos).IsRequired(false);
                entity.Property(e => e.Frequency).IsRequired(false);
                entity.Property(e => e.PerMillion).IsRequired(false);

                entity.Property(e => e.Features_Type).IsRequired(false);
                entity.Property(e => e.Features_Degree).IsRequired(false);
                entity.Property(e => e.Features_Gender).IsRequired(false);
                entity.Property(e => e.Features_Number).IsRequired(false);
                entity.Property(e => e.Features_Case).IsRequired(false);
                entity.Property(e => e.Features_Definiteness).IsRequired(false);

                entity.Property(e => e.Morph_Case).IsRequired(false);
                entity.Property(e => e.Morph_Definite).IsRequired(false);
                entity.Property(e => e.Morph_Degree).IsRequired(false);
                entity.Property(e => e.Morph_Gender).IsRequired(false);
                entity.Property(e => e.Morph_Number).IsRequired(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}