using PackageValidator.Core;
using PackageValidator.Core.Models;
using System;
using System.IO;
using System.Linq;

namespace PackageValidator
{
  public class Program
  {
    static void Main(string[] args)
    {
      IProjectModelValidator _validator = new ProjectModelValidator();
      IProjectModelBuilder _builder = new ProjectModelBuilder();

      if (args.Length == 0)
      {
        Console.WriteLine("PackageValidator usage: PackageValidator <fileName>");
        return;
      }

      string fileName = args[0];
      Project project;

      try
      {
        project = _builder.Build(fileName);
      }
      catch(FileNotFoundException)
      {
        WriteLineWithColor($"FILE_NOT_FOUND: {fileName}", ConsoleColor.Red);
        return;
      }
      catch (Exception e)
      {
        WriteLineWithColor($"FILE_FORMAT_ERROR", ConsoleColor.Red);
        WriteLineWithColor(e.Message,ConsoleColor.Red);
        return;
      }
      // validate the model
      var errors = _validator
          .With(project)
          .Validate();
      if (errors.Count() > 0)
      {

        WriteLineWithColor("VALIDATION_ERROR",ConsoleColor.Red);
        foreach (var e in errors)
        {
          WriteLineWithColor(e.Message, ConsoleColor.Red);
        }
      }
      else // success
      {
        WriteLineWithColor(Properties.Resources.VaildFile, ConsoleColor.Green);
      }
    }

    private static void WriteLineWithColor(string text, ConsoleColor color)
    {
      var originalColor = Console.ForegroundColor;
      Console.ForegroundColor = color;
      Console.WriteLine(text);
      Console.ForegroundColor = originalColor;
    }
  }
}
