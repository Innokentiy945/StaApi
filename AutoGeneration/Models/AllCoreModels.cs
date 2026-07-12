using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StaApi.Models;

namespace StaApi.AutoGeneration.Models;

#region AUTH

[Table("app_users")]
public class AppUserModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("email")]
    public string Email { get; set; } = default!;

    [Column("display_name")]
    public string DisplayName { get; set; } = default!;

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [Column("native_language_code")]
    public string NativeLanguageCode { get; set; } = default!;

    [Column("level")]
    public UserLevel Level { get; set; } = UserLevel.A1;

    [Column("xp")]
    public uint Xp { get; set; }

    [Column("streak_days")]
    public uint StreakDays { get; set; }

    [Column("token_version")]
    public uint TokenVersion { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("is_verified")]
    public bool IsVerified { get; set; }

    [Column("last_login_at")]
    public DateTime? LastLoginAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<UserAuthProviderModel> AuthProviders { get; set; } = new List<UserAuthProviderModel>();
    public ICollection<UserSessionModel> Sessions { get; set; } = new List<UserSessionModel>();
    public ICollection<ExerciseAttemptModel> ExerciseAttempts { get; set; } = new List<ExerciseAttemptModel>();
    public ICollection<UserSubtopicProgressModel> SubtopicProgresses { get; set; } = new List<UserSubtopicProgressModel>();
}

[Table("user_auth_providers")]
public class UserAuthProviderModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("user_id")]
    public ulong UserId { get; set; }

    [Column("provider")]
    public AuthProvider Provider { get; set; }

    [Column("provider_user_id")]
    public string ProviderUserId { get; set; } = default!;

    [Column("password_hash")]
    public string? PasswordHash { get; set; }

    [Column("email_verified")]
    public bool EmailVerified { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public AppUserModel AppUser { get; set; } = default!;
}

[Table("user_sessions")]
public class UserSessionModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("user_id")]
    public ulong UserId { get; set; }

    [Column("refresh_token_hash")]
    public string RefreshTokenHash { get; set; } = default!;

    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [Column("ip_address")]
    public string? IpAddress { get; set; }

    [Column("user_agent")]
    public string? UserAgent { get; set; }

    [Column("device_name")]
    public string? DeviceName { get; set; }

    [Column("revoked")]
    public bool Revoked { get; set; }

    [Column("last_used_at")]
    public DateTime? LastUsedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public AppUserModel AppUser { get; set; } = default!;
}

#endregion

#region TOPICS

[Table("topics")]
public class TopicModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = default!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("topic_type")]
    public TopicType TopicType { get; set; }

    [Column("order_index")]
    public uint OrderIndex { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<SubtopicModel> Subtopics { get; set; } = new List<SubtopicModel>();
}

[Table("subtopics")]
public class SubtopicModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("topic_id")]
    public ulong TopicId { get; set; }

    [Column("title")]
    public string Title { get; set; } = default!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("difficulty")]
    public DifficultyLevel Difficulty { get; set; }

    [Column("order_index")]
    public uint OrderIndex { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey(nameof(TopicId))]
    public TopicModel Topic { get; set; } = default!;

    public ICollection<ExerciseModel> Exercises { get; set; } = new List<ExerciseModel>();
}

#endregion

#region EXERCISES

[Table("exercise_types")]
public class ExerciseTypeModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("code")]
    public string Code { get; set; } = default!;

    [Column("name")]
    public string Name { get; set; } = default!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

[Table("exercises")]
public class ExerciseModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("subtopic_id")]
    public ulong SubtopicId { get; set; }

    [Column("exercise_type_id")]
    public ulong ExerciseTypeId { get; set; }

    [Column("source_type")]
    public string SourceType { get; set; } = "manual";

    [Column("difficulty")]
    public string Difficulty { get; set; } = "easy";

    [Column("xp_reward")]
    public uint XpReward { get; set; } = 10;

    [Column("data_json")]
    public string DataJson { get; set; } = string.Empty;

    [Column("order_index")]
    public uint OrderIndex { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey(nameof(SubtopicId))]
    public SubtopicModel Subtopic { get; set; } = default!;

    [ForeignKey(nameof(ExerciseTypeId))]
    public ExerciseTypeModel ExerciseType { get; set; } = default!;

    // UPDATED: exercise_answers table removed
    public List<ExerciseAttemptModel> Attempts { get; set; } = new();
}

[Table("exercise_attempts")]
public class ExerciseAttemptModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("user_id")]
    public ulong UserId { get; set; }

    [Column("subtopic_id")]
    public ulong SubtopicId { get; set; }

    [Column("exercise_id")]
    public ulong? ExerciseId { get; set; }

    [Column("user_answer")]
    public string? UserAnswer { get; set; }

    [Column("is_correct")]
    public bool IsCorrect { get; set; }

    [Column("score_percent", TypeName = "decimal(5,2)")]
    public decimal ScorePercent { get; set; }

    [Column("xp_earned")]
    public uint XpEarned { get; set; }

    [Column("time_spent_seconds")]
    public uint TimeSpentSeconds { get; set; }

    [Column("answered_at")]
    public DateTime AnsweredAt { get; set; }
}

#endregion

#region PROGRESS

[Table("user_subtopic_progress")]
public class UserSubtopicProgressModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("user_id")]
    public ulong UserId { get; set; }

    [Column("subtopic_id")]
    public ulong SubtopicId { get; set; }

    [Column("progress_percent")]
    public decimal ProgressPercent { get; set; }

    [Column("completed")]
    public bool Completed { get; set; }

    [Column("stars")]
    public uint Stars { get; set; }

    [Column("last_activity_at")]
    public DateTime? LastActivityAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

#endregion