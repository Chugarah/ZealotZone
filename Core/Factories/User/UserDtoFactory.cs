using Core.Dto.User;
using Core.Interfaces.DTos;

namespace Core.Factories.User;

public class UserDtoFactory : IUserDtoFactory
{
    // Creating from Domain object to Create a DTO object, Inserting all fields
    public Domain.User.User ToDomain(UserInsertDto insertDto) =>
        new() { Email = insertDto.Email, FirstName = insertDto.FirstName, LastName = insertDto.LastName };

    // Creating from Domain object to Create a DTO object Displaying only Email
    public UserDisplay ToDisplay(Domain.User.User user) => new()
    {
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName
    };

    public Domain.User.User ToDomain(UserLoginDto userLoginDto)
    {
        return new Domain.User.User { Email = userLoginDto.Email };
    }

    // Creating from Domain object to Create a DTO object, Login only
    public Domain.User.User ToDisplayLogin(UserDisplay userDisplay) => new()
    {
        Email = userDisplay.Email
    };
}