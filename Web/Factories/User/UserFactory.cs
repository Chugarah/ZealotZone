using Core.Dto.User;
using ZealotZone.Features.Project.ProjectsDisplay.ViewModels;
using ZealotZone.Features.User.UserLogin.ViewModels;
using ZealotZone.Features.User.UserRegistration.ViewModels;
using ZealotZone.Interfaces;
using ZealotZone.Interfaces.Services;

namespace ZealotZone.Factories.User;

/// <summary>
/// A factory class responsible for creating instances of the data transfer object (DTO)
/// <see cref="UserInsertDto"/> from instances of the view model
/// <see cref="UserRegistrationViewModel"/>.
/// This is typically used to streamline the conversion of data between layers in the
/// application, ensuring separation of concerns and reusability.
/// </summary>
public class UserFactory : IUserFactory
{
    public UserInsertDto CreateDtoFromViewModel(UserRegistrationViewModel
        userRegistrationViewModel)
    {
        return new UserInsertDto
        {
            Email = userRegistrationViewModel.Email,
        };
    }

    public UserLoginDto CreateDtoFromViewModel(UserLoginViewModel userLoginViewModel)
    {
        return new UserLoginDto()
        {
            Email = userLoginViewModel.Email
        };
    }

    public UserDisplay CreateViewModelFromDto(UserDataViewModel userDisplay)
    {
        return new UserDisplay()
        {
            Email = userDisplay.userEmail,
            FirstName = userDisplay.userFirstName,
            LastName = userDisplay.userLastName
        };
    }
}