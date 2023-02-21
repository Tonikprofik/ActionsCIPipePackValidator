using System.Collections.ObjectModel;

namespace PackageValidator.Core.Models
{
  public class Project
  {
    public int PackageCount { get; set; }
    public Collection<Package> Packages { get; set; }
    public int DependencyCount { get; set; }
    public Collection<Dependency> Dependencies { get; set; }

    public Project()
    {
      this.Packages = new Collection<Package>();
      this.Dependencies = new Collection<Dependency>();
    }
  }
}
