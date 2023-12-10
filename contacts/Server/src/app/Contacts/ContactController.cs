using contacts.Server.Contacts.Service;
using contacts.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace contacts.Server.Contacts;

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

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateContract(
        [FromBody] CreateContact contact,
        int id)
    {
        var validatedContact = new Contact
        {
            Id = id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            Password = contact.Password,
            PhoneNumber = contact.PhoneNumber,
            BirthDate = contact.BirthDate,
            CategoryId = contact.CategoryId,
            SubCategoryId = contact.SubCategoryId,
        };

        var response = await _contactService.UpdateContact(validatedContact,
            contact.SubCategoryName);

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        return Ok();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostContract(
        [FromBody] CreateContact contact)
    {
        var validatedContact = new Contact
        {
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            Password = contact.Password,
            PhoneNumber = contact.PhoneNumber,
            BirthDate = contact.BirthDate,
            CategoryId = contact.CategoryId,
            SubCategoryId = contact.SubCategoryId,
        };
        var response = await _contactService.AddContact(validatedContact,
            contact.SubCategoryName);

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        return Ok();
    }
}