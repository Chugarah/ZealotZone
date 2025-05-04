using Core.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZealotZone.Features.User.UserRegistration.ViewModels;
using ZealotZone.Interfaces.Services;

namespace ZealotZone.Features.User.UserRegistration;

[Area("User")]
public class UserRegistrationController(IUserFactory userFactory, IUserService userService)
    : Controller
{
    [HttpGet]
    [AllowAnonymous]
    [Route("/user-registration")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/user-registration")]
    public async Task<IActionResult> AddMember(
        [FromForm] UserRegistrationViewModel userRegistration
    )
    {
        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        // Breaking Full name into FirstName and LastName
        // Used string so we don't need to specify value.
        string firstName;
        string lastName;
        var fullName = userRegistration
            .FullName.Trim()
            .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        // Simple Validation, it needs to be full Name No Firstname or Lastname only!
        if (fullName.Length == 2)
        {
            firstName = fullName[0];
            lastName = fullName[1];
        }
        else
        {
            // Sending Custom Error Message (This is from the server)
            return ValidationProblem(
                nameof(userRegistration.FullName),
                "Full name must be in the format 'FirstName LastName'."
            );
        }

        // Mapping Viewmodel to Dto
        var userInsertDto = userFactory.CreateDtoFromViewModel(userRegistration);
        // Setting FirstName and LastName
        userInsertDto.FirstName = firstName;
        userInsertDto.LastName = lastName;

        // Using Password Directly from ViewModel instead of in the DTO
        var userRegistrationResult = await userService.CreateUserAsync(
            userInsertDto,
            userRegistration.Password
        );

        // Check if the service operation failed
        if (userRegistrationResult.StatusCode is 200 or 201)
            return Ok(
                new
                {
                    Message = "User registered successfully.",
                    User = userRegistrationResult.Value,
                    RedirectUrl = Url.Action("Index", "UserLogin", new { area = "User" }),
                }
            );

        // Check if the service operation failed
        ModelState.AddModelError(
            !string.IsNullOrEmpty(userRegistrationResult.Error.FieldName)
                ? userRegistrationResult.Error.FieldName
                // Add as a general model error if no specific field
                : string.Empty,
            userRegistrationResult.Error.Message
        );
        return ValidationProblem(ModelState);
    }
}
