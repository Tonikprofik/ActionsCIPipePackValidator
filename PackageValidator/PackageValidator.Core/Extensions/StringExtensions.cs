namespace PackageValidator.Core.Extensions
{
  public static class StringExtensions
  {
    public static int? ToNullableInt(this string value)
    {
      int intValue;
      if(int.TryParse(value, out intValue))
      {
        return intValue;
      }
      return null;
    }
  }
}
