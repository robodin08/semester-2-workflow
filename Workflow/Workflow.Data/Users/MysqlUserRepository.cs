using Data.Users;
using MySql.Data.MySqlClient;

namespace Data.Users;

public class MysqlUserRepository(IDbConnectionFactory factory): IUserRepository
{
    public UserDto CreateUser(string email, string username, string passwordHash)
    {
        using var connection = factory.CreateOpenConnection();
        
        using var command = new MySqlCommand(@"
            INSERT INTO users (email, username, password_hash)
            VALUES (@email, @username, @password_hash);
            SELECT id, email, username, password_hash, created_at
            FROM users
            WHERE id = LAST_INSERT_ID();
        ", connection);
        
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@password_hash", passwordHash);
        
        using var reader = command.ExecuteReader();
        if (!reader.Read()) throw new Exception("Failed to create user.");
        
        var id = reader.GetInt32("id");
        var dbUsername = reader.GetString("username");
        var dbPassword = reader.GetString("password_hash");
        var dbEmail = reader.GetString("email");
        var createdAt = reader.GetDateTime("created_at");

        return new UserDto(id, dbEmail, dbPassword, dbUsername, createdAt);
    }
}