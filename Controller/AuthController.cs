using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaApi.Context;
using StaApi.Models;
using StaApi.Services;

namespace StaApi.Controller;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    private readonly AuditService _audit;

    public AuthController(AppDbContext db, AuditService audit)
    {
        _db = db;
        _audit = audit;
    }

    // =========================================
    // REGISTER
    // =========================================

    [HttpPost("register")]
    public async Task<IActionResult> Register(string username, string password)
    {
        var exists = await _db.Users
            .AnyAsync(x => x.Username == username);

        if (exists)
        {
            return BadRequest("UserModel already exists");
        }

        // 🔐 HASH PASSWORD
        var hash =
            BCrypt.Net.BCrypt.HashPassword(password);

        var user = new UserModel
        {
            Username = username,

            PasswordHash = hash
        };

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        await _audit.Log(
            "REGISTER_SUCCESS",
            "INFO",
            user.Id);

        return Ok();
    }

    // =========================================
    // LOGIN
    // =========================================

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        string username,
        string password)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(
                x => x.Username == username);

        if (user == null)
        {
            await _audit.Log(
                "LOGIN_FAIL",
                "WARNING",
                null,
                "user not found");

            return Unauthorized();
        }

        // 🔐 VERIFY PASSWORD
        var valid =
            BCrypt.Net.BCrypt.Verify(
                password,
                user.PasswordHash);

        if (!valid)
        {
            await _audit.Log(
                "LOGIN_FAIL",
                "WARNING",
                user.Id,
                "wrong password");

            return Unauthorized();
        }

        await _audit.Log(
            "LOGIN_SUCCESS",
            "INFO",
            user.Id);

        return Ok("Login success");
    }
}