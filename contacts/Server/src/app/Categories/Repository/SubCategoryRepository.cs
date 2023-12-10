using contacts.Server.Database;
using contacts.Shared;
using Microsoft.EntityFrameworkCore;

namespace contacts.Server.Categories.Repository;

public class SubCategoryRepository : ISubCategoryRepository
{
    private ContactsContext _context;

    public SubCategoryRepository(ContactsContext context)
    {
        _context = context;
    }

    public async Task<List<SubCategory>> GetAllSubCategories()
    {
        return await _context.SubCategories.Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<IEnumerable<SubCategory>> FindAllByCategoryId(int id)
    {
        return await _context.SubCategories.Where(e => e.CategoryId == id)
            .ToListAsync();
    }

    public void CreateSubCategory(SubCategory subCategory)
    {
        _context.SubCategories.Add(subCategory);
    }

    public async Task<SubCategory?> FindSubCategoryByName(string name)
    {
        return await _context.SubCategories.FirstOrDefaultAsync(e =>
            e.Name == name);
    }

    public async Task<SubCategory?> FindSubCategoryById(int id)
    {
        return await _context.SubCategories.FirstOrDefaultAsync(e =>
            e.Id == id);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
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