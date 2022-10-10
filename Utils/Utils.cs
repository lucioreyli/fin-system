using System;
using static System.Console;

namespace Utils;
public class Utils
{
  public static void MountMenu()
  {
    WriteLine("FinSystem @ lucioroadtoglory");
    WriteLine("----------------------------");
    WriteLine("Select a option:");
    WriteLine("1 - List");
    WriteLine("2 - Create");
    WriteLine("3 - Edit");
    WriteLine("4 - Delete");
    WriteLine("5 - Relatory");
    WriteLine("6 - Exit");
    Write("Option: ");
  }

  public static void MountHeader(string title, char code = '-', int len = 30)
  {
    WriteLine($"{new string(code, len)} {title} {new string(code, len)}");
  }
}
