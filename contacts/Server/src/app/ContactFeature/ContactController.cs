using Microsoft.AspNetCore.Mvc;
using contacts.Shared;

namespace contacts.Server.ContactFeature;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    public async Task<IActionResult> GetAllContacts()
    {
        var fullList = await _contactService.GetContactList();
        return Ok(fullList);
    }
    
}