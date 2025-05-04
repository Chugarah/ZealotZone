using Core.Dto.User;
using Core.Interfaces.Data;
using Core.Interfaces.Factories;
using Domain;
using Domain.User;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserPersistenceService(
    UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager,
    RoleManager<IdentityRole> roleManager,
    IEntityFactory<User, UserEntity>? userEntityFactory,
    IRepositoryResultFactory resultFactory
) : IUserPersistenceService
{
    public async Task<RepositoryResult<bool>> CreateUserPersistenceAsync(
        User? user,
        string userPassword
    )
    {
        // ==== 1. Input Validation Start ===
        // Validating user domain
        if (user is null)
        {
            // Example using NullValue
            return resultFactory.OperationFailed<bool>(Error.NullValue, 400);
        }

        // Validate UserEntityFactory that is not null
        if (userEntityFactory == null)
        {
            return resultFactory.OperationFailed<bool>(
                new Error("Error.Configuration", "Internal server configuration error.", "null"),
                500
            );
        }
        // ==== Input Validation End ===

        // ==== 2. Object Mapping ===
        UserEntity userEntity;
        try
        {
            // Make sure your factory handles potential nulls if necessary
            userEntity = userEntityFactory.ToEntity(user!)!;
            // This is the Magic :) Mapping from Entity to Microsoft Identity
            userEntity.Email = user.Email;
            userEntity.UserName = user.Email;
            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;
        }
        catch (Exception ex)
        {
            return resultFactory.OperationFailed<bool>(
                new Error("Error.Mapping", "Internal error converting user data:" + ex.Message),
                500
            );
        }
        // ==== 2. Object Mapping End ===

        // ==== 3. Microsoft Identify UserManager - Create User ===
        // Result from a Microsoft Identity framework
        IdentityResult identityResult;
        try
        {
            // We need to ensure the user with that email does not exist before creating it! (Not him or she)
            var existingUser = await GetUserAsync(user);

            switch (existingUser.StatusCode)
            {
                // Duplication found by status 200, simulating IdentityError
                case 200:
                {
                    // Thanks, Google Gemini Pro for helping me to reuse/Simulate IdentityError (reusing mapping logic)
                    var identityError = new IdentityError
                    {
                        Code = nameof(IdentityErrorDescriber.DuplicateEmail),
                        Description = $"Email '{user.Email}' already exists.",
                    };

                    // Using the helper to map IdentityError to Domain.Error
                    var errorToDomainError = MapIdentityErrorToDomainError(identityError);
                    return resultFactory.OperationFailed<bool>(errorToDomainError, 400);
                }
                // We need to have a backup in case we get a network error or something with fetching did not work.
                // Rider helped me with converting into Switch and grouping if the return is the same :)
                case 500:
                    return resultFactory.OperationFailed<bool>(
                        existingUser.Error,
                        existingUser.StatusCode
                    );
                default:
                    identityResult = await userManager.CreateAsync(userEntity, userPassword!);
                    break;
            }
        }
        // Catch potential exceptions during DB interaction
        catch (Exception ex)
        {
            return resultFactory.OperationFailed<bool>(
                new Error(
                    "Persistence.Failure",
                    "An unexpected error occurred saving the user. " + ex.Message
                ),
                500
            );
        }
        // ==== 3. Microsoft Identify UserManager - Create User End===

        // ==== 4. Microsoft Identify UserManager - Assigning Role ===
        if (identityResult.Succeeded)
        {
            // --- Creating Role ---
            const string defaultRoleName = "Preator";

            // Let's check if the role exists
            var roleExists = await roleManager.RoleExistsAsync(defaultRoleName);
            if (!roleExists)
            {
                // If the role doesn't exist, create it
                var createRole = new IdentityRole(defaultRoleName);
                var roleResult = await roleManager.CreateAsync(createRole);

                // Check if the role creation was successful
                if (!roleResult.Succeeded)
                {
                    // Failed to create the role, return an error
                    return resultFactory.OperationFailed<bool>(
                        new Error(
                            "Persistence.RoleFailure",
                            "Failed to create the required user role. "
                                + roleResult.Errors.FirstOrDefault()?.Description
                        ),
                        500
                    );
                }
            }
            // --- Creating Role END---

            // --- Attaching User to Default Role ---
            var addUserToRoleResult = await userManager.AddToRoleAsync(userEntity, defaultRoleName);
            if (!addUserToRoleResult.Succeeded)
            {
                // Failed to add the user to the role, return an error
                return resultFactory.OperationFailed<bool>(
                    new Error(
                        "Persistence.RoleAssignmentFailure",
                        "Failed to assign the user to the required role. "
                            + addUserToRoleResult.Errors.FirstOrDefault()?.Description
                    ),
                    500
                );
            }
            // --- Attaching User to Default Role END ---
            // ==== 4. Microsoft Identify UserManager - Assigning Role ===
        }

        // ==== 5. Map IdentityResult to RepositoryResult ===
        // Rider Refactored this part :D
        if (identityResult.Succeeded)
            return resultFactory.OperationSuccess(true, 201); // 201 Created is appropriate

        // Map the first IdentityError to Domain.Error including FieldName
        var firstError = identityResult.Errors.FirstOrDefault();
        var domainError = MapIdentityErrorToDomainError(firstError);

        // Typically map Identity failures to a 400 Bad Request
        return resultFactory.OperationFailed<bool>(domainError, 400);

        // Return success using your factory
        // ==== 5. Map IdentityResult to RepositoryResult End ===
    }

    /// <summary>
    /// Attempts to authenticate a user based on their email and password using ASP.NET Core Identity UserManager.
    /// This method validates the input, checks the user's credentials, and returns a structured result indicating success or failure.
    /// Got inspired by Hans and got assistance from Google Gemini Pro AI.
    /// </summary>
    /// <param name="user">The user object containing the email to authenticate. Can be null.</param>
    /// <param name="password">The password provided by the user for verification.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="RepositoryResult{T}"/> of type <see cref="bool"/>:
    /// <list type="bullet">
    /// <item><description>If authentication is successful (user found and password matches), returns a successful result (<see cref="RepositoryResult{T}.IsSuccess"/> is true) with a value of <c>true</c> and HTTP status code 200 (OK).</description></item>
    /// <item><description>If the input is invalid (e.g., null user or email), returns a failed result with error code "User.InvalidInput" and HTTP status code 400 (Bad Request).</description></item>
    /// <item><description>If the user is not found by email or the password check fails, returns a failed result with error code "User.InvalidCredentials" and HTTP status code 401 (Unauthorized).</description></item>
    /// <item><description>If an internal configuration error occurs (e.g., missing factory), returns a failed result with error code "Error.Configuration" and HTTP status code 500 (Internal Server Error).</description></item>
    /// <item><description>If an unexpected exception occurs during database interaction or password checking, returns a failed result with error code "Persistence.Failure" and HTTP status code 500 (Internal Server Error).</description></item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// This method performs input validation, interacts with the <c>UserManager</c> to find the user and check the password, handles potential exceptions, and returns a structured result indicating the outcome of the login attempt.
    /// </remarks>
    public async Task<RepositoryResult<bool>> LoginUserPersistenceAsync(User? user, string password)
    {
        // ==== 1. Input Validation Start ===
        if (user?.Email is null)
        {
            // Use resultFactory for invalid input
            return resultFactory.OperationFailed<bool>(
                new Error("User.InvalidInput", "Email and password are required."),
                400 // Bad Request
            );
        }
        // ==== Input Validation End ===

        // ==== 2. Find User By Email using UserManager ===
        UserEntity? userEntity;
        try
        {
            // ==== 2. Find User By Email using UserManager ===
            userEntity = await userManager.FindByEmailAsync(user.Email);

            // ==== 3. Check Credentials & Attempt Sign-In ===
            if (userEntity == null)
                return resultFactory.OperationFailed<bool>(
                    new Error("User.InvalidCredentials", "Login failed, invalid credentials."),
                    401 // Unauthorized
                );
            // Check if the password is correct
            // Attempt to sign in the user with the provided password
            // Note: The PasswordSignInAsync method will also check if the user is locked out
            var signInResult = await signInManager.PasswordSignInAsync(
                userEntity,
                password,
                // TODO: Is Persistence not implemented yet (Remember Password)?
                isPersistent: false,
                lockoutOnFailure: true
            );

            // ==== 4. Process SignInResult ===
            // Check if the sign-in was successful
            if (signInResult.Succeeded)
            {
                // Sign-in successful
                return resultFactory.OperationSuccess(true, 200); // OK
            }

            // Check if the user is locked out and other failure reasons
            return resultFactory.OperationFailed<bool>(
                signInResult.IsLockedOut
                    ? new Error("User.LockedOut", "Account locked out.")
                    : new Error("User.InvalidCredentials", "Login failed, invalid credentials."),
                401
            );
        }
        // Catch potential exceptions during DB interaction
        catch (Exception ex)
        {
            // Log the exception (ex) is recommended
            return resultFactory.OperationFailed<bool>(
                new Error(
                    "Persistence.Failure",
                    "An unexpected error occurred during login: " + ex.Message
                ),
                500
            );
        }
    }

    /// <summary>
    /// Signs the current user out using ASP.NET Core Identity SignInManager.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="RepositoryResult{Boolean}"/>:
    /// - Success (true, 200 OK) if sign-out is successful.
    /// - Failure (false, 500 Internal Server Error) if an unexpected error occurs during sign-out.
    /// </returns>
    public async Task<RepositoryResult<bool>> LogoutUserPersistenceAsync()
    {
        try
        {
            // ==== 1. Attempt Sign Out using SignInManager ===
            await signInManager.SignOutAsync();
            // ==== 1. Attempt Sign Out using SignInManager End ===
            return resultFactory.OperationSuccess(true, 200);
        }
        catch (Exception ex)
        {
            // ==== 3. Handle Exceptions ===
            // Sending a generic error message to avoid exposing sensitive information
            return resultFactory.OperationFailed<bool>(
                new Error(
                    "Persistence.LogoutFailure",
                    // Avoid exposing ex.Message directly in production environments
                    "An unexpected error occurred during logout."
                ),
                500
            );
            // ==== 3. Handle Exceptions End ===
        }
    }

    public async Task<RepositoryResult<UserDisplay>> GetUserAsync(User user)
    {
        // ==== 1. Input Validation Start ===
        // Check email is not null
        if (user?.Email is null)
        {
            // Use resultFactory for invalid input
            return resultFactory.OperationFailed<UserDisplay>(Error.NullValue, 400);
        }

        // Check if the factory is available
        if (userEntityFactory == null)
        {
            // Use resultFactory for configuration errors
            return resultFactory.OperationFailed<UserDisplay>(
                new Error(
                    "Error.Configuration",
                    "Internal server configuration error: UserEntityFactory is null."
                ),
                500
            );
        }
        // ==== 1. Input Validation End ===

        // ==== 2. Using User manager to get user by Email ===
        try
        {
            var userEntity = await userManager.FindByEmailAsync(user.Email);

            // If a user manager can't find a user, it sends a response using resultFactory.
            if (userEntity is null)
            {
                // User isn't found, use resultFactory to indicate this
                return resultFactory.OperationFailed<UserDisplay>(
                    new Error("User.NotFound", $"User with email '{user.Email}' not found."),
                    404 // 404 Not Found
                );
            }

            // Map the UserEntity back to the domain User object
            var domainUser = userEntityFactory.ToDomain(userEntity);
            var userDisplayDto = new UserDisplay
            {
                Email = domainUser!.Email,
                FirstName = domainUser.FirstName,
                LastName = domainUser.LastName,
            };

            // Return the mapped DTO
            return resultFactory.OperationSuccess(userDisplayDto, 200);
        }
        // In case a network error or something unexpected happens
        catch (Exception ex)
        {
            return resultFactory.OperationFailed<UserDisplay>(
                new Error(
                    "Persistence.Failure",
                    "An unexpected error occurred while retrieving the user: " + ex.Message
                ),
                500
            );
        }
    }

    /// <summary>
    /// This helper was 100% Generated by Google Pro Gemini AI
    /// Maps an IdentityError to a Domain.Error object, optionally associating the error with a specific field or context.
    /// This method translates a standard IdentityError (e.g., from ASP.NET Identity) into an application-level error structure
    /// with additional context to make it more meaningful for domain or client use.
    /// </summary>
    /// <param name="identityError">
    /// The IdentityError object to be mapped. Represents an error that occurs during operations in Identity,
    /// such as invalid input, password requirements, or duplicate entries.
    /// If null, a default error is returned indicating an unknown identity issue.
    /// </param>
    /// <returns>
    /// Returns a Domain.Error object that encapsulates the error code, message, and optionally the field name
    /// relating to the error. This structure can be consumed by higher layers to provide user-friendly messages
    /// or to process validation failures on specific fields in the domain or DTO level.
    /// </returns>
    private static Error MapIdentityErrorToDomainError(IdentityError? identityError)
    {
        if (identityError == null)
        {
            // Default error if no specific IdentityError is provided
            return new Error("Identity.Unknown", "An unknown identity error occurred.");
        }

        var errorCode = identityError.Code;
        var message = identityError.Description;

        // Mapping to Viewmodel or DTO
        var fieldName = identityError.Code switch
        {
            // Email/ Username-related errors
            nameof(IdentityErrorDescriber.DuplicateUserName)
            or nameof(IdentityErrorDescriber.DuplicateEmail)
            or nameof(IdentityErrorDescriber.InvalidEmail)
            or nameof(IdentityErrorDescriber.InvalidUserName) =>
            // Use the property name from UserRegistrationViewModel or UserInsertDto
            nameof(UserInsertDto.Email),
            _ => throw new ArgumentOutOfRangeException(),
        };

        // Create the Domain.Error with the mapped FieldName
        return new Error(errorCode, message, fieldName);
    }
}
