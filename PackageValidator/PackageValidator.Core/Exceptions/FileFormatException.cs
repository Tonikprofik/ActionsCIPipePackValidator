using System;

namespace PackageValidator.Core.Exceptions
{
  public class FileFormatException : Exception
  {
    public int LineNumber { get; set; }
    public FileFormatException()
    {
    }

    public FileFormatException(int lineNumber, string message)
        : base(message)
    {
      this.LineNumber = lineNumber; 
    }
    public FileFormatException(string message)
        : base(message)
    {
    }

    public FileFormatException(string message, Exception inner)
        : base(message, inner)
    {
    }
  }
}
