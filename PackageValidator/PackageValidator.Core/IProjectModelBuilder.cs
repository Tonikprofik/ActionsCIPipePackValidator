using PackageValidator.Core.Models;
using System.Collections.Generic;

namespace PackageValidator.Core
{
  public interface IProjectModelBuilder
  {
    Project Build(IEnumerable<string> lines);
    Project Build(string fileName);
  }
}
