namespace contacts.Server.Result;

// Class used to provide results of Service layer operations if operations may fail,
// as I want to separate HTTP results from service layer. I also don't want to use
// exceptions for the flow of the http errors.
// If operations cannot fail (unless something critical happens) service should return
// unpacked values.
public class ServiceResult<TData>
{
    public bool Succeeded { get; set; }
    public TData? Data { get; set; }
    public ErrorType? Error { get; set; }
}

// This class is used when there is no Data but operation succeeded.
// It models domain better than Object type.
public class Empty
{
}