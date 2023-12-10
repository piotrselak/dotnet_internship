using System.Net.Http.Json;
using contacts.Client.Domain;
using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Client.Services;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(HttpClient httpClient,
        ILogger<CategoryService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<AllCategoriesWithSub>> GetAllCategories()
    {
        var res = await _httpClient.GetFromJsonAsync<IEnumerable<Category>>(
            "api/Category");
        var resSub =
            await _httpClient.GetFromJsonAsync<IEnumerable<SubCategory>>(
                "api/Category/sub");

        if (res == null || resSub == null)
            return new Result<AllCategoriesWithSub>
            {
                Succeeded = false,
                Error = new Error(400, "Could not parse into given type")
            };

        _logger.LogInformation("Got response with categories of size " +
                               res.ToList().Count);

        var all = new Dictionary<Category, IEnumerable<SubCategory>>();
        foreach (var category in res)
        {
            var subs = new List<SubCategory>();
            foreach (var subCategory in resSub)
            {
                if (category.Id == subCategory.CategoryId)
                    subs.Add(subCategory);
            }

            all.Add(category, subs);
        }

        return new Result<AllCategoriesWithSub>
        {
            Succeeded = true,
            Data = new AllCategoriesWithSub
            {
                CategoryDictionary = all
            }
        };
    }

    public async Task<Result<Task>> CreateNewSubCategory(
        SubCategory subCategory)
    {
        throw new NotImplementedException();
    }
}