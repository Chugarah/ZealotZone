using Core.Dto.User;

namespace Core.Interfaces.DTos;

public interface IUserDtoFactory
{
    Domain.User? ToDomain(UserInsertDto insertDto);
}