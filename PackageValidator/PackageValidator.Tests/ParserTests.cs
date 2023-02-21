using PackageValidator.Core;
using PackageValidator.Core.Exceptions;
using System;
using System.Collections.Generic;
using Xunit;

namespace PackageValidator.Tests
{
  public class ParserTests
  {
    IProjectModelValidator _validator = new ProjectModelValidator();
    IProjectModelBuilder _builder = new ProjectModelBuilder();
    private IEnumerable<string> _validProject = new List<string> {
      "2",
      "A,1",
      "B,1",
      "3",
      "A,1,B,1",
      "A,2,B,2",
      "C,1,B,1",
    };

    public ParserTests()
    {
    }

    [Fact]
    public void Valid_Number_Of_Packages_Match_Counts()
    {
      var project = _builder.Build(_validProject);
      Assert.Equal(project.PackageCount, project.Packages.Count);
    }

    [Fact]
    public void Invalid_Number_Of_Packages_Throws_FileFormatException()
    {
      var ex = Assert.Throws<ItemCountMismatchException>(() => _builder.Build(new List<string> {
        "5",
        "A,2",
        "B,2",
        "5",
        "A,1,B,1",
        "A,1,B,2",
        "A,2,C,3",
        "C,3,D,4",
        "D,4,B,1",
      }));
      Assert.Equal(2, ex.Actual);
      Assert.Equal(5, ex.Count);
    }

    [Fact]
    public void Valid_Number_Of_Dependencies_Match_Counts()
    {
      var project = _builder.Build(_validProject);
      Assert.Equal(project.DependencyCount, project.Dependencies.Count);
    }

    [Fact]
    public void Invalid_Number_Of_Dependencies_Throws_FileFormatException()
    {
      var ex = Assert.Throws<ItemCountMismatchException>(() => _builder.Build(new List<string> {
        "1",
        "A,2",
        "2",
        "A,1,B,1",
        "A,1,B,2",
        "A,2,C,3",
        "C,3,D,4",
        "D,4,B,1",
      }));
      Assert.Equal(5, ex.Actual);
      Assert.Equal(2, ex.Count);
    }

    [Fact]
    public void Invalid_Package_Count_Throws_FileFormatException()
    {
      var ex = Assert.Throws<FileFormatException>(() => _builder.Build(new List<string> {
        "1e",
        "A,2",
      }));
      Assert.Equal(1, ex.LineNumber);
    }

    [Fact]
    public void Empty_File_Throws_FileFormatException()
    {
      var ex = Assert.Throws<FileFormatException>(() => _builder.Build(new List<string> {}));
      Assert.Equal(0, ex.LineNumber);
    }

  }
}
