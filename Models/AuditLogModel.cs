namespace StaApi.Models;

public class AuditLogModel
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public string EventType { get; set; } = null!;

    public string Severity { get; set; } = "INFO";

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? Message { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}