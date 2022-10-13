using System;
using MySqlConnector;
using BetterConsoleTables;
using Database;
using static System.Console;
using TransactionModel = Model.Transaction;
using CategoryModel = Model.Category;
using TransactionRepository = Persistence.Transaction;
using CategoryRepository = Persistence.Category;

namespace FinSystem;
class Program {
  private List<TransactionModel> transactions = new List<TransactionModel>();
  private List<CategoryModel> categories = new List<CategoryModel>();

  static string connectionString = Database.Connection.GetStringConnection();
  static MySqlConnection conn = new MySqlConnection(connectionString);
  private TransactionRepository transaction = new TransactionRepository(conn);
  private CategoryRepository category = new CategoryRepository(conn);

  static void Main() {
    Clear();
    Program self = new Program();
      
    int option;
    do
    {
      Utils.Utils.MountMenu();
      option = Convert.ToInt32(ReadLine());
      Clear();
      if(option < 1 || option > 6)
      {
        BackgroundColor = ConsoleColor.Red;
        ForegroundColor = ConsoleColor.White;
        Utils.Utils.MountHeader("Invalid option");
        ResetColor();
        continue;
      }

      switch(option)
      {
        case 1:
          Utils.Utils.MountHeader("List transactions");
          ListTransactions(self);
          break;

        case 2:
          Utils.Utils.MountHeader("Create transaction");
          string? transactionDescription = "";
          do
          {
            Write("Transaction description: ");
            transactionDescription = ReadLine();
          } while (transactionDescription == "" || transactionDescription == null);
          Write("Report transaction value: ");
          double transactionValue = Convert.ToDouble(ReadLine());
          WriteLine("Report transaction type, available options: P or R");
          WriteLine("P - Payment");
          WriteLine("R - Receive");
          char transactionType = Convert.ToChar(ReadLine()!);
          Write("Wha is the expiration date? (DD/MM/YYYY):  ");
          DateTime transactionExpirationDate = DateTime.Parse(ReadLine()!);

          WriteLine("Selecione uma categoria pelo ID:");
          self.categories = self.category.ListAllTransactions();
          Table categoriesTable = new Table("ID", "name");
          foreach (CategoryModel categoryItem in self.categories)
            categoriesTable.AddRow(categoryItem.id, categoryItem.name);
          Write(categoriesTable.ToString());
          int categoryId = Convert.ToInt32(ReadLine());
          CategoryModel category = self.category.GetCategory(categoryId);
          TransactionModel newTransaction = new TransactionModel(
            transactionDescription,
            transactionValue,
            transactionType,
            transactionExpirationDate,
            category
          );

          self.transaction.SaveTransaction(newTransaction);
          ForegroundColor = ConsoleColor.Green;
          Utils.Utils.MountHeader("TRANSACTION CREATED", '+');
          ResetColor();
          break;
        case 3:
          Utils.Utils.MountHeader("Edit transaction");
          break;

        case 4:
          Utils.Utils.MountHeader("Delete transaction");
          ListTransactions(self);
          Write("Select a transaction by ID to delete: ");
          int transactionToDelete = Convert.ToInt32(ReadLine());
          self.transaction.DeleteTransaction(transactionToDelete);
          ForegroundColor = ConsoleColor.Red;
          Utils.Utils.MountHeader("TRANSACTION DELETED", '+');
          ResetColor();
          break;

        case 5:
          Utils.Utils.MountHeader("Relatory");
          Utils.Utils.MountHeader("Search by date (dd/mm/yyyy)");
          Write("Initial date (dd/mm/yyyy): ");
          string? initialDate = ReadLine();
          Write("End date (dd/mm/yyyy): ");
          string? endDate = ReadLine();
          ListTransactions(self, initialDate ?? "", endDate ?? "");
          break;
      }
      ReadKey();
      Clear();
    } while (option != 6);
  }

  static void ListTransactions(
    Program ctx, string initialDate = "", string endDate = ""
  )
  {
    Table table = new Table("ID", "Description", "Type", "Value","Expiration date");
    var transactions = ctx.transaction.ListAllTransactions(initialDate, endDate);
    foreach (TransactionModel transaction in transactions)
      table.AddRow(
        transaction.id,
        transaction.description,
        transaction.type.Equals('R') ? "Receive" : "Payment",
        String.Format("{0:c}", transaction.value),
        String.Format("{0:dd/MM/yyyy}", transaction.expiration_date)
      );
    Write(table.ToString());
  }
}