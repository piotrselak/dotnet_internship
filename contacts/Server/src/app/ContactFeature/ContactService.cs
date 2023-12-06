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
    
    public async Task<IEnumerable<BriefContact>> GetContactList()
    {
        var contacts = await _repository.GetContacts();
        return contacts.Select(BriefContact.FromContact);
    }

    public async Task<ServiceResult<Contact>> GetContactDetails(int id)
    {
        var contact = await _repository.GetContactById(id);

        if (contact == null)
            return new ServiceResult<Contact>
            {
                Succeeded = false,
                Error = ErrorType.NotFound
            };

        return new ServiceResult<Contact>
        {
            Succeeded = true,
            Data = contact
        };
    }

    public async Task<ServiceResult<Empty>> RemoveContact(int id)
    {
        var contact = await _repository.GetContactById(id);
        
        if (contact == null)
            return new ServiceResult<Empty>
            {
                Succeeded = false,
                Error = ErrorType.NotFound
            };
        
        _repository.DeleteContact(contact);
        await _repository.SaveAsync();
        
        return new ServiceResult<Empty>()
        {
            Succeeded = true,
            Data = new Empty()
        };
    }
 
    public async Task<ServiceResult<Empty>> AddContact(Contact contact)
    {
        var exists = await _repository.GetContactByEmail(contact.Email) != null;
        if (exists)
            return new ServiceResult<Empty>
            {
                Succeeded = false,
                Error = ErrorType.AlreadyExists
            };
        
        _repository.CreateContact(contact);
        await _repository.SaveAsync();
        
        return new ServiceResult<Empty>()
        {
            Succeeded = true,
            Data = new Empty()
        };
    }

    public async Task<ServiceResult<Empty>> UpdateContact(Contact contact)
    {
        var existingContact = await _repository.GetContactById(contact.Id);
        
        if (existingContact == null)
            return new ServiceResult<Empty>
            {
                Succeeded = false,
                Error = ErrorType.NotFound
            };
        
        _repository.UpdateContact(contact);
        await _repository.SaveAsync();
        
        return new ServiceResult<Empty>()
        {
            Succeeded = true,
            Data = new Empty()
        };
    }
}