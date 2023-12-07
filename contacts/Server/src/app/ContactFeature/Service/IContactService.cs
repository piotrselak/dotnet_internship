﻿using contacts.Server.Result;
using contacts.Shared;

namespace contacts.Server.ContactFeature.Service;

// ContactService will be used as a business logic holder.
public interface IContactService
{
    // It cannot fail so there is no point to wrap it in ServiceResult
    Task<IEnumerable<BriefContact>> GetContactList();
    Task<Result<Contact>> GetContactDetails(int id);
    Task<Result<Empty>> RemoveContact(int id);
    Task<Result<Empty>> AddContact(Contact contact);
    Task<Result<Empty>> UpdateContact(Contact contact);
}