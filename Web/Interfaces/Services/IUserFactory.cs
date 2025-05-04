using Core.Dto.User;
using Domain.User;
using ZealotZone.Features.Project.ProjectsDisplay.ViewModels;
using ZealotZone.Features.User.UserLogin.ViewModels;
using ZealotZone.Features.User.UserRegistration.ViewModels;

namespace ZealotZone.Interfaces.Services;

public interface IUserFactory
{
    UserInsertDto CreateDtoFromViewModel(UserRegistrationViewModel userRegistrationViewModel);
    UserLoginDto CreateDtoFromViewModel(UserLoginViewModel userLoginViewModel);

    UserDisplay CreateViewModelFromDto(UserDataViewModel userDataViewModel);
}