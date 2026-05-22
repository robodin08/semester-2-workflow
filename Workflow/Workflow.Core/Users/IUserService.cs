namespace Workflow.Core.Users;

public interface IUserService
{
    User Register(RegisterRequest request);
    
    User Login(LoginRequest request);
    
    User? GetUserById(int id);
}