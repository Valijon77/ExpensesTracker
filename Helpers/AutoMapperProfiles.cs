using AutoMapper;
using ExpensesTracker.DTOs;
using ExpensesTracker.Entities;

namespace ExpensesTracker.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<ExpenseDto, Expenses>().ReverseMap();
        CreateMap<CreateExpenseDto, Expenses>();
        CreateMap<User, UserDto>();
        CreateMap<User, UserInfoDto>();
    }
}
