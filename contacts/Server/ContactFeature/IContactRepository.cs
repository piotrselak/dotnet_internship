using contacts.Shared;

namespace contacts.Server.ContactFeature;

public interface IContactRepository : IDisposable
{
    IEnumerable<Contact> GetContacts();
    Contact GetContactById(int id);
    void CreateContact(Contact contact);
    void DeleteContact(int id);
    void UpdateContact(Contact contact);
    void Save();
}