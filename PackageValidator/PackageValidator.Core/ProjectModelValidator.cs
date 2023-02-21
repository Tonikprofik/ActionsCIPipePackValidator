using PackageValidator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PackageValidator.Core
{
  public class ProjectModelValidator : IProjectModelValidator
  {
    private Project _project;
    public IProjectModelValidator With(Project project)
    {
      _project = project;
      return this;
    }
    public IEnumerable<ValidationError> Validate()
    {
      if(_project == null)
      {
        throw new Exception("Project is null, call With(Project) to set the project");
      }

      var dependentPackages = new List<Package>(_project.Packages);
      foreach(var package in _project.Packages)
      {
        this.GetDependentPackages(package, dependentPackages);
      }
      var validationErrors = this.ValidateVersions(dependentPackages);

      return validationErrors;
    }
    private void GetDependentPackages(Package target, List<Package> dependentPackages)
    {
      var packages = _project.Dependencies
        .Where(d => d.Package.Name == target.Name && d.Package.Version == target.Version)
        .Select(d => d.DependentPackage);

      foreach (var package in packages)
      {
        if (!dependentPackages.Any(t => t.Name == package.Name && t.Version == package.Version))
        {
          dependentPackages.Add(package);
          GetDependentPackages(package, dependentPackages);
        }
      }
    }
    private IEnumerable<ValidationError> ValidateVersions(IEnumerable<Package> dependentPackages)
    {
      var failedPackageGroups = dependentPackages
        .GroupBy(t => t.Name)
        .Where(g => g.Count() > 1)
        .Select(grp => grp);

      foreach (var packageGroup in failedPackageGroups)
      {
        string errorMessage = $"Package {packageGroup.Key} contains mismatched versions: ";
        foreach (var package in packageGroup)
        {
          errorMessage += $"[{package.Name} ({package.Version}) line {package.LineNumber}] ";
        }
        yield return new ValidationError
        {
          ErrorType = "VALIDATION_ERROR",
          Message = errorMessage,
        };
      }
    }
  }
}
