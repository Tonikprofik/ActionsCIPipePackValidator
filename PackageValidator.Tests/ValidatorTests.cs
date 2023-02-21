using PackageValidator.Core;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PackageValidator.Tests
{
  public class ValidatorTests
  {
    private IProjectModelValidator _validator;
    private IProjectModelBuilder _builder;

    public ValidatorTests()
    {
      _validator = new ProjectModelValidator();
      _builder = new ProjectModelBuilder();
    }

    [Fact]
    public void Valid_Project_Returns_No_Errors()
    {
      var project = _builder.Build(new List<string> {
        "2",
        "A,1",
        "B,1",
        "3",
        "A,1,B,1",
        "A,2,B,2",
        "C,1,B,1",
      });

      var errors = _validator
          .With(project)
          .Validate();

      Assert.Empty(errors);
    }

    [Fact]
    public void Invalid_Flat_Project_Returns_Error()
    {
      var project = _builder.Build(new List<string> {
        "2",
        "A,1",
        "B,1",
        "3",
        "A,1,B,2",
        "A,1,B,1",
        "B,1,B,2"
      });
      var errors = _validator
          .With(project)
          .Validate();

      Assert.Single(errors);
    }

    [Fact]
    public void Invalid_Nested_Dependency_Returns_Error()
    {
      var project = _builder.Build(new List<string> {
        "2",
        "A,1",
        "B,1",
        "4",
        "A,1,C,2",
        "C,2,D,3",
        "B,2,B,1",
        "D,3,A,5",
      });
      var errors = _validator
          .With(project)
          .Validate();

      Assert.Single(errors);
    }
    [Fact]
    public void Invalid_Nested_Dependencies_Return_Multiple_Errors()
    {
      //Package A contains mismatched versions: [A(1) line 2][A(5) line 8]
      //Package B contains mismatched versions: [B(1) line 3][B(6) line 11]
      //Package D contains mismatched versions: [D(3) line 6][D(2) line 10]

      var project = _builder.Build(new List<string> {
        "2",
        "A,1",
        "B,1",
        "7",
        "A,1,C,2",
        "C,2,D,3",
        "B,2,B,1",
        "B,1,A,5",
        "A,5,E,5",
        "E,5,D,2",
        "D,2,B,6"
      });
      var errors = _validator
          .With(project)
          .Validate();

      Assert.Equal(3, errors.Count());
    }
  }
}
