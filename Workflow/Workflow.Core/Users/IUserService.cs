using Data.Users;

namespace Workflow.Core.Users;

public interface IUserService
{
    User CreateUser(CreateUserRequest request);
    
    bool VerifyPassword(UserDto user, string password);
}