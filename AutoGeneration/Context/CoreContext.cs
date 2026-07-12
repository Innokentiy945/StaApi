using Microsoft.EntityFrameworkCore;
using StaApi.AutoGeneration.Models;
using StaApi.Models;

namespace StaApi.AutoGeneration.Context;

public class CoreContext : DbContext
{
    public CoreContext(DbContextOptions<CoreContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TopicModel>()
            .Property(x => x.TopicType)
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<TopicType>(v, true)
            );

        modelBuilder.Entity<SubtopicModel>()
            .Property(x => x.Difficulty)
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<DifficultyLevel>(v, true)
            );

        modelBuilder.Entity<AppUserModel>()
            .Property(x => x.Level)
            .HasConversion(
                v => v.ToString().ToUpperInvariant(),
                v => Enum.Parse<UserLevel>(v, true)
            );

        modelBuilder.Entity<UserAuthProviderModel>()
            .Property(x => x.Provider)
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<AuthProvider>(v, true)
            );

        
    }
    
    public DbSet<TopicModel> Topics { get; set; }
    public DbSet<SubtopicModel>  Subtopics { get; set; }
    public DbSet<ExerciseModel>  Exercises { get; set; }
    public DbSet<ExerciseTypeModel> ExercisesType { get; set; }
    public DbSet<ExerciseAttemptModel>  ExerciseAttempts { get; set; }
    
    public DbSet<UserSubtopicProgressModel>  UserSubtopicProgresses { get; set; }
}