using Microsoft.EntityFrameworkCore;
using StaApi.Models;

namespace StaApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserModel> Users => Set<UserModel>();

    public DbSet<AuditLogModel> AuditLogs => Set<AuditLogModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.UseOpenIddict();
    }
}