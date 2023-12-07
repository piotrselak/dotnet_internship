using contacts.Server.ContactFeature.Service;
using Microsoft.AspNetCore.Mvc;
using contacts.Shared;
using Microsoft.AspNetCore.Authorization;

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
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllContacts()
    {
        var fullList = await _contactService.GetContactList();
        return Ok(fullList);
    }

    // TODO debug only
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetContact(int id)
    {
        return Ok("Works only for logged");
    }
}