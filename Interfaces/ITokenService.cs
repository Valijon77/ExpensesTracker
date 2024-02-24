using ExpensesTracker.Entities;

namespace ExpensesTracker.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}