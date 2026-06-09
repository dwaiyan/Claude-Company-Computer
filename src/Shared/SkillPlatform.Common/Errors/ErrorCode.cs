namespace SkillPlatform.Common.Errors;

public static class ErrorCode
{
    public const string InvalidCredentials = "AUTH_INVALID_CREDENTIALS";
    public const string UserNotFound = "AUTH_USER_NOT_FOUND";
    public const string EmailAlreadyExists = "AUTH_EMAIL_EXISTS";
    public const string TokenExpired = "AUTH_TOKEN_EXPIRED";
    public const string InvalidToken = "AUTH_INVALID_TOKEN";
    public const string Unauthorized = "AUTH_UNAUTHORIZED";
    public const string Forbidden = "AUTH_FORBIDDEN";
    public const string ValidationFailed = "VALIDATION_FAILED";
    public const string InternalError = "INTERNAL_ERROR";
    public const string ResourceNotFound = "RESOURCE_NOT_FOUND";
}
