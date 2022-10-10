using System;
using System.Text;

namespace Database;
public class Connection
{
  private static readonly string server = "127.0.0.1,3306";
  private static readonly string database = "FinSystem";
  private static readonly string user = "root";
  private static readonly string password = "docker";

  public static string GetStringConnection() {
    StringBuilder connectionString = new StringBuilder();
    connectionString.Append($"Server={server};");
    connectionString.Append($"Database={database};");
    connectionString.Append($"User Id={user};");
    connectionString.Append($"Password={password};");
    return connectionString.ToString();
  }
}
