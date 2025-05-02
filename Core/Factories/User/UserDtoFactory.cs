using Core.Dto.User;
using Core.Interfaces.DTos;

namespace Core.Factories.User;

public class UserDtoFactory : IUserDtoFactory
{
    // Creating from Domain object to Create a DTO object
    public Domain.User? ToDomain(UserInsertDto insertDto) =>
        new() { FirstName = insertDto.FirstName, LastName = insertDto.LastName };
}