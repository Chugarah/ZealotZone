using Core.Dto.User;
using Core.Factories.User;
using Core.Interfaces.DTos;

namespace Core.Interfaces.User;

public interface IUserService
{
    Task<IUserDtoFactory> CreateUserAsync(UserInsertDto userInsertDto);
}