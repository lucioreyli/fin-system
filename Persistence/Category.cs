using System.Text;
using MySqlConnector;
using CategoryModel = Model.Category;

namespace Persistence;
public class Category
{
  private readonly MySqlConnection conn;

  public Category(MySqlConnection conn) {
    this.conn = conn;
  }

  public CategoryModel GetCategory(int id)
  {
    CategoryModel category = new CategoryModel();

    StringBuilder commandStr = new StringBuilder("");
    commandStr.AppendLine("SELECT Category.id, Category.name FROM Category");
    commandStr.AppendLine("WHERE Category.id = @id");
    MySqlCommand command = new MySqlCommand(commandStr.ToString(), this.conn);
    command.Parameters.AddWithValue("@id", id);
    this.conn.Open();

    using (MySqlDataReader dataReader = command.ExecuteReader())
    {
      dataReader.Read();
      category.id = Convert.ToInt32(dataReader["id"].ToString());
      category.name = dataReader["name"].ToString()!;
    }

    this.conn.Close();
    return category;
  }


  public List<CategoryModel> ListAllTransactions()
  {
    List<CategoryModel> categories = new List<CategoryModel>();
    StringBuilder commandStr = new StringBuilder("");
    commandStr.AppendLine("SELECT Category.id, Category.name FROM Category");
    MySqlCommand command = new MySqlCommand(commandStr.ToString(), this.conn);
    this.conn.Open();

    using (MySqlDataReader dataReader = command.ExecuteReader())
    {
      while (dataReader.Read())
      {
        int category_id = Convert.ToInt32(dataReader["id"].ToString());
        CategoryModel category = new CategoryModel
        {
          id = Convert.ToInt32(dataReader["id"].ToString())!,
          name = dataReader["name"].ToString()!,
        };
        categories.Add(category);
      }
    }

    this.conn.Close();
    return categories;
  }
}
