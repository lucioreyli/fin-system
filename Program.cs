using System;
using MySqlConnector;
using BetterConsoleTables;
using static System.Console;
using TransactionModel = Model.Transaction;
using CategoryModel = Model.Category;
using TransactionRepository = Persistence.Transaction;
using CategoryRepository = Persistence.Category;

namespace FinSystem {
  class Program {
    private List<TransactionModel> transactions;
    private List<CategoryModel> categories;

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
      Program self = new Program();

      Clear();
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
            self.transactions = self.transaction.listAllTransactions();
            Table table = new Table("Id", "Description", "Type", "Value");
            foreach(TransactionModel transaction in self.transactions)
            {
              table.AddRow(
                transaction.id,
                transaction.description,
                transaction.type.Equals('R') ? "Receive" : "Payment",
                String.Format("{0:c}",transaction.value)
              );
            }
            Console.Write(table.ToString());
            break;
          case 2:
            Utils.Utils.MountHeader("Create transaction");
            break;
          case 3:
            Utils.Utils.MountHeader("Edit transaction");
            break;
          case 4:
            Utils.Utils.MountHeader("Delete transaction");
            break;
          case 5:
            Utils.Utils.MountHeader("Relatory");
            break;
        }
      } while (option != 6);
    }
  }
}
