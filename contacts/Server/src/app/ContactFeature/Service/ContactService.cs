using contacts.Server.ContactFeature.Repository;
using contacts.Server.Result;
using contacts.Shared;

namespace contacts.Server.ContactFeature.Service;

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

    public async Task<Result<Contact>> GetContactDetails(int id)
    {
        var contact = await _repository.GetContactById(id);

        if (contact == null)
            return new Result<Contact>
            {
                Succeeded = false,
                Error = new Error(404, "Contact not found")
            };

        return new Result<Contact>
        {
            Succeeded = true,
            Data = contact
        };
    }

    public async Task<Result<Empty>> RemoveContact(int id)
    {
        var contact = await _repository.GetContactById(id);
        
        if (contact == null)
            return new Result<Empty>
            {
                Succeeded = false,
                Error = new Error(404, "Contact not found")
            };
        
        _repository.DeleteContact(contact);
        await _repository.SaveAsync();
        
        return new Result<Empty>()
        {
            Succeeded = true,
            Data = new Empty()
        };
    }
 
    public async Task<Result<Empty>> AddContact(Contact contact)
    {
        var exists = await _repository.GetContactByEmail(contact.Email) != null;
        if (exists)
            return new Result<Empty>
            {
                Succeeded = false,
                Error = new Error(409,
                    "Contact with given email already exists")
            };
        
        _repository.CreateContact(contact);
        await _repository.SaveAsync();
        
        return new Result<Empty>()
        {
            Succeeded = true,
            Data = new Empty()
        };
    }

    public async Task<Result<Empty>> UpdateContact(Contact contact)
    {
        var existingContact = await _repository.GetContactById(contact.Id);
        
        if (existingContact == null)
            return new Result<Empty>
            {
                Succeeded = false,
                Error = new Error(404, "Contact not found")
            };
        
        _repository.UpdateContact(contact);
        await _repository.SaveAsync();
        
        return new Result<Empty>()
        {
            Succeeded = true,
            Data = new Empty()
        };
    }
}