using contacts.Shared;
using Microsoft.EntityFrameworkCore;

namespace contacts.Server.Database;

public class ContactsContext : DbContext
{
    public ContactsContext(DbContextOptions<ContactsContext> options) : base(options) { }
    
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().HasIndex(c => c.Email).IsUnique();
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Business",
            }, new Category
            {
                Id = 2,
                Name = "Private",
            }, new Category
            {
                Id = 3,
                Name = "Other",
            });
        
        modelBuilder.Entity<SubCategory>().HasData(
            new SubCategory
            {
                Id = 1,
                Name = "Boss",
                CategoryId = 1
            },
            new SubCategory
            {
                Id = 2,
                Name = "Client",
                CategoryId = 1
            },
            new SubCategory
            {
                Id = 3,
                Name = "Coworker",
                CategoryId = 1
            },
            new SubCategory
            {
                Id = 4,
                Name = "Family",
                CategoryId = 3
            });

        modelBuilder.Entity<Contact>().HasData(
            new Contact
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Password = "dashAJS@12J",
                PhoneNumber = "1234567890",
                BirthDate = new DateOnly(1990, 1, 1),
                CategoryId = 1,
                SubCategoryId = 1,
            },
            new Contact
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                Password = "dskaKA!@23L",
                PhoneNumber = "9876543210",
                BirthDate = new DateOnly(1985, 5, 10),
                CategoryId = 2,
                SubCategoryId = null
            },
            new Contact
            {
                Id = 3,
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice@example.com",
                Password = "dsaj!@#jdsaAS",
                PhoneNumber = "5555555555",
                BirthDate = new DateOnly(1995, 8, 20),
                CategoryId = 1,
                SubCategoryId = 2,
            },
            new Contact
            {
                Id = 4,
                FirstName = "Bob",
                LastName = "Anderson",
                Email = "bob@example.com",
                Password = "dsJAJ@#j4A",
                PhoneNumber = "1111111111",
                BirthDate = new DateOnly(1988, 11, 15),
                CategoryId = 3,
                SubCategoryId = 4
            }
        );
    }
}