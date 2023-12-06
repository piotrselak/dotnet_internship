namespace contacts.Server.Result;

// Class used to provide results of Service layer operations if operations may fail,
// as I want to separate HTTP results from service layer.
// If operations cannot fail (unless something critical happens) service should return
// unpacked values.
public class ServiceResult<TData>
{
    public bool Succeeded { get; set; }
    public TData Data { get; set; }
    public ErrorType Error { get; set; }
}