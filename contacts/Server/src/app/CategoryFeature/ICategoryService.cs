using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Server.CategoryFeature;

public interface ICategoryService
{
    Task<Result<Empty>> CreateSubCategory(SubCategory subCategory);
    Task<Result<IEnumerable<Category>>> GetAllCategories();
    Task<Result<IEnumerable<SubCategory>>> GetAllSubCategories();
}