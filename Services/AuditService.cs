using StaApi.Context;
using StaApi.Models;

namespace StaApi.Services;

public class AuditService
{
    private readonly AppDbContext _db;

    private readonly IHttpContextAccessor _http;

    public AuditService(
        AppDbContext db,
        IHttpContextAccessor http)
    {
        _db = db;
        _http = http;
    }

    public async Task Log(
        string eventType,
        string severity = "INFO",
        long? userId = null,
        string? message = null)
    {
        var ctx = _http.HttpContext;

        var log = new AuditLogModel
        {
            UserId = userId,

            EventType = eventType,

            Severity = severity,

            Message = message,

            IpAddress =
                ctx?.Connection.RemoteIpAddress?.ToString(),

            UserAgent =
                ctx?.Request.Headers.UserAgent.ToString(),

            CreatedAt = DateTime.UtcNow
        };

        _db.AuditLogs.Add(log);

        await _db.SaveChangesAsync();
    }
}