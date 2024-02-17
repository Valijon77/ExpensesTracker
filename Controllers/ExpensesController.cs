using AutoMapper;
using ExpensesTracker.Data;
using ExpensesTracker.DTOs;
using ExpensesTracker.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Controllers;

public class ExpensesController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public ExpensesController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    // O: Modify it to fetch only one expense record from database.
    [HttpGet("expenses-history/{userId}")] // I: Not safe because any other user can manipulate segment variables. Tokens should be introduced.
    public async Task<ActionResult> GetExpensesRecord([FromRoute] int userId) // O: [FromQuery] int days (will it be considered pagination?)
    {
        bool userExists = _dataContext.Users.Any(u => u.Id == userId);
        if (!userExists)
            return NotFound("User does not exist");

        var expensesHistory = await _dataContext.Expenses
            .Where(e => e.UserId == userId)
            .ToListAsync();

        var expenseDtos = _mapper.Map<IEnumerable<ExpenseDto>>(expensesHistory);

        return Ok(expenseDtos);
    }

    [HttpDelete("delete-expense-record/{userId:int}")]
    public ActionResult DeleteExpenseRecord([FromRoute] int userId, [FromQuery] int expenseId)
    {
        var currentUser = _dataContext.Users // O: Delete record without fetching user object.
            .Include(u => u.Expenses)
            .FirstOrDefault(u => u.Id == userId);

        var expenseRecord = currentUser?.Expenses?.FirstOrDefault(e => e.ExpenseId == expenseId);

        if (currentUser is null || expenseRecord is null)
        {
            return NotFound();
        }

        _dataContext.Expenses.Remove(expenseRecord);

        if (_dataContext.SaveChanges() > 0)
        {
            return Ok("Expense record was deleted successfully");
        }

        return BadRequest("Problem while deleting record");
    }

    [HttpDelete("delete-expense-recordd")] // W: what should HttpDelete return?
    public ActionResult DeleteExpenseRecordd(int userId, int expenseId)
    {
        var currentUser = _dataContext.Users.Find(userId);
        if (currentUser is null)
        {
            return NotFound("User not found");
        }

        // [O]: check if current user has expense record to be deleted in its expenses history.
        if (currentUser.Expenses.Any(e => e.ExpenseId == expenseId))
        {
            return NotFound();
        }

        var expenseRecord = _dataContext.Expenses.FirstOrDefault(e => e.ExpenseId == expenseId);

        if (expenseRecord is null)
        {
            return NotFound("Expense record not found");
        }

        _dataContext.Remove(expenseRecord);

        if (_dataContext.SaveChanges() > 0)
        {
            return Ok("Expense record was deleted successfully");
        }

        return BadRequest("Problem while deleting record");
    }

    [HttpPost("create-expense-record/{userId:int}")]
    public ActionResult CreateExpenseRecord(CreateExpenseDto expenseDto, [FromRoute] int userId) // O: Modify it to use DTOs. Circular references won't be a problem anymore.
    {
        // [W]: won't it cause circular reference problem if the user object is not retrieved from database: NO!
        // var user = _dataContext.Users.Find(userId);

        // [O]: don't retrieve whole object. Find the way to check if user with userId exists or not:
        bool userExists = _dataContext.Users.Any(u => u.Id == userId);

        if (!userExists)
            return NotFound("User not found");

        if (expenseDto.WithdrawalAmount <= 0)
            return BadRequest("Expenditure amount must be greater than zero.");

        // TODO: validate the Type and PaymentMethods also.

        // var expenseRecord = (Expenses)expenseDto; // I: added explicit conversion operator in Expenses entity.
        var expenseRecord = _mapper.Map<Expenses>(expenseDto);
        // expenseRecord.User = user; // I: works just same as below line of code.
        expenseRecord.UserId = userId;

        _dataContext.Expenses.Add(expenseRecord);

        if (_dataContext.SaveChanges() > 0)
        {
            // I1: Breaking circular reference;
            // expenseRecord.User.Expenses = null;

            // I2: I1
            // user.Expenses = null;
            return Ok(_mapper.Map<ExpenseDto>(expenseRecord));
        }

        return BadRequest("Problem while saving changes to the database");
    }

    [HttpPost("create-expense-recordd/{userId:int}")]
    public ActionResult CreateExpenseRecordd(CreateExpenseDto expenseDto, [FromRoute] int userId)
    {
        // I: Include() causing circular referencing problem.
        var user = _dataContext.Users.Include(u => u.Expenses).FirstOrDefault(u => u.Id == userId);

        if (user is null)
            return NotFound("User not found");

        if (expenseDto.WithdrawalAmount <= 0)
            return BadRequest("Expenditure amount must be greater than zero.");

        var expenseRecord = (Expenses)expenseDto;

        // I: changes started reflecting to the database after adding .Include() method in the above code.
        user.Expenses.Add(expenseRecord);

        if (_dataContext.SaveChanges() > 0)
        {
            // I: Preventing circular reference problem
            foreach (var e in user.Expenses)
            {
                e.User = null;
            }
            return Ok(user);
        }

        return BadRequest("Problem while saving changes to the database");
    }

    [HttpPut("update-expense-record/{userId:int}")]
    public ActionResult UpdateExpenseRecord(
        [FromRoute] int userId,
        ExpenseDto updateExpenseDto
    )
    {
        // I: Validating inside controller causing code duplication (was mentioned in book).
        var user = _dataContext.Users
            // .AsNoTracking()
            .Include(u => u.Expenses)
            .FirstOrDefault(u => u.Id == userId);

        // var user2 = _dataContext.Users.Find(userId);

        // I: it is being tracked be EF Core.
        var expenseRecord = user?.Expenses?.FirstOrDefault(
            e => e.ExpenseId == updateExpenseDto.ExpenseId
        );

        if (user is null || expenseRecord is null)
            return NotFound();

        // expenseRecord.WithdrawalAmount = updateExpenseDto.WithdrawalAmount;
        // ***
        // I: 2 ways to update 'Expenses' table
        // expenseRecord = (Expenses)updateExpenseDto;
        // expenseRecord.UserId = userId; // O1: redundant work. find the way to map without touching to these properties
        // expenseRecord.User = user; // O2: O1
        // ***

        _mapper.Map(updateExpenseDto, expenseRecord);

        // _dataContext.Expenses.Update(expenseRecord);

        if (_dataContext.SaveChanges() > 0)
        {
            return NoContent();
        }

        return BadRequest("Problem while saving changes to the database");
    }
}
