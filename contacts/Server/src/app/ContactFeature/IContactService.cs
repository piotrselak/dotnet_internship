using contacts.Server.Result;
using contacts.Shared;

namespace contacts.Server.ContactFeature;

// ContactService will be used as a business logic holder.
public interface IContactService
{
    // It cannot fail so there is no point to wrap it in ServiceResult
    Task<IEnumerable<BriefContact>> GetContactList();
    Task<ServiceResult<Contact>> GetContactDetails(int id);
    Task<ServiceResult<Empty>> RemoveContact(int id);
    Task<ServiceResult<Empty>> AddContact(Contact contact);
    Task<ServiceResult<Empty>> UpdateContact(Contact contact);
}