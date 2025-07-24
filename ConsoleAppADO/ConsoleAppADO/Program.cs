using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main()
    {
        var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();

        string connectionString = config.GetConnectionString("DefaultConnection");
        
        AddUser("Sara", "Sara@email.com", connectionString);
        ReadUsers(connectionString);
    }

    static void AddUser(string name, string email, string connectionString)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        string sql = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)";
        using SqlCommand cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Email", email);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} row(s) inserted.");
    }

    static void ReadUsers(string connectionString)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM Users";
        using SqlCommand cmd = new SqlCommand(sql, connection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["Name"]}, Email: {reader["Email"]}");
        }
    }
}
