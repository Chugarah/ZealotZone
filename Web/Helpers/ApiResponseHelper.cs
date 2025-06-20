using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Helpers;

public abstract class ApiResponseHelper : IResult
{
    /// <summary>
    /// [Phind-generated] Returns successful response with data payload
    /// </summary>
    /// <param name="data">Domain object/dto to return</param>
    /// <typeparam name="T">Response type</typeparam>
    /// <returns>200 OK with data</returns>
    public static IResult Success<T>(T data) => Results.Ok(data);

    /// <summary>
    /// [Phind-generated] Returns resource creation response
    /// </summary>
    /// <param name="data">Created resource data</param>
    /// <typeparam name="T">Resource type</typeparam>
    /// <returns>201 Created with empty location header</returns>
    public static IResult Created<T>(T data) => Results.Created(string.Empty, data);

    /// <summary>
    /// [Phind-generated] Returns resource creation response with location header
    /// </summary>
    /// <param name="routeName">Named route for resource location</param>
    /// <param name="routeValues">Route parameters</param>
    /// <param name="data">Created resource data</param>
    /// <typeparam name="T">Resource type</typeparam>
    /// <returns>201 Created with Location header</returns>
    public static IResult CreatedAtRoute<T>(string routeName, object routeValues, T data) =>
        Results.CreatedAtRoute(routeName, routeValues, data);

    /// <summary>
    /// [Phind-generated] Returns RFC 7807 validation error response
    /// </summary>
    /// <param name="error"></param>
    /// <returns>400 Bad Request with error details</returns>
    public static IResult BadRequest(string error) =>
        Results.BadRequest(CreateProblemDetails("Bad Request", error, 400));

    /// <summary>
    /// [Phind-generated] Returns 404 Haven't Found response
    /// </summary>
    /// <param name="resource">Missing resource name/identifier</param>
    /// <returns>404 Not Found with error details</returns>
    public static IResult NotFound(string resource) =>
        Results.NotFound(CreateProblemDetails("Not Found", $"{resource} not found", 404));

    /// <summary>
    /// [Phind-generated] Returns 409 Conflict response
    /// </summary>
    /// <param name="resource">Conflict description</param>
    /// <returns>409 Conflict with error details</returns>
    public static IResult ConflictDuplicate(string resource) =>
        Results.Conflict(CreateProblemDetails("Conflict", $"{resource} already exists", 409));

    /// <summary>
    /// [Phind-generated] Returns 400 Validation Error response
    /// </summary>
    /// <param name="errors">Dictionary of field errors (key: field name, value: error messages)</param>
    /// <returns>400 Bad Request with validation errors</returns>
    public static IResult ValidationProblem(IDictionary<string, string[]> errors) =>
        Results.ValidationProblem(errors, title: "Validation Error");

    /// <summary>
    /// This Summary Generated by Phind AI
    /// Returns a 400 Bad Request response with structured error details
    /// </summary>
    /// <param name="error">Human-readable error title</param>
    /// <param name="detail">Technical details about the specific error occurrence</param>
    /// <returns>RFC 7807-compliant problem details response with 400 status code</returns>
    /// <remarks>
    /// Use this for client errors where both user-facing message and developer
    /// debugging details are needed. Matches Problem Details RFC 7807 specification.
    /// </remarks>
    public static IResult BadRequestWithMessage(string error, string detail) =>
        Results.BadRequest(CreateProblemDetails(error, detail, 400));

    /// <summary>
    /// [Phind-generated] Handles uncaught exceptions with optional debug details
    /// </summary>
    /// <param name="ex">Exception instance</param>
    /// <param name="includeDetails">Show stack trace in development</param>
    /// <returns>500 Internal Server Error response</returns>
    /// <remarks>
    /// Phind AI implementation based on Microsoft's ProblemDetails spec
    /// </remarks>
    public static IResult Problem(Exception ex, bool includeDetails = false) =>
        Results.Problem(
            title: "Server Error",
            detail: includeDetails ? ex.ToString() : "An unexpected error occurred",
            statusCode: 500
        );

    private static ProblemDetails CreateProblemDetails(
        string title,
        string detail,
        int statusCode,
        object? data = null
    )
    {
        // Create a new ProblemDetails object
        var problem = new ProblemDetails
        {
            Type = $"https://httpstatuses.io/{statusCode}",
            Title = title,
            Detail = detail,
            Status = statusCode,
        };

        // Add optional data to the problem response
        if (data != null)
        {
            problem.Extensions["data"] = data;
        }

        // Return the problem response
        return problem;
    }

    /// <summary>
    /// Returns a 400 Bad Request response with structured error details
    /// https://stackoverflow.blog/2020/03/02/best-practices-for-rest-api-design/
    /// </summary>
    /// <param name="detail"></param>
    /// <param name="statusCode"></param>
    /// <param name="errorData"></param>
    /// <returns></returns>
    public static IResult Problem(
        string detail,
        HttpStatusCode statusCode,
        object? errorData = null
    )
    {
        return Results.Problem(
            CreateProblemDetails(
                title: statusCode.ToString(),
                detail: detail,
                statusCode: (int)statusCode,
                data: errorData
            )
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task ExecuteAsync(HttpContext httpContext)
    {
        throw new NotImplementedException();
    }
}
