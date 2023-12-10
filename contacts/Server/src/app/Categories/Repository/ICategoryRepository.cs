using contacts.Shared;

namespace contacts.Server.Categories.Repository;

public interface ICategoryRepository : IDisposable
{
    Task<List<Category>> GetAllCategories();
    Task<Category?> GetCategoryById(int id);
}