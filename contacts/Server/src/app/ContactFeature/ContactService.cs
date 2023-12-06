using contacts.Server.Result;
using contacts.Shared;

namespace contacts.Server.ContactFeature;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;

    public ContactService(IContactRepository repository)
    {
        _repository = repository;
    }
    
    public IEnumerable<BriefContact> GetContactList()
    {
        return _repository
            .GetContacts()
            .Select(BriefContact.FromContact);
    }

    public ServiceResult<Contact> GetContactDetails(int id)
    {
        throw new NotImplementedException();
    }

    public ServiceResult<bool> RemoveContact(int id)
    {
        throw new NotImplementedException();
    }

    public ServiceResult<bool> AddContact(Contact contact)
    {
        throw new NotImplementedException();
    }

    public ServiceResult<bool> UpdateContact(Contact contact)
    {
        throw new NotImplementedException();
    }
}