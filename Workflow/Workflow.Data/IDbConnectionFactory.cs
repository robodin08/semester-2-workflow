using MySql.Data.MySqlClient;

namespace Data;

public interface IDbConnectionFactory
{
    MySqlConnection CreateConnection();
}