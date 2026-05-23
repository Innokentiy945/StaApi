namespace StaApi.Models.AuthApi;

public class LoginRequestModel
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}