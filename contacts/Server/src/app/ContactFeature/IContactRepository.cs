using contacts.Shared;

namespace contacts.Server.ContactFeature;

// To implement database operations, I used the repository pattern.
// It abstracts data to collection-like being.
// Repository extends Disposable interface as we want to dispose the database connection later.
public interface IContactRepository : IDisposable
{
    IEnumerable<Contact> GetContacts();
    Contact? GetContactById(int id);
    void CreateContact(Contact contact);
    void DeleteContact(int id);
    void UpdateContact(Contact contact);
    void Save();
}