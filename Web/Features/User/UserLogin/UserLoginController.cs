using Core.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZealotZone.Features.User.UserLogin.ViewModels;
using ZealotZone.Interfaces.Services;

namespace ZealotZone.Features.User.UserLogin;

[Area("User")]
public class UserLoginController(IUserService userService, IUserFactory userFactory) : Controller
{
    [HttpGet]
    [AllowAnonymous]
    [Route("/")]
    [Route("/user-login")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/user-login")]
    public async Task<IActionResult> LoginUser([FromForm] UserLoginViewModel userLoginViewModel)
    {
        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        // Using Password Directly from ViewModel instead of in the DTO
        // Mapping Viewmodel to Dto
        var userLoginDto = userFactory.CreateDtoFromViewModel(userLoginViewModel);
        var userLoginResult = await userService.LoginUserAsync(
            userLoginDto,
            userLoginViewModel.Password
        );

        // Check the result from the service
        return userLoginResult.StatusCode switch
        {
            200 => // OK - Login successful
            Ok(
                new
                {
                    Message = "Login successful.",
                    User = userLoginResult.Value,
                    RedirectUrl = Url.Action(
                        "Projects",
                        "ProjectsDisplay",
                        new { area = "Project" }
                    ),
                }
            ),
            401 => // Unauthorized
            Unauthorized(new { Message = "Invalid username or password." }),
            _ => StatusCode(
                userLoginResult.StatusCode,
                new { Message = "An unexpected error occurred during login." }
            ),
        };
    }
}
