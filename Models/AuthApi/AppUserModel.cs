namespace StaApi.Models.AuthApi;

public class AppUserModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}