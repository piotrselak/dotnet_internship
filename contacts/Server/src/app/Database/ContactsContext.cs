using contacts.Shared;
using Microsoft.EntityFrameworkCore;

namespace contacts.Server.Database;

public class ContactsContext : DbContext
{
    public ContactsContext(DbContextOptions<ContactsContext> options) : base(options) { }
    
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }

}