using ExpensesTracker.Data;
using ExpensesTracker.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers;

public class UsersController : BaseApiController
{
    private readonly DataContext _dataContext;

    public UsersController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost("register")]
    public ActionResult RegisterUser(User user)
    {
        // P: Username should be unique
        var sameUser = _dataContext.Users.FirstOrDefault(u => u.Username == user.Username);
        if (sameUser is not null)
            return BadRequest("User with specified username already exists");

        // P: Password length must be greater or equal to 6
        // O: Use validation features of framework
        if (user.Password.Length < 6)
            return BadRequest("Password should be at least 6 characters");

        _dataContext.Users.Add(user);

        if (_dataContext.SaveChanges() > 0)
            return Ok(user);

        return BadRequest("Problem while saving changes to the database.");  // O: introduce a static list to store list of registered users: enables fast testing
    }

    [HttpPost("login")]
    public ActionResult LoginUser(User user)
    {
        var userDb = _dataContext.Users.SingleOrDefault(u => u.Username == user.Username);

        if (userDb is null || userDb.Password != user.Password)
        {
            return Unauthorized("Username or password is incorrect");
        }

        return Ok("You are now authenticated.");
    }

    [HttpGet("{username}")]
    public ActionResult<User> GetUserbyUsername(string username)
    {
        // I: does not include related data
        var user = _dataContext.Users.SingleOrDefault(u => u.Username == username);
        if (user is null)
            return NotFound("User with specified username does not exist");

        return Ok(user);
    }
}
