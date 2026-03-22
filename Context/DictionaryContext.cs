using Microsoft.EntityFrameworkCore;
using StaApi.Models;

namespace StaApi.Context;

public class DictionaryContext : DbContext
{
    public DictionaryContext(DbContextOptions<DictionaryContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<DictionaryExplanationaryModel> DictionaryExplanatoryItem { get; set; }
    public DbSet<DictionaryMorphologyModel> DictionaryMorphologyItem { get; set; }
}