using AutoMapper;
using ExpensesTracker.DTOs;
using ExpensesTracker.Entities;

namespace ExpensesTracker.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Expense, ExpenseDto>().ReverseMap();

        CreateMap<CreateExpenseDto, Expense>();

        CreateMap<UpdateExpenseDto, Expense>();

        CreateMap<User, UserDto>();

        CreateMap<User, UserInfoDto>()
            .ForMember(dest => dest.MemberSince, opt => opt.MapFrom(src => src.CreatedDateTime))
            .ForMember(
                dest => dest.ProfileUpdated,
                opt => opt.MapFrom(src => src.LastModifiedDateTime)
            );

        CreateMap<UserUpdateDto, User>();
    }
}
