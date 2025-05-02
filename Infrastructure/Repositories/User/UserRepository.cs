using Core.Interfaces.Data;
using Core.Interfaces.Factories;
using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories.User;

public class UserRepository(
    DataContext dataContext,
    IEntityFactory<Domain.User?, UserEntity> factory,
    IRepositoryResultFactory resultFactory) :
    BaseRepository<Domain.User,
        UserEntity>(dataContext, factory, resultFactory),
    IUserRepository
{
    // If we want to override the methods from the BaseRepository
    // using override keyword, remember that the method needs to be virtual in the BaseRepository
}