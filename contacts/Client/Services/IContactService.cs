using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Client.Services;

public interface IContactService
{
    Task<IEnumerable<BriefContact>> GetContacts();
    Task<Result<Contact>> GetContactById(int id);
    Task<Result<Empty>> DeleteContact(int id);
    Task<Result<Empty>> CreateNewContact(CreateContact contact);
    Task<Result<Empty>> UpdateContract(CreateContact contact, int id);
}