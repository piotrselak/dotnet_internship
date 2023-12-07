namespace contacts.Server.Result;

public enum ErrorType
{
    Validation = 400,
    NotFound = 404,
    AlreadyExists = 409,
    Other = 500
}