using System;
using System.Text;
using MySqlConnector;
using Database;
using TransactionModel = Model.Transaction;
using CategoryModel = Model.Category;

namespace Persistence;
public class Transaction
{
  private MySqlConnection conn;
  private Category category;

  public Transaction(MySqlConnection conn) {
    string connectionString = Connection.GetStringConnection();
    this.conn = conn;
    this.category = new Category(new MySqlConnection(connectionString));
  }

  public List<TransactionModel> listAllTransactions()
  {
    List<TransactionModel> transactions = new List<TransactionModel>();
    StringBuilder commandStr = new StringBuilder("");
    commandStr.AppendLine("SELECT * FROM Transaction");
    commandStr.AppendLine("INNER JOIN Category");
    commandStr.AppendLine("WHERE Transaction.category_id = Category.id");
    MySqlCommand command = new MySqlCommand(commandStr.ToString(), this.conn);
    this.conn.Open();

    using(MySqlDataReader dataReader = command.ExecuteReader())
    {
      while (dataReader.Read())
      {
        int category_id = Convert.ToInt32(dataReader["category_id"].ToString());
        TransactionModel transaction = new TransactionModel
        {
          id = Convert.ToInt32(dataReader["id"].ToString())!,
          description = dataReader["description"].ToString(),
          type = Convert.ToChar(dataReader["type"].ToString()!),
          value = Convert.ToDouble(dataReader["value"].ToString()),
          category = this.category.GetCategory(category_id),
          expiration_date = Convert.ToDateTime(dataReader["expiration_date"].ToString())
        };
        transactions.Add(transaction);
      }
    }

    this.conn.Close();
    return transactions;
  }
}
