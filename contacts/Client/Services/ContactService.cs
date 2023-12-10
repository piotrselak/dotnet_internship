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
            var parsed =
                await res.Content
                    .ReadFromJsonAsync<ErrorResponseValidation>();
            string errors = "";
            foreach (var (key, value) in parsed!.Errors)
            {
                errors += key + ": " + string.Join(", ", value);
            }

            return new Result<Empty>
            {
                Succeeded = false,
                Error = new Error((int)res.StatusCode, errors)
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
            _logger.LogInformation("Updating failed.");
            // var parsed =
            //     await res.Content
            //         .ReadFromJsonAsync<ErrorResponseValidation>();
            string errors = "";
            // if (parsed != null && parsed.Errors.Count > 0)
            // {
            //     _logger.LogInformation("ValidationResponseError");
            //     foreach (var (key, value) in parsed!.Errors)
            //     {
            //         errors += key + ": " + string.Join(", ", value);
            //     }
            // }
            // else
            // {
            _logger.LogInformation("Not VRE");
            errors = (await res.Content
                .ReadFromJsonAsync<ErrorResponse>())!.Detail;
            // }

            _logger.LogInformation(errors);
            return new Result<Empty>
            {
                Succeeded = false,
                Error = new Error((int)res.StatusCode, errors)
            };
        }

        return new Result<Empty>
        {
            Succeeded = true,
            Data = new Empty()
        };
    }
}