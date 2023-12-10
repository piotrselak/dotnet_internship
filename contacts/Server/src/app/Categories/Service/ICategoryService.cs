using contacts.Server.Categories.Dto;
using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Server.Categories.Service;

public interface ICategoryService
{
    Task<Result<int>> CreateSubCategory(SubCategory subCategory);
    Task<Result<IEnumerable<Category>>> GetAllCategories();
    Task<Result<IEnumerable<SubCategory>>> GetAllSubCategories();

    // Used when needed by the contact service while creating/modifying one
    Task<Result<CategoriesDto>> HandleCategoriesFromContact(int categoryId,
        int? subCategoryId, string? subCategoryName);
}