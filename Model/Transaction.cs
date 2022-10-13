using System;

namespace Model;
public class Transaction
{
  public int? id { get; private set;  }
  public string description { get; private set; }
  public double value { get; private set; }
  public char type { get; private set; }
  public DateTime expiration_date { get; private set; }
  public Category category { get; private set; }

  public Transaction(
    string description,
    double value,
    char type,
    DateTime expiration_date,
    Category category
  )
  {
    this.description = description;
    this.type = type;
    this.value = value;
    this.expiration_date = expiration_date;
    this.category = category;
  }

  public Transaction(
    int id,
    string description,
    double value,
    char type,
    DateTime expiration_date,
    Category category
  )
  {
    this.description = description;
    this.type = type;
    this.value = value;
    this.expiration_date = expiration_date;
    this.category = category;
    this.id = id;
  }
}
