using AutoMapper;
using ExpensesTracker.DTOs;
using ExpensesTracker.Entities;

namespace ExpensesTracker.Helpers;

public class AutoMapperProfiles:Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<ExpenseDto, Expenses>();
        CreateMap<Expenses, ExpenseDto>();
        CreateMap<CreateExpenseDto, Expenses>();
    }
}