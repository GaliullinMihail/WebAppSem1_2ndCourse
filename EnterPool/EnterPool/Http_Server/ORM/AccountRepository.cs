using System.Data.SqlClient;
using EnterPool.Http_Server.models;

namespace EnterPool.Http_Server.ORM;

public class AccountRepository
{
    private readonly string connectionString;
    private readonly List<Account> accounts;

    public AccountRepository(string connectionString)
    {
        this.connectionString = connectionString;
        accounts = new List<Account>();

        var sqlExpression = "SELECT * FROM Users";
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var command = new SqlCommand(sqlExpression, connection);
        using var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                accounts.Add(new Account(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2)));
            }
        }
    }

    public IEnumerable<Account> GetAll() => accounts;

    public Account? GetById(int id) => accounts.Find(account => id == account.Id);

    public void Insert(string login, string password)
    {
        string sqlExpression =
            $"INSERT INTO Users(login, password) " +
            $"VALUES('{login}', '{password}')";

        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var command = new SqlCommand(sqlExpression, connection);
        command.ExecuteNonQuery();

        UpdateList();
    }
    
    private void UpdateList()
    {
        var sqlExpression = $"SELECT TOP 1 * FROM Users ORDER BY id DESC";
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var command = new SqlCommand(sqlExpression, connection);
        var reader = command.ExecuteReader();

        reader.Read();

        accounts.Add(new Account(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2)));
    }
}