using PackageValidator.Core.Models;
using System.Collections.Generic;

namespace PackageValidator.Core
{
  public interface IProjectModelValidator
  {
    IProjectModelValidator With(Project project);
    IEnumerable<ValidationError> Validate();
  }
}
