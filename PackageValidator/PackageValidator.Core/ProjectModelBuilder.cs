using PackageValidator.Core.Exceptions;
using PackageValidator.Core.Extensions;
using PackageValidator.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PackageValidator.Core
{
  public class ProjectModelBuilder : IProjectModelBuilder
  {
    public Project Build(string fileName)
    {
      var lines = File.ReadAllLines(fileName);
      return this.Build(lines);
    }
    public Project Build(IEnumerable<string> lines)
    {
      if (lines.Count() == 0)
      {
        throw new FileFormatException(0, $"The file was empty");
      }
      var project = new Project();
      RecordType recordType = RecordType.PackageCount;
      int lineNumber = 1;

      foreach (string line in lines)
      {
        if (recordType == RecordType.PackageCount)
        {
          project.PackageCount = this.PackageCountRecord(line, lineNumber);
          recordType = RecordType.Package;
        }
        else if (recordType == RecordType.Package)
        {
          if (line.Contains(","))
          {
            project.Packages.Add(this.PackageRecord(line, lineNumber));
          }
          else
          {
            project.DependencyCount = this.DependencyCountRecord(line, lineNumber);
            recordType = RecordType.Dependency;
          }
        }
        else if(recordType == RecordType.Dependency)
        { 
          if (line.Contains(","))
          {
            project.Dependencies.Add(this.DependencyRecord(line, lineNumber));
          }
        }
        lineNumber++;
      }
      if (project.PackageCount != project.Packages.Count)
      {
        throw new ItemCountMismatchException(project.PackageCount, project.Packages.Count,
          $"{project.PackageCount} packages were expected but {project.Packages.Count()} were defined");
      }
      if (project.DependencyCount != project.Dependencies.Count)
      {
        throw new ItemCountMismatchException(project.DependencyCount, project.Dependencies.Count,
          $"{project.DependencyCount} dependencies were expected but {project.Dependencies.Count()} were defined");
      }
      return project;
    }
    private int PackageCountRecord(string line, int lineNumber)
    {
      int? packageCount = line.ToNullableInt();
      if (!packageCount.HasValue)
      {
        throw new FileFormatException(lineNumber, $"Invalid package count {line}");
      }
      return packageCount.Value;
    }
    private Package PackageRecord(string line, int lineNumber)
    {
      string[] parts = line.Split(',');
      if (parts.Count() != 2)  // required P,V
      {
        throw new FileFormatException(lineNumber, $"Invalid package [{line} line {lineNumber}]");
      }
      return new Package
      {
        Name = parts[0],
        Version = parts[1],
        LineNumber = lineNumber,
      };
    }
    private int DependencyCountRecord(string line, int lineNumber)
    {
      int? dependencyCount = line.ToNullableInt();
      if (!dependencyCount.HasValue)
      {
        throw new FileFormatException(lineNumber, $"Invalid dependency count {line}");
      }
      return dependencyCount.Value;
    }
    private Dependency DependencyRecord(string line, int lineNumber)
    {
      string[] parts = line.Split(',');
      if (parts.Count() != 4)  // required P,V,D,V
      {
        throw new FileFormatException(lineNumber, $"Invalid dependency [{line} line {lineNumber}]");
      }
      return new Dependency
      {
        Package = new Package { Name = parts[0], Version = parts[1] },
        DependentPackage = new Package { Name = parts[2], Version = parts[3], LineNumber = lineNumber },
      };
    }
  }
}
