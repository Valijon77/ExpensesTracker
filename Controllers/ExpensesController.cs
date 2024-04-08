using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExpensesTracker.Data;
using ExpensesTracker.DTOs;
using ExpensesTracker.Entities;
using ExpensesTracker.Extensions;
using ExpensesTracker.Helpers;
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

    // O: Modify it to fetch only one expense record from database or create such method.

    [HttpGet("history")]
    public async Task<ActionResult<PagedList<ExpenseDto>>> GetExpenseRecords(
        [FromQuery] ExpenseParams expenseParams
    )
    {
        var userId = User.GetUserId();

        var expenseDtosHistoryQuery = _dataContext.Expenses
            .Where(e => e.UserId == userId)
            .ProjectTo<ExpenseDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .AsQueryable();

        var pagedList = await PagedList<ExpenseDto>.CreateAsync(
            expenseDtosHistoryQuery,
            expenseParams.PageNumber,
            expenseParams.PageSize
        );

        Response.AddPaginationHeader(
            new PaginationHeader(
                pagedList.CurrentPage,
                pagedList.PageSize,
                pagedList.TotalPages,
                pagedList.TotalCount
            )
        );

        return Ok(pagedList);
    }

    [HttpDelete("delete/{expenseId:int}")]
    public ActionResult DeleteExpenseRecord([FromRoute] int expenseId)
    {
        var userId = User.GetUserId();

        var expenseRecord = _dataContext.Expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
        bool belongsToUser = expenseRecord?.UserId == userId; // [W]: what will 'belongsToUser' contain? Null or false? : false

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
    public ActionResult CreateExpenseRecord(CreateExpenseDto createExpenseDto)
    {
        var userId = User.GetUserId();

        if (createExpenseDto.WithdrawalAmount <= 0)
            return BadRequest("Expenditure amount must be greater than zero.");

        // [TODO]: validate the Type and PaymentMethods also.
        var expenseRecord = _mapper.Map<Expense>(createExpenseDto);
        expenseRecord.UserId = userId;

        _dataContext.Expenses.Add(expenseRecord);

        if (_dataContext.SaveChanges() > 0)
        {
            return Ok(_mapper.Map<ExpenseDto>(expenseRecord));
        }

        return BadRequest("Problem while saving changes to the database");
    }

    [HttpPut("update")]
    public ActionResult UpdateExpenseRecord(UpdateExpenseDto updateExpenseDto)
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
