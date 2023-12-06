using contacts.Shared;

namespace contacts.Server.ContactFeature;

public class ContactService : IContactService
{
    public IEnumerable<BriefContact> GetContactList()
    {
        throw new NotImplementedException();
    }

    public Contact GetContactDetails(int id)
    {
        throw new NotImplementedException();
    }

    public bool RemoveContact(int id)
    {
        throw new NotImplementedException();
    }

    public bool AddContact(Contact contact)
    {
        throw new NotImplementedException();
    }

    public bool UpdateContact(Contact contact)
    {
        throw new NotImplementedException();
    }
}