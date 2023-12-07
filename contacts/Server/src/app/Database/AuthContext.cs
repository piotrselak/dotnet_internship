using contacts.Server.AuthFeature;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace contacts.Server.Database;

public class AuthContext : IdentityDbContext<User>
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) {}
}