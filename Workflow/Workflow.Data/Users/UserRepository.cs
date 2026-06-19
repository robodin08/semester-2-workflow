using MySql.Data.MySqlClient;

namespace Data.Users;

public class UserRepository(IDbConnectionFactory factory) : IUserRepository
{
    public bool UserExistsByEmail(string email)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                     SELECT 1
                                                     FROM users
                                                     WHERE email = @email
                                                     LIMIT 1;
                                             """, connection);

        command.Parameters.AddWithValue("@email", email);

        var result = command.ExecuteScalar();

        return result != null;
    }

    public bool UserExistsByUsername(string username)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                     SELECT 1
                                                     FROM users
                                                     WHERE username = @username
                                                     LIMIT 1;
                                             """, connection);

        command.Parameters.AddWithValue("@username", username);

        var result = command.ExecuteScalar();

        return result != null;
    }

    public UserDto Register(RegisterDto dto)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                              INSERT INTO users (email, username, password_hash, role)
                                                              VALUES (@email, @username, @password_hash, @role);
                                                              SELECT id, email, username, password_hash, role, created_at
                                                              FROM users
                                                              WHERE id = LAST_INSERT_ID();
                                              """, connection);

        command.Parameters.AddWithValue("@email", dto.Email);
        command.Parameters.AddWithValue("@username", dto.Username);
        command.Parameters.AddWithValue("@password_hash", dto.PasswordHash);
        command.Parameters.AddWithValue("@role", "Employee");

        using var reader = command.ExecuteReader();
        if (!reader.Read()) throw new Exception("Failed to create user.");

        var id = reader.GetInt32("id");
        var dbEmail = reader.GetString("email");
        var dbUsername = reader.GetString("username");
        var dbPassword = reader.GetString("password_hash");
        var dbRoleId = reader.GetString("role");
        var createdAt = reader.GetDateTime("created_at");

        return new UserDto(id, dbEmail, dbUsername, dbPassword, dbRoleId, createdAt);
    }

    public UserDto? GetUserByEmail(string email)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand($"""
                                                               SELECT id, email, username, password_hash, role, created_at
                                                               FROM users
                                                               WHERE email = @email;
                                               """, connection);

        command.Parameters.AddWithValue($"@email", email);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;

        var dbId = reader.GetInt32("id");
        var dbEmail = reader.GetString("email");
        var dbUsername = reader.GetString("username");
        var dbPassword = reader.GetString("password_hash");
        var dbRoleId = reader.GetString("role");
        var createdAt = reader.GetDateTime("created_at");

        return new UserDto(dbId, dbEmail, dbUsername, dbPassword, dbRoleId, createdAt);
    }

    public UserDto? GetUserById(int id)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand($"""
                                                               SELECT id, email, username, password_hash, role, created_at
                                                               FROM users
                                                               WHERE id = @id;
                                               """, connection);

        command.Parameters.AddWithValue($"@id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;

        var dbId = reader.GetInt32("id");
        var dbEmail = reader.GetString("email");
        var dbUsername = reader.GetString("username");
        var dbPassword = reader.GetString("password_hash");
        var dbRoleId = reader.GetString("role");
        var createdAt = reader.GetDateTime("created_at");

        return new UserDto(dbId, dbEmail, dbUsername, dbPassword, dbRoleId, createdAt);
    }

    public List<UserDto> GetAllUsers()
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      SELECT id, email, username, password_hash, role, created_at
                                                      FROM users
                                                      ORDER BY username;
                                              """, connection);

        using var reader = command.ExecuteReader();
        var users = new List<UserDto>();
        while (reader.Read())
        {
            var id = reader.GetInt32("id");
            var email = reader.GetString("email");
            var username = reader.GetString("username");
            var passwordHash = reader.GetString("password_hash");
            var roleId = reader.GetString("role");
            var createdAt = reader.GetDateTime("created_at");

            users.Add(new UserDto(id, email, username, passwordHash, roleId, createdAt));
        }

        return users;
    }

    public void UpdatePasswordHash(int userId, string newPasswordHash)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      UPDATE users
                                                      SET password_hash = @password_hash
                                                      WHERE id = @id;
                                              """, connection);

        command.Parameters.AddWithValue("@password_hash", newPasswordHash);
        command.Parameters.AddWithValue("@id", userId);

        command.ExecuteNonQuery();
    }
}