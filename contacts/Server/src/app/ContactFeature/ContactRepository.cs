using contacts.Server.Database;
using contacts.Shared;
using Microsoft.EntityFrameworkCore;

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
        return _context.Contacts.ToList();
    }

    public Contact? GetContactById(int id)
    {
        return _context.Contacts.Find(id);
    }

    public void CreateContact(Contact contact)
    {
        _context.Contacts.Add(contact);
    }

    public void DeleteContact(int id)
    {
        Contact contact = _context.Contacts.Find(id)!;
        _context.Contacts.Remove(contact);
    }

    public void UpdateContact(Contact contact)
    {
        _context.Entry(contact).State = EntityState.Modified;
    }

    public void Save()
    {
        _context.SaveChanges();
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