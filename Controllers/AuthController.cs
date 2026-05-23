using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaApi.Models;
using StaApi.Services;
using StaApi.Context;
using StaApi.Models.AuthApi;

namespace StaApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthContext _context;
    private readonly JwtService _jwtService;

    public AuthController(AuthContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    // ================= REGISTER =================
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestModel request)
    {
        var exists = await _context.Users
            .AnyAsync(x => x.UserName == request.UserName);

        if (exists)
            return BadRequest("User already exists");

        var user = new AppUserModel
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName
        };

        var hasher = new PasswordHasher<AppUserModel>();
        user.PasswordHash = hasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // ================= LOGIN =================
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequestModel request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.UserName == request.UserName);

        if (user == null)
            return Unauthorized();

        var hasher = new PasswordHasher<AppUserModel>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized();

        var jwt = _jwtService.Generate(user);
        var refresh = RefreshTokenService.Create();

        _context.RefreshTokens.Add(new RefreshTokenModel
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = RefreshTokenHasher.Hash(refresh),
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        });

        await _context.SaveChangesAsync();

        // ACCESS TOKEN COOKIE
        Response.Cookies.Append("accessToken", jwt, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });

        // REFRESH TOKEN COOKIE
        Response.Cookies.Append("refreshToken", refresh, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok(); 
    }

    // ================= REFRESH =================
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh()
    {
        var token = Request.Cookies["refreshToken"];

        if (token == null)
            return Unauthorized();

        var hash = RefreshTokenHasher.Hash(token);

        var stored = await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == hash);

        if (stored == null || stored.IsRevoked || stored.Expires < DateTime.UtcNow)
            return Unauthorized();

        stored.IsRevoked = true;

        var newJwt = _jwtService.Generate(stored.User);
        var newRefresh = RefreshTokenService.Create();

        _context.RefreshTokens.Add(new RefreshTokenModel
        {
            Id = Guid.NewGuid(),
            UserId = stored.UserId,
            TokenHash = RefreshTokenHasher.Hash(newRefresh),
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        });

        await _context.SaveChangesAsync();

        
        Response.Cookies.Append("accessToken", newJwt, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });

        Response.Cookies.Append("refreshToken", newRefresh, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok(); 
    }
    
    
}