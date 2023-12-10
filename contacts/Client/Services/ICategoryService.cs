using contacts.Client.Domain;
using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Client.Services;

public interface ICategoryService
{
    Task<Result<AllCategoriesWithSub>> GetAllCategories();
    Task<Result<Task>> CreateNewSubCategory(SubCategory subCategory);
}