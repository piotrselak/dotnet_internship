namespace contacts.Shared;

public class CreateContact
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly BirthDate { get; set; }
    public int CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    // used when creating new subcategory when other is chosen as the main one
    public string? SubCategoryName { get; set; }
}