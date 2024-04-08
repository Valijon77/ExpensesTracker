using System.Security.Cryptography;
using System.Text;
using ExpensesTracker.Data;
using ExpensesTracker.DTOs;
using ExpensesTracker.Entities;
using ExpensesTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers;

[AllowAnonymous]
public class AccountController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext dataContext, ITokenService tokenService)
    {
        _dataContext = dataContext;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public ActionResult<UserDto> RegisterUser(RegisterDto registerDto)
    {
        // P: Username should be unique
        var currentUser = _dataContext.Users.FirstOrDefault(
            u => u.Username == registerDto.Username!.ToLower()
        );

        // O: Multi-threading framework feature to test that if username already exists as user types it along the way.
        if (currentUser is not null)
            return BadRequest("User with specified username already exists");

        using var hmac = new HMACSHA512();

        User appUser =
            new()
            {
                Username = registerDto.Username!.ToLower(),
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                Country = registerDto.Country,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
            };

        _dataContext.Users.Add(appUser);

        if (_dataContext.SaveChanges() > 0)
        {
            return new UserDto
            {
                Username = appUser.Username,
                FullName = appUser.FullName,
                Email = appUser.Email,
                Token = _tokenService.CreateToken(appUser)
            };
        }

        return BadRequest("Problem while saving changes to the database."); // O: introduce a static list to store list of registered users: enables fast testing
    }

    [HttpPost("login")]
    public ActionResult<UserDto> LoginUser(LoginDto loginDto)
    {
        var appUser = _dataContext.Users.SingleOrDefault(u => u.Username == loginDto.Username);

        if (appUser is null)
            return Unauthorized("Invalid username");

        var hmac = new HMACSHA512(appUser.PasswordSalt);

        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password!));

        for (int i = 0; i < computedHash.Length; ++i)
        {
            if (computedHash[i] != appUser.PasswordHash[i])
                return Unauthorized("Invalid password");
        }

        return new UserDto
        {
            Username = appUser.Username,
            FullName = appUser.FullName,
            Email = appUser.Email,
            Token = _tokenService.CreateToken(appUser)
        };
    }
}
