using Microsoft.AspNetCore.Mvc;
using contacts.Shared;

namespace contacts.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    public IEnumerable<Contact> GetAllContacts()
    {
        return null;
    }
}