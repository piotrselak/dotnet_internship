using Microsoft.AspNetCore.Mvc;
using contacts.Shared;

namespace contacts.Server.ContactFeature;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    public IActionResult GetAllContacts()
    {
        return null;
    }
}