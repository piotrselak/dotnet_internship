using contacts.Server.Database;
using contacts.Shared;

namespace contacts.Server.ContactFeature;

public class ContactRepository : IContactRepository
{
    private ContactsContext _context;

    public ContactRepository(ContactsContext context)
    {
        _context = context;
    }

    public IEnumerable<Contact> GetContacts()
    {
        throw new NotImplementedException();
    }

    public Contact GetContactById(int id)
    {
        throw new NotImplementedException();
    }

    public void CreateContact(Contact contact)
    {
        throw new NotImplementedException();
    }

    public void DeleteContact(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateContact(Contact contact)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) _context.Dispose();
        _disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}