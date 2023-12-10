using contacts.Server.Database;
using contacts.Shared;
using Microsoft.EntityFrameworkCore;

namespace contacts.Server.CategoryFeature.Repository;

public class CategoryRepository : ICategoryRepository
{
    private ContactsContext _context;

    public CategoryRepository(ContactsContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(e => e.Id == id);
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) _context.Dispose();
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}