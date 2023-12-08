﻿using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Client.Services;

public interface IContactService
{
    Task<IEnumerable<BriefContact>> GetContacts();
    Task<Result<Contact>> GetContactById(int id);
}