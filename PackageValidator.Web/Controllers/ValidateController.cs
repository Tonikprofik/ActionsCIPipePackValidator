using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PackageValidator.Api.Services;
using System.Linq;

namespace PackageValidator.Api.Controllers
{
  public class ValidateController : Controller
  {
    private readonly IProjectValidatorService _projectValidatorService;
    public ValidateController(IWebHostEnvironment hostingEnvironment,
      IProjectValidatorService projectValidatorService)
    {
      _projectValidatorService = projectValidatorService;
    }
    [HttpPost]
    public IActionResult Validate()
    {
      if(Request.Form.Files.Count != 1)
      {
        return BadRequest("No file");
      }
      var file = Request.Form.Files[0];
      if (file.Length > 0)
      {
        var validationErrors = _projectValidatorService.Validate(file);
        if(validationErrors.Any())
        {
          return BadRequest(validationErrors);
        }
        return Ok("Success");
      }
      return BadRequest("The file was empty");
    }
  }
}