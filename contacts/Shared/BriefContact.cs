namespace contacts.Shared;

public class BriefContact
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    // Helpful when mapping Contacts from db to brief versions
    public static BriefContact FromContact(Contact contact) 
        => new BriefContact
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email
            };
}