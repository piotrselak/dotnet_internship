using Microsoft.EntityFrameworkCore;

namespace contacts.Shared;

[Index(nameof(Email), IsUnique = true)]
public class Contact
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly BirthDate { get; set; }
    public Category Category { get; set; }
    public SubCategory? SubCategory { get; set; }
}