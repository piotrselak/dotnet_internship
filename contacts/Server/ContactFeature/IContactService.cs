using contacts.Shared;

namespace contacts.Server.ContactFeature;

// ContactService will be used as a business logic holder.
public interface IContactService
{
    IEnumerable<BriefContact> GetContactList();
    Contact GetContactDetails(int id);
    bool RemoveContact(int id);
    bool AddContact(Contact contact);
    bool UpdateContact(Contact contact);
}