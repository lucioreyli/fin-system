using System;

namespace Model;
public class Transaction
{
  public int? id { get; set; }
  public string? description { get; set; }
  private char _type;
  public char type { 
    get => this._type; 
    set {
      this._type = value.Equals('P') || value.Equals('R') ? 
        value 
        :
        throw new Exception(
          $"{value} is a invalid option! Available options: P - Payment | R - Receipt"
        );
    }
  }
  public double value { get; set; }
  public DateTime expiration_date { get; set; }
  public Category category { get; set; }
}
