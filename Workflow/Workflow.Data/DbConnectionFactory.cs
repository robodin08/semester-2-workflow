using MySql.Data.MySqlClient;

namespace Data;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public MySqlConnection CreateConnection()
    {
        return new MySqlConnection(connectionString);
    }

    public MySqlConnection CreateOpenConnection()
    {
        var connection = CreateConnection();
        connection.Open();
        return connection;
    }
}