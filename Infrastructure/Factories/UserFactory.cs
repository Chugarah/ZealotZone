using Domain;
using Infrastructure.Entities;

namespace Infrastructure.Factories;
/// <summary>
/// User Factory
/// </summary>
public class UserFactory : EntityFactoryBase<User, UserEntity>
{
    /// <summary>
    /// Create a UserEntity from a User Entity to
    /// a domain object
    /// Entity -> Domain
    /// </summary>
    /// <param name="usersEntity"></param>
    /// <returns></returns>
    public override User ToDomain(UserEntity usersEntity) => new()
    {
        FirstName = usersEntity.FirstName,
        LastName = usersEntity.LastName,
    };

    /// <summary>
    /// Creating from Domain object to Entity object
    /// Domain -> Entity
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    public override UserEntity ToEntity(User users) => new()
    {
        FirstName = users.FirstName,
        LastName = users.LastName,
    };
}