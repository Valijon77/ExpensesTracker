using AutoMapper;
using ExpensesTracker.Data;
using ExpensesTracker.DTOs;
using ExpensesTracker.Entities;
using ExpensesTracker.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Controllers;

[Authorize]
public class ExpensesController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public ExpensesController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    // O: Modify it to fetch only one expense record from database or create such method.
    [HttpGet("history")]
    public async Task<ActionResult> GetExpensesRecord() // O: [FromQuery] int days (will it be considered pagination?)
    {
        var userId = User.GetUserId();

        var expensesHistory = await _dataContext.Expenses
            .Where(e => e.UserId == userId)
            .ToListAsync();

        var expenseDtos = _mapper.Map<IEnumerable<ExpenseDto>>(expensesHistory);

        return Ok(expenseDtos);
    }

    [HttpDelete("delete/{expenseId:int}")]
    public ActionResult DeleteExpenseRecord([FromRoute] int expenseId)
    {
        var userId = User.GetUserId();

        var expenseRecord = _dataContext.Expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
        bool belongsToUser = expenseRecord?.UserId == userId;

        if (expenseRecord is null || !belongsToUser)
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

    [HttpPost("create")]
    public ActionResult CreateExpenseRecord(CreateExpenseDto expenseDto)
    {
        var userId = User.GetUserId();

        if (expenseDto.WithdrawalAmount <= 0)
            return BadRequest("Expenditure amount must be greater than zero.");

        // TODO: validate the Type and PaymentMethods also.
        var expenseRecord = _mapper.Map<Expenses>(expenseDto);
        expenseRecord.UserId = userId;

        _dataContext.Expenses.Add(expenseRecord);

        if (_dataContext.SaveChanges() > 0)
        {
            return Ok(_mapper.Map<ExpenseDto>(expenseRecord));
        }

        return BadRequest("Problem while saving changes to the database");
    }

    [HttpPut("update")]
    public ActionResult UpdateExpenseRecord(ExpenseDto updateExpenseDto)
    {
        var userId = User.GetUserId();

        // I: Validating inside controller causing code duplication (was mentioned in the book).

        var expenseRecord = _dataContext.Expenses.FirstOrDefault(
            e => e.ExpenseId == updateExpenseDto.ExpenseId
        );

        bool belongsToUser = expenseRecord?.UserId == userId;

        if (expenseRecord is null || !belongsToUser)
            return NotFound();

        _mapper.Map(updateExpenseDto, expenseRecord);

        if (_dataContext.SaveChanges() > 0)
        {
            return NoContent();
        }

        return BadRequest("Problem while saving changes to the database");
    }
}
