using Data.Users;

namespace Workflow.Core.Users;

public interface IUserService
{
    User CreateUser(CreateUserRequest request);

    User GetUserByEmail(string email);
    
    User GetUserByUsername(string username);
}