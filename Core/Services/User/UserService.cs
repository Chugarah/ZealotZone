using Core.Dto.User;
using Core.Interfaces.Data;
using Core.Interfaces.DTos;
using Core.Interfaces.Factories;
using Core.Interfaces.User;
using Domain;

namespace Core.Services.User;

public class UserService(
    IUnitOfWork unitOfWork,
    IUserDtoFactory userDtoFactory,
    IRepositoryResultFactory resultFactory,
    IUserPersistenceService userPersistenceService
) : IUserService
{
    /// <summary>
    /// Asynchronously creates a new user based on the provided DTO and password.
    /// </summary>
    /// <param name="userInsertDto">The data transfer object containing the details for the new user.
    ///  Can be null.</param>
    /// <param name="userPassword">The password for the new user.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="RepositoryResult{UserDisplay}"/>
    /// which includes the created user's display details (<see cref="UserDisplay"/>)
    /// and status code 201 (Created) on success,
    /// or an error code and status code indicating the failure reason if
    /// the user could not be created (e.g., duplicate username/email).
    /// </returns>
    /// <exception cref="Exception">Throws an exception if an unexpected error occurs during the user creation process.</exception>
    public async Task<RepositoryResult<UserDisplay>> CreateUserAsync(
        UserInsertDto? userInsertDto,
        string userPassword
    )
    {
        try
        {
            // 1. Convert the DTO to a domain object
            var userInsert = userDtoFactory.ToDomain(userInsertDto!);

            var persistenceResult = await userPersistenceService.CreateUserPersistenceAsync(
                userInsert,
                userPassword
            );

            // 2. Check if there is an error
            if (persistenceResult.Error != Error.NonError)
            {
                // Return the error using RepositoryResult
                return new RepositoryResult<UserDisplay>(
                    persistenceResult.Error,
                    persistenceResult.StatusCode
                );
            }
            // Create the display DTO using the factory
            var displayUserDto = userDtoFactory.ToDisplay(userInsert); // Use ToDisplay method

            // Return success response with code 201 from the Repository result factory (Thanks Hans)
            return resultFactory.OperationSuccess(displayUserDto, 201);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the user.", ex);
        }
    }

    public async Task<RepositoryResult<bool>> LoginUserAsync(
        UserLoginDto userLoginDto,
        string userPassword
    )
    {
        try
        {
            // 1. Convert the DTO to a domain object
            var userLogin = userDtoFactory.ToDomain(userLoginDto);

            // 2. Call the persistence service which now handles sign-in
            var loginResult = await userPersistenceService.LoginUserPersistenceAsync(
                userLogin,
                userPassword
            );

            // 3. Return the result directly from the persistence layer
            return loginResult;
        }
        // Catch potential DTO mapping errors or other service-level issues
        catch (Exception)
        {
            // Log the exception ex
            // Use a more specific error code if possible
            return resultFactory.OperationFailed<bool>(
                new Error(
                    "User.LoginError",
                    "An unexpected error occurred during the login process."
                ),
                500
            );
        }
    }

    /// <summary>
    /// Logs the current user out.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="RepositoryResult{Boolean}"/>
    /// indicating the outcome of the logout operation.
    /// </returns>
    public async Task<RepositoryResult<bool>> LogoutUserAsync()
    {
        try
        {
            // Access logout service
            var logoutResult = await userPersistenceService.LogoutUserPersistenceAsync();

            // Check if the logout was successful
            return logoutResult;
        }
        // Catch potential unexpected errors during the service call
        catch (Exception)
        {
            // Log the exception ex (recommended)
            return resultFactory.OperationFailed<bool>(
                new Error(
                    "User.LogoutError",
                    "An unexpected error occurred during the logout process."
                ),
                500
            );
        }
    }

    public async Task<RepositoryResult<UserDisplay>> GetUserAsync(UserLoginDto userLoginDto)
    {
        try
        { // Convert the DTO to a domain object
            var userLogin = userDtoFactory.ToDomain(userLoginDto);

            // Call the persistence service to get the user
            var getUserPersistenceResult = await userPersistenceService.GetUserAsync(userLogin);

            // Check if the result is successful
            if (
                getUserPersistenceResult.StatusCode != 200
                || getUserPersistenceResult.Value == null
            )
            {
                // Return the error using RepositoryResult
                return resultFactory.OperationFailed<UserDisplay>(
                    getUserPersistenceResult.Error,
                    getUserPersistenceResult.StatusCode
                );
            }

            var displayUserDto = userDtoFactory.ToDisplay(userLogin);
            return resultFactory.OperationSuccess(displayUserDto, 200);
        }
        catch (Exception)
        {
            // Log the exception ex (recommended)
            return resultFactory.OperationFailed<UserDisplay>(
                new Error(
                    "User.RetrievalError",
                    "An unexpected error occurred while retrieving user details."
                ),
                500 // Internal Server Error
            );
        }
    }
}
