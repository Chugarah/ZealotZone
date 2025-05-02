using Core.Dto.User;
using Core.Interfaces.DTos;
using Core.Interfaces.User;

namespace Core.Services.User;

public class UserService : IUserService
{
    public Task<IUserDtoFactory> CreateUserAsync(UserInsertDto userInsertDto)
    {
        throw new NotImplementedException();
    }
}