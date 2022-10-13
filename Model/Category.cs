namespace Model;
public class Category
{
  public int id { get; }
  public string name { get; }

  public Category(int id, string name)
  {
    this.id = id;
    this.name = name;
  }
}
