using System.Net.Http.Json;
using contacts.Client.Domain;
using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Client.Services;

public class ContactService : IContactService
{
    private readonly HttpClient _httpClient;

    public ContactService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<BriefContact>> GetContacts()
    {
        // This should never fail (unless critical server error happens)
        return (await _httpClient.GetFromJsonAsync<IEnumerable<BriefContact>>(
            "api/Contact"))!;
    }

    public async Task<Result<Contact>> GetContactById(int id)
    {
        var res = await _httpClient.GetAsync($"api/Contact/{id}");
        if (!res.IsSuccessStatusCode)
            return new Result<Contact>
            {
                Succeeded = false,
                Error = new Error((int)res.StatusCode,
                    (await res.Content.ReadFromJsonAsync<ErrorResponse>())!
                    .Detail)
            };
        return new Result<Contact>
        {
            Succeeded = true,
            Data = await res.Content.ReadFromJsonAsync<Contact>(),
        };
    }

    public async Task<Result<Empty>> DeleteContact(int id)
    {
        var res = await _httpClient.DeleteAsync($"api/Contact/{id}");
        if (!res.IsSuccessStatusCode)
            return new Result<Empty>
            {
                Succeeded = false,
                Error = new Error((int)res.StatusCode,
                    (await res.Content.ReadFromJsonAsync<ErrorResponse>())!
                    .Detail)
            };
        return new Result<Empty>
        {
            Succeeded = true,
            Data = new Empty()
        };
    }

    public async Task<Result<Empty>> CreateContact(Contact contact)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Empty>> UpdateContract(Contact contact)
    {
        throw new NotImplementedException();
    }
}