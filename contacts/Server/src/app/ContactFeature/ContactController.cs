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

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetContact(int id)
    {
        var response = await _contactService.GetContactDetails(id);

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        return Ok(response.Data);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var response = await _contactService.RemoveContact(id);

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        return Ok(response.Data);
    }
}