using Core.Dto.User;
using Core.Interfaces.Factories;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces.Data;

public interface IUserPersistenceService
{
    /// <summary>
    /// Creates a new user asynchronously by persisting the provided user data into the system.
    /// </summary>
    /// <returns>
    /// A <c>Task</c> representing the asynchronous operation. The result of the task is a <c>RepositoryResult&lt;bool&gt;</c>,
    /// where:
    /// - A value of <c>true</c> indicates the user was successfully created.
    /// - A value of <c>false</c> indicates the user creation failed due to some issue.
    /// - In case of an error, includes detailed error information describing the failure.
    /// </returns>
    Task<RepositoryResult<bool>> CreateUserPersistenceAsync(Domain.User.User user , string userPassword);

    Task<RepositoryResult<bool>> LoginUserPersistenceAsync(Domain.User.User user, string password);
    Task<RepositoryResult<bool>> LogoutUserPersistenceAsync();

    /// <summary>
    /// Retrieves a user asynchronously from the system based on the provided email address.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>
    /// A <c>Task</c> representing the asynchronous operation.
    /// The result of the task is an instance of <c>User</c> if a matching user is found.
    /// If no user is found, the result will be <c>null</c>.
    /// </returns>
    Task<RepositoryResult<UserDisplay>> GetUserAsync(Domain.User.User user);
}