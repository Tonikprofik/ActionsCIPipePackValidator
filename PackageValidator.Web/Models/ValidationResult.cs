using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PackageValidator.Core.Models;

namespace PackageValidator.Web.Models
{
  public class ValidationResult
  {
    public IEnumerable<ValidationError> ValidationErrors { get; set; }
    public string FileName { get; set; }
    public bool IsValid { get; set; }
  }
}
