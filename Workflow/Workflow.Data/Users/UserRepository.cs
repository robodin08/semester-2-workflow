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

    public UserModel Register(RegisterModel model)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                             INSERT INTO users (email, username, password_hash)
                                                             VALUES (@email, @username, @password_hash);
                                                             SELECT id, email, username, password_hash, created_at
                                                             FROM users
                                                             WHERE id = LAST_INSERT_ID();
                                             """, connection);

        command.Parameters.AddWithValue("@email", model.Email);
        command.Parameters.AddWithValue("@username", model.Username);
        command.Parameters.AddWithValue("@password_hash", model.PasswordHash);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) throw new Exception("Failed to create user.");

        var id = reader.GetInt32("id");
        var dbEmail = reader.GetString("email");
        var dbUsername = reader.GetString("username");
        var dbPassword = reader.GetString("password_hash");
        var createdAt = reader.GetDateTime("created_at");

        return new UserModel(id, dbEmail, dbUsername, dbPassword, createdAt);
    }

    public UserModel? GetUserByEmail(string email)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand($"""
                                                              SELECT id, email, username, password_hash, created_at
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
        var createdAt = reader.GetDateTime("created_at");

        return new UserModel(dbId, dbEmail, dbUsername, dbPassword, createdAt);
    }

    public UserModel? GetUserById(int id)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand($"""
                                                              SELECT id, email, username, password_hash, created_at
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
        var createdAt = reader.GetDateTime("created_at");

        return new UserModel(dbId, dbEmail, dbUsername, dbPassword, createdAt);
    }
}