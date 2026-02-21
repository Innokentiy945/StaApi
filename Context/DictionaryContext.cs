using Microsoft.EntityFrameworkCore;
using StaApi.Models;

namespace StaApi.Context;

public class DictionaryContext : DbContext
{
    public DictionaryContext(DbContextOptions<DictionaryContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<DictionaryModel> DictionaryItem { get; set; }
}