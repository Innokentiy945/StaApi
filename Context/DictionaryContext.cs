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
    }
}