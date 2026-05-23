namespace StaApi.Models.AuthApi;

public class RefreshTokenModel
{
    public Guid Id { get; set; }
    public string TokenHash { get; set; } = null!;
    public Guid UserId { get; set; }

    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }

    public AppUserModel User { get; set; } = null!;
}

