namespace StaApi.Models.AuthApi;

public class RegisterRequestModel
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}