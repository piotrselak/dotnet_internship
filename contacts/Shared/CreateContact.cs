using System.ComponentModel.DataAnnotations;

namespace contacts.Shared;

public class CreateContact
{
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string Email { get; set; }

    [StringLength(100,
        ErrorMessage = "The {0} must be at least {2} characters long.",
        MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage =
            "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
    [Required]
    public string Password { get; set; }

    [Required] public string PhoneNumber { get; set; }
    [Required] public DateOnly BirthDate { get; set; }
    [Required] public int CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    // used when creating new subcategory when other is chosen as the main one
    public string? SubCategoryName { get; set; }
}