namespace PackageValidator.Core.Models
{
  public class Dependency
  {
    public Package Package { get; set; }
    public Package DependentPackage { get; set; }
  }
}
