using System;
using static System.Console;

namespace Utils;
public class Utils
{

  public static void MountMenu()
  {
    string programTitle = "FinSystem @ lucioroadtoglory";
    Title = programTitle;
    WriteLine(programTitle);
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

  public static string FormatSearchDate(string date)
  {
    string[] temp = date.Split('/');
    return string.Join('-', temp.Reverse());
  }
}
