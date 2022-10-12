using System;
using System.Text;
using MySqlConnector;
using Database;
using Util = Utils.Utils;
using TransactionModel = Model.Transaction;
using CategoryModel = Model.Category;

namespace Persistence;
public class Transaction
{
  private readonly MySqlConnection conn;
  private readonly Category category;

  public Transaction(MySqlConnection conn) {
    string connectionString = Connection.GetStringConnection();
    this.conn = conn;
    this.category = new Category(new MySqlConnection(connectionString));
  }

  public List<TransactionModel> ListAllTransactions(
    string? initialDate = "",
    string? endDate = ""
  )
  {
    List<TransactionModel> transactions = new List<TransactionModel>();
    StringBuilder commandStr = new StringBuilder("SELECT ");
    commandStr.AppendLine(
      "Transaction.id," +
      "Transaction.description," +
      "Transaction.type," +
      "Transaction.value," +
      "Transaction.expiration_date," +
      "Transaction.category_id"
    );
    commandStr.AppendLine("FROM Transaction");
    commandStr.AppendLine("INNER JOIN Category");
    commandStr.AppendLine("ON Transaction.category_id = Category.id");

    bool haveFilter = !(initialDate == "") && !(endDate == "");
    if (haveFilter)
    {
      commandStr.AppendLine("WHERE Transaction.expiration_date");
      commandStr.AppendLine("BETWEEN @initial_date AND @end_date");
    }
    MySqlCommand command = new MySqlCommand(commandStr.ToString(), this.conn);
    this.conn.Open();

    if(haveFilter)
    {
      command.Parameters.AddWithValue(
        "@initial_date",
        Util.FormatSearchDate(initialDate!)
      );
      command.Parameters.AddWithValue(
        "@end_date",
        Util.FormatSearchDate(endDate!)
      );
    }

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
          expiration_date = Convert.ToDateTime(
            dataReader["expiration_date"].ToString()
          )
        };
        transactions.Add(transaction);
      }
    }

    this.conn.Close();
    return transactions;
  }

  public void SaveTransaction(TransactionModel transaction)
  {
    if (transaction.id == null) CreateTransaction(transaction);
    else EditTransaction(transaction);
  }

  private void CreateTransaction(TransactionModel transaction)
  {
    this.conn.Open();
    StringBuilder commandStr = new StringBuilder("");
    commandStr.AppendLine("INSERT INTO Transaction ");
    commandStr.AppendLine("(description, type, value, expiration_date, category_id) ");
    commandStr.AppendLine("VALUES (" +
      "@description," +
      "@type," +
      "@value," +
      "@expiration_date," +
      "@category_id" +
      ")");

    MySqlCommand command = new MySqlCommand(commandStr.ToString(), this.conn);
    command.Parameters.AddWithValue("@description", transaction.description);
    command.Parameters.AddWithValue("@type", transaction.type);
    command.Parameters.AddWithValue("@value", transaction.value);
    command.Parameters.AddWithValue("@expiration_date", transaction.expiration_date);
    command.Parameters.AddWithValue("@category_id", transaction.category.id);

    command.ExecuteNonQuery();
    this.conn.Close();
  }

  public void EditTransaction(TransactionModel transaction)
  {

  }

  public void DeleteTransaction(int transactionId)
  {
    this.conn.Open();
    StringBuilder commandStr = new StringBuilder("");
    commandStr.AppendLine("DELETE FROM Transaction ");
    commandStr.AppendLine("WHERE Transaction.id = @id");

    MySqlCommand command = new MySqlCommand(commandStr.ToString(), this.conn);
    command.Parameters.AddWithValue("@id", transactionId);

    command.ExecuteNonQuery();
    this.conn.Close();
  }
}
