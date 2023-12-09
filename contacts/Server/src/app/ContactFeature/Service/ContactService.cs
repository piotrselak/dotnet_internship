using contacts.Server.ContactFeature.Repository;
using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Server.ContactFeature.Service;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;
    private readonly ILogger<ContactService> _logger;

    public ContactService(IContactRepository repository,
        ILogger<ContactService> logger)
    {
        _repository = repository;
        _logger = logger;
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
        _logger.LogInformation(contact.Category.Name);
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