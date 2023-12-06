using contacts.Server.Result;
using contacts.Shared;

namespace contacts.Server.ContactFeature;

// ContactService will be used as a business logic holder.
public interface IContactService
{
    // This cannot fail (as empty list is also a valid response)
    // so it returns unpacked value.
    IEnumerable<BriefContact> GetContactList();
    ServiceResult<Contact> GetContactDetails(int id);
    ServiceResult<bool> RemoveContact(int id);
    ServiceResult<bool> AddContact(Contact contact);
    ServiceResult<bool> UpdateContact(Contact contact);
}