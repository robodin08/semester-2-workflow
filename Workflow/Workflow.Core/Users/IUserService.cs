namespace Workflow.Core.Users;

public interface IUserService
{
    User CreateUser(CreateUserRequest request);
}