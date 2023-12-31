﻿using contacts.Server.Categories.Service;
using contacts.Server.Contacts.Repository;
using contacts.Shared;
using contacts.Shared.Result;
using Microsoft.EntityFrameworkCore;

namespace contacts.Server.Contacts.Service;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;
    private readonly ILogger<ContactService> _logger;
    private readonly ICategoryService _categoryService;

    public ContactService(IContactRepository repository,
        ILogger<ContactService> logger, ICategoryService categoryService)
    {
        _repository = repository;
        _logger = logger;
        _categoryService = categoryService;
    }

    public async Task<IEnumerable<BriefContact>> GetContactList()
    {
        var contacts = await _repository.GetContacts();
        return contacts.Select(BriefContact.FromContact);
    }

    public async Task<Result<Contact>> GetContactDetails(int id)
    {
        var contact = await _repository.GetContactById(id);

        if (contact == null)
            return new Result<Contact>
            {
                Succeeded = false,
                Error = new Error(404, "Contact not found")
            };

        return new Result<Contact>
        {
            Succeeded = true,
            Data = contact
        };
    }

    public async Task<Result<Empty>> RemoveContact(int id)
    {
        var contact = await _repository.GetContactById(id);

        if (contact == null)
            return new Result<Empty>
            {
                Succeeded = false,
                Error = new Error(404, "Contact not found")
            };

        _repository.DeleteContact(contact);
        await _repository.SaveAsync();

        return new Result<Empty>
        {
            Succeeded = true,
            Data = new Empty()
        };
    }

    public async Task<Result<Empty>> AddContact(Contact contact,
        string? subCategoryName)
    {
        var exists = await _repository.GetContactByEmail(contact.Email) != null;
        if (exists)
            return new Result<Empty>
            {
                Succeeded = false,
                Error = new Error(409,
                    "Contact with given email already exists")
            };

        var categoryResult =
            await _categoryService.HandleCategoriesFromContact(
                contact.CategoryId, contact.SubCategoryId, subCategoryName);

        if (!categoryResult.Succeeded)
            return new Result<Empty>
            {
                Succeeded = false,
                Error = categoryResult.Error
            };
        contact.CategoryId = categoryResult.Data!.CategoryId;
        contact.SubCategoryId = categoryResult.Data!.SubCategoryId;

        _repository.CreateContact(contact);
        await _repository.SaveAsync();

        return new Result<Empty>()
        {
            Succeeded = true,
            Data = new Empty()
        };
    }

    public async Task<Result<Empty>> UpdateContact(Contact contact,
        string? subCategoryName)
    {
        var categoryResult =
            await _categoryService.HandleCategoriesFromContact(
                contact.CategoryId, contact.SubCategoryId, subCategoryName);

        if (!categoryResult.Succeeded)
            return new Result<Empty>
            {
                Succeeded = false,
                Error = categoryResult.Error
            };
        contact.CategoryId = categoryResult.Data!.CategoryId;
        contact.SubCategoryId = categoryResult.Data!.SubCategoryId;

        try
        {
            _repository.UpdateContact(contact);
            await _repository.SaveAsync();
        }
        catch (DbUpdateException e)
        {
            return new Result<Empty>
            {
                Succeeded = false,
                Error = new Error(409,
                    "Contact with given email already exists")
            };
        }

        return new Result<Empty>()
        {
            Succeeded = true,
            Data = new Empty()
        };
    }
}