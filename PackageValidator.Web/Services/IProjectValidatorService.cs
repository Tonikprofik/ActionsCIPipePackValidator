using Microsoft.AspNetCore.Http;
using PackageValidator.Core.Models;
using System.Collections.Generic;

namespace PackageValidator.Api.Services
{
  public interface IProjectValidatorService
  {
    List<ValidationError> Validate(IFormFile file);
  }
}
