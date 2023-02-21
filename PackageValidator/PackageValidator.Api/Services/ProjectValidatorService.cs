using Microsoft.AspNetCore.Http;
using PackageValidator.Core;
using PackageValidator.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace PackageValidator.Api.Services
{
  public class ProjectValidatorService : IProjectValidatorService
  {
    private readonly IProjectModelBuilder _builder;
    private readonly IProjectModelValidator _validator;

    public ProjectValidatorService(IProjectModelBuilder builder,IProjectModelValidator validator)
    {
      _builder = builder;
      _validator = validator;
    }
    public IEnumerable<ValidationError> Validate(IFormFile file)
    {
      Project project;
      try
      {
        project = _builder.Build(this.GetFileContents(file));
      }
      catch (Exception e)
      {
        var validationError = new ValidationError
        {
          ErrorType = "FILE_FORMAT_ERROR",
          Message = e.Message
        };
        return new List<ValidationError> { validationError };
      }

      // validate the model
      var errors = _validator
          .With(project)
          .Validate();
      return errors;
    }
    public IEnumerable<string> GetFileContents(IFormFile file)
    {
      string line;
      var fileLines = new List<string>();
      using (var reader = new StreamReader(file.OpenReadStream()))
      {
        while ((line = reader.ReadLine()) != null)
        {
          yield return line;
        }
      }
    }
  }
}
