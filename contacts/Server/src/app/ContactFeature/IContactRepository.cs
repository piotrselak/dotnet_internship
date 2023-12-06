using contacts.Shared;

namespace contacts.Server.ContactFeature;

// To implement database operations, I used the repository pattern.
// It abstracts data to collection-like being.
// Repository extends Disposable interface as we want to dispose the database connection later.
public interface IContactRepository : IDisposable
{
    Task<IEnumerable<Contact>> GetContacts();
    Task<Contact?> GetContactById(int id);
    void CreateContact(Contact contact);
    void DeleteContact(Contact contact);
    void UpdateContact(Contact contact);
    Task SaveAsync();
}