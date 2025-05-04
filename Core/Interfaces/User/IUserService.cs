using Core.Dto.User;
using Domain;

namespace Core.Interfaces.User;

public interface IUserService
{
    Task<RepositoryResult<UserDisplay>> CreateUserAsync(
        UserInsertDto? userInsertDto,
        string userPassword
    );
    Task<RepositoryResult<bool>> LoginUserAsync(UserLoginDto userLoginDto, string userPassword);
    Task<RepositoryResult<bool>> LogoutUserAsync();

    Task<RepositoryResult<UserDisplay>> GetUserAsync(UserLoginDto userLoginDto);



}
