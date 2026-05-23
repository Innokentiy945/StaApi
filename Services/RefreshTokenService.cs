using System.Security.Cryptography;
using System.Text;

namespace StaApi.Services;

public static class RefreshTokenService
{
    public static string Create()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}