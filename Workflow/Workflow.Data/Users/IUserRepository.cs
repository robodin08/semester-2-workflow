namespace Data.Users;

public interface IUserRepository
{
    bool UserExistsByEmail(string email);
    bool UserExistsByUsername(string username);
    
    UserDto Register(RegisterDto dto);
    
    UserDto? GetUserByEmail(string email);
    
    UserDto? GetUserById(int id);

    void UpdatePasswordHash(int userId, string newPasswordHash);
}