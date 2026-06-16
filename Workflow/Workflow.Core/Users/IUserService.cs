namespace Workflow.Core.Users;

public interface IUserService
{
    UserResponse Register(RegisterRequest request);
    
    UserResponse Login(LoginRequest request);
    
    UserResponse GetUserById(int id);

    void ChangePassword(int userId, string currentPassword, string newPassword);
}