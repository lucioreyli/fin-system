using System;
using MySqlConnector;
using BetterConsoleTables;
using static System.Console;
using TransactionModel = Model.Transaction;
using CategoryModel = Model.Category;
using TransactionRepository = Persistence.Transaction;
using CategoryRepository = Persistence.Category;

namespace FinSystem;
class Program {
  private List<TransactionModel> transactions = new List<TransactionModel>();
  private List<CategoryModel> categories = new List<CategoryModel>();

  private TransactionRepository transaction;
  private CategoryRepository category;

  public Program()
  {
    string connectionString = Database.Connection.GetStringConnection();
    MySqlConnection conn = new MySqlConnection(connectionString);
    this.transaction = new TransactionRepository(conn);
    this.category = new CategoryRepository(conn);
  }

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
          self.transactions = self.transaction.ListAllTransactions();
          Table transactionsTable = new Table(
            "ID",
            "Description",
            "Type",
            "Value",
            "Expiration date"
          );
          foreach (TransactionModel transaction in self.transactions)
            transactionsTable.AddRow(
              transaction.id,
              transaction.description,
              transaction.type.Equals('R') ? "Receive" : "Payment",
              String.Format("{0:c}", transaction.value),
              String.Format("{0:dd/MM/yyyy}", transaction.expiration_date)
            );
          Write(transactionsTable.ToString());
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

          TransactionModel newTransaction = new TransactionModel
          {
            description = transactionDescription,
            value = transactionValue,
            type = transactionType,
            expiration_date = transactionExpirationDate,
            category = category,
          };


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
          self.transactions = self.transaction.ListAllTransactions();
          transactionsTable = new Table(
            "ID",
            "Description",
            "Type",
            "Value",
            "Expiration date"
          );
          foreach (TransactionModel transaction in self.transactions)
            transactionsTable.AddRow(
              transaction.id,
              transaction.description,
              transaction.type.Equals('R') ? "Receive" : "Payment",
              String.Format("{0:c}", transaction.value),
              String.Format("{0:dd/MM/yyyy}", transaction.expiration_date)
            );
          Write(transactionsTable.ToString());
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
          self.transactions = self.transaction.ListAllTransactions(
            initialDate,
            endDate
          );
          Table relatoryTable = new Table(
            "ID",
            "Description",
            "Type",
            "Value",
            "Expiration date"
          );
          foreach (TransactionModel transaction in self.transactions)
            relatoryTable.AddRow(
              transaction.id,
              transaction.description,
              transaction.type.Equals('R') ? "Receive" : "Payment",
              String.Format("{0:c}", transaction.value),
              String.Format("{0:dd/MM/yyyy}", transaction.expiration_date)
            );
          Write(relatoryTable.ToString());
          break;
      }
      ReadKey();
      Clear();
    } while (option != 6);
  }
}