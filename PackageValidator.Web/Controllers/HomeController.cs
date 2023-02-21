using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PackageValidator.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using PackageValidator.Api.Services;
using PackageValidator.Core.Models;

namespace PackageValidator.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly IProjectValidatorService _projectValidatorService;

    public HomeController( IWebHostEnvironment hostingEnvironment, IProjectValidatorService projectValidatorService)
    {
      _projectValidatorService = projectValidatorService;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpPost]
    [Route("/validation-results")]
    public IActionResult Validate()
    {
      string fileName = string.Empty;
      var validationErrors = new List<ValidationError>();
      if (Request.Form.Files.Count != 1)
      {
        validationErrors.Add( new ValidationError { ErrorType = "Missing file", Message = "Please select a file to upload" } );

      }
      else
      {
        var file = Request.Form.Files[0];
        fileName = file.FileName;
        if (file.Length > 0)
        {
          validationErrors = _projectValidatorService.Validate(file);
        }
        else
        {
          validationErrors.Add(new ValidationError { ErrorType = "Empty File", Message = "Please select a valid file to upload" });
        }
      }

      return View("ValidationResult", new ValidationResult { FileName = fileName, ValidationErrors = validationErrors } );

    }
  }
}
