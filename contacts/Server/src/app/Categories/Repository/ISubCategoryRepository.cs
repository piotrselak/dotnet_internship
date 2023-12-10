using contacts.Shared;

namespace contacts.Server.Categories.Repository;

public interface ISubCategoryRepository : IDisposable
{
    Task<List<SubCategory>> GetAllSubCategories();
    Task<IEnumerable<SubCategory>> FindAllByCategoryId(int id);
    void CreateSubCategory(SubCategory subCategory);
    Task<SubCategory?> FindSubCategoryByName(string name);
    Task<SubCategory?> FindSubCategoryById(int id);
    Task SaveAsync();
}