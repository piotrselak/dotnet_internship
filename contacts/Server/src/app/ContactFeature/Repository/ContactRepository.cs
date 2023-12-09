using contacts.Server.Database;
using contacts.Shared;
using Microsoft.EntityFrameworkCore;

namespace contacts.Server.ContactFeature.Repository;

public class ContactRepository : IContactRepository
{
    private ContactsContext _context;
    private readonly ILogger<ContactRepository> _logger;

    public ContactRepository(ContactsContext context,
        ILogger<ContactRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Contact>> GetContacts()
    {
        return await _context.Contacts.ToListAsync();
    }

    public async Task<Contact?> GetContactById(int id)
    {
        return await _context.Contacts.Include(p => p.Category)
            .Include(p => p.SubCategory)
            .FirstOrDefaultAsync(contact =>
                contact.Id == id);
    }

    public async Task<Contact?> GetContactByEmail(string email)
    {
        return await _context.Contacts.Include(p => p.Category)
            .Include(p => p.SubCategory)
            .FirstOrDefaultAsync(contact =>
                contact.Email == email);
    }

    public void CreateContact(Contact contact)
    {
        _context.Contacts.Add(contact);
    }

    public void DeleteContact(Contact contact)
    {
        _context.Contacts.Remove(contact);
    }

    public void UpdateContact(Contact contact)
    {
        _context.Contacts.Update(contact);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
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