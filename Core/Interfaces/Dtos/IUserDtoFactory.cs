using Core.Dto.User;

namespace Core.Interfaces.DTos;

public interface IUserDtoFactory
{
    Domain.User.User ToDomain(UserInsertDto insertDto);
    Domain.User.User ToDomain(UserLoginDto userLoginDto);
    UserDisplay ToDisplay(Domain.User.User user);


}