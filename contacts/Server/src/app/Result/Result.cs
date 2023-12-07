namespace contacts.Server.Result;

// Class used to provide results of Service layer operations if operations may fail,
// as I want to separate HTTP results from service layer. I also don't want to use
// exceptions for the flow of the http errors.
// If operations cannot fail (unless something critical happens) service should return
// unpacked values.
public class Result<TData>
{
    public bool Succeeded { get; init; }
    public TData? Data { get; init; }
    public Error? Error { get; init; }
}

// This class is used when there is no Data but operation succeeded.
// It models domain better than Object type.
public class Empty
{
}