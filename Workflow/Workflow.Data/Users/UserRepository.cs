using MySql.Data.MySqlClient;

namespace Data.Users
{
    public class UserRepository(IDbConnectionFactory factory) : IUserRepository
    {
        public UserModel CreateUser(CreateUserModel model)
        {
            using var connection = factory.CreateOpenConnection();

            using var command = new MySqlCommand(@"
                INSERT INTO users (email, username, password_hash)
                VALUES (@email, @username, @password_hash);
                SELECT id, email, username, password_hash, created_at
                FROM users
                WHERE id = LAST_INSERT_ID();
            ", connection);

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

            return new UserModel(id, dbEmail, dbPassword, dbUsername, createdAt);
        }

        public UserModel GetUserByEmail(string email)
        {
            return GetUserByField("email", email);
        }

        public UserModel GetUserByUsername(string username)
        {
            return GetUserByField("username", username);
        }
        
        private UserModel GetUserByField(string fieldName, string value)
        {
            using var connection = factory.CreateOpenConnection();

            using var command = new MySqlCommand($@"
                SELECT id, email, username, password_hash, created_at
                FROM users
                WHERE {fieldName} = @{fieldName};
            ", connection);

            command.Parameters.AddWithValue($"@{fieldName}", value);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) throw new Exception($"User with {fieldName} '{value}' not found.");

            var id = reader.GetInt32("id");
            var dbEmail = reader.GetString("email");
            var dbUsername = reader.GetString("username");
            var dbPassword = reader.GetString("password_hash");
            var createdAt = reader.GetDateTime("created_at");

            return new UserModel(id, dbEmail, dbPassword, dbUsername, createdAt);
        }
    }
}