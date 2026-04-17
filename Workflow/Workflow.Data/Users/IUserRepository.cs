namespace Data.Users;

public interface IUserRepository
{
    UserModel CreateUser(CreateUserModel model);
    
    UserModel GetUserByEmail(string email);
    
    UserModel GetUserByUsername(string username);
}