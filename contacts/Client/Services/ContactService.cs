using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using contacts.Client.Domain;
using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Client.Services;

public class ContactService : IContactService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ContactService> _logger;

    public ContactService(HttpClient httpClient, ILogger<ContactService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
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

    public async Task<Result<Empty>> CreateNewContact(CreateContact contact)
    {
        var stringContent = new StringContent(
            JsonSerializer.Serialize(contact), Encoding.UTF8,
            "application/json");
        var res = await _httpClient.PostAsync($"api/Contact/", stringContent);
        if (!res.IsSuccessStatusCode)
        {
            var error = await ParseError(res.Content);

            return new Result<Empty>
            {
                Succeeded = false,
                Error = error
            };
        }

        return new Result<Empty>
        {
            Succeeded = true,
            Data = new Empty()
        };
    }

    public async Task<Result<Empty>> UpdateContract(CreateContact contact,
        int id)
    {
        var stringContent = new StringContent(
            JsonSerializer.Serialize(contact), Encoding.UTF8,
            "application/json");
        HttpResponseMessage res =
            await _httpClient.PutAsync($"api/Contact/{id}",
                stringContent);

        if (!res.IsSuccessStatusCode)
        {
            var error = await ParseError(res.Content);
            return new Result<Empty>
            {
                Succeeded = false,
                Error = error
            };
        }

        return new Result<Empty>
        {
            Succeeded = true,
            Data = new Empty()
        };
    }

    private async Task<Error> ParseError(HttpContent errorContent)
    {
        var parsed =
            await errorContent.ReadFromJsonAsync<ErrorResponseValidation>();

        if (parsed != null)
        {
            _logger.LogInformation("Parsed to ErrorResponseValidation");
            string errors = "";

            foreach (var (key, value) in parsed!.Errors)
            {
                errors += key + ": " + string.Join(" ", value) + " ";
            }

            return new Error(0, errors);
        }

        var parsed2 = await errorContent.ReadFromJsonAsync<ErrorResponse>();
        if (parsed2 != null)
        {
            _logger.LogInformation("Parsed to ErrorResponse");
            return new Error(0, parsed2.Detail);
        }

        throw new Exception("Could not parse validation error!");
    }
}