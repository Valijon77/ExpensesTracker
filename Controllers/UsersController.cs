using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ExpensesTracker.Data;
using ExpensesTracker.DTOs;
using ExpensesTracker.Entities;
using ExpensesTracker.Extensions;
using ExpensesTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Controllers;

public class UsersController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public UsersController(DataContext dataContext, IMapper mapper, ITokenService tokenService)
    {
        _dataContext = dataContext;
        _mapper = mapper;
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

        // P: Password length must be greater or equal to 6
        // O: Use validation features of framework
        if (registerDto.Password!.Length < 6)
            return BadRequest("Password should be at least 6 characters");

        using var hmac = new HMACSHA512();

        User appUser =
            new()
            {
                Username = registerDto.Username!.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
            };

        _dataContext.Users.Add(appUser);

        if (_dataContext.SaveChanges() > 0)
        {
            return new UserDto
            {
                Username = appUser.Username,
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
            Token = _tokenService.CreateToken(appUser)
        };
    }

    [Authorize]
    [HttpGet("{username}")] // W: why do even I need this method?
    public ActionResult<UserInfoDto> GetUserbyUsername(string username)
    {
        // I: does not include related data
        var user = _dataContext.Users
            .Include(u => u.Expenses)
            .SingleOrDefault(u => u.Username == username);

        if (user is null)
            return NotFound("User with specified username does not exist");

        return _mapper.Map<UserInfoDto>(user);
    }
}
