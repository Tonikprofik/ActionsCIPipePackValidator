namespace PackageValidator.Core.Extensions
{
  public static class StringExtensions
  {
    public static int? ToNullableInt(this string value)
    {
      if(int.TryParse(value, out int intValue))
      {
        return intValue;
      }
      return null;
    }
  }
}
