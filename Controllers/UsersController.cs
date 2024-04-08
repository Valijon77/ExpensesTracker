using AutoMapper;
using ExpensesTracker.Data;
using ExpensesTracker.DTOs;
using ExpensesTracker.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Controllers;

public class UsersController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UsersController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    [HttpGet("{username}")]
    public ActionResult<UserInfoDto> GetUserbyUsername(string username)
    {
        // I: does not include related data if .Include() is not chained on.
        var user = _dataContext.Users
            .Include(u => u.Expenses)
            .SingleOrDefault(u => u.Username == username);

        if (user is null)
            return NotFound("User with specified username does not exist");

        return _mapper.Map<UserInfoDto>(user);
    }

    [HttpGet("userId:int")]
    public ActionResult<UserInfoDto> GetUserById(int userId) // W: Should I add GetUsers method?
    {
        var user = _dataContext.Users.Include(u => u.Expenses).SingleOrDefault(u => u.Id == userId);

        if (user is null)
        {
            return NotFound("User not found");
        }

        return _mapper.Map<UserInfoDto>(user);
    }

    [HttpDelete("{userId:int}")] // W: Should HttpDelete use 'User Id' or 'Username'? Which one is more convinient? O: Make it to require Admin credentials
    public ActionResult DeleteUser(int userId)
    {
        var user = _dataContext.Users.FirstOrDefault(u => u.Id == userId);

        if (user is null)
            return NotFound("User not found");

        _dataContext.Users.Remove(user);

        if (_dataContext.SaveChanges() > 0)
        {
            return Ok("User was deleted successfully!");
        }

        return BadRequest("Something went wrong!");
    }

    [HttpPut]
    public ActionResult UpdateUser(UserUpdateDto userUpdateDto)
    {
        // I: current user
        var user = _dataContext.Users.Find(User.GetUserId());

        if (user is null) // I: this line of code was included in tutorial.
            return NotFound();

        if (user.Username != userUpdateDto.Username) // I: ensuring username was not taken by someone else
        {
            var targetedUser = _dataContext.Users.SingleOrDefault(
                u => u.Username == userUpdateDto.Username
            );

            if (targetedUser is not null)
            {
                return BadRequest("Username was already taken");
            }
        }

        _mapper.Map(userUpdateDto, user); // I: Ensuring change to the database is always being made.

        if (_dataContext.SaveChanges() > 0)
        {
            return NoContent();
        }

        return BadRequest("Problem while saving changes to the database");
    }
}
