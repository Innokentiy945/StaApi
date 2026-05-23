using Microsoft.EntityFrameworkCore;
using StaApi.Models;
using StaApi.Models.AuthApi;

namespace StaApi.Context;

public class AuthContext : DbContext
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) {}

    public DbSet<AppUserModel> Users => Set<AppUserModel>();
    public DbSet<RefreshTokenModel> RefreshTokens => Set<RefreshTokenModel>();
}