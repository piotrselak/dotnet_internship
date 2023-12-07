using contacts.Shared;

namespace contacts.Client.Services;

public interface IContactService
{
    Task<IEnumerable<BriefContact>> GetContacts();
}