using System;

namespace PackageValidator.Core.Exceptions
{
  public class ItemCountMismatchException : Exception
  {
    public int Count { get; set; }
    public int Actual { get; set; }
    public ItemCountMismatchException()
    {
    }
    public ItemCountMismatchException(int count, int actual, string message)
        : base(message)
    {
      this.Count = count;
      this.Actual = actual;
    }

    public ItemCountMismatchException(string message)
        : base(message)
    {
    }

    public ItemCountMismatchException(string message, Exception inner)
        : base(message, inner)
    {
    }
  }
}
