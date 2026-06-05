namespace Data.Users;

public interface IUserRepository
{
    bool UserExistsByEmail(string email);
    bool UserExistsByUsername(string username);
    
    UserModel Register(RegisterModel model);
    
    UserModel? GetUserByEmail(string email);
    
    UserModel? GetUserById(int id);
}