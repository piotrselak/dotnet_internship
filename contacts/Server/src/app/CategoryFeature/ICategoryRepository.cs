using contacts.Shared;

namespace contacts.Server.CategoryFeature;

public interface ICategoryRepository : IDisposable
{
    Task<List<Category>> GetAllCategories();
}