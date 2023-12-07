using System.Net.Http.Json;
using contacts.Shared;

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
        // Todo rethink suppression
        return (await _httpClient.GetFromJsonAsync<IEnumerable<BriefContact>>("api/Contact"))!;
    }
}