using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Server.CategoryFeature;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository categoryRepository,
        ISubCategoryRepository subCategoryRepository,
        ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
        _logger = logger;
    }

    public async Task<Result<Empty>> CreateSubCategory(SubCategory subCategory)
    {
        _subCategoryRepository.CreateSubCategory(subCategory);
        await _subCategoryRepository.SaveAsync();
        return new Result<Empty>
        {
            Succeeded = true,
            Data = new Empty(),
        };
    }

    public async Task<Result<IEnumerable<Category>>> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllCategories();

        return new Result<IEnumerable<Category>>
        {
            Succeeded = true,
            Data = categories
        };
    }

    public async Task<Result<IEnumerable<SubCategory>>> GetAllSubCategories()
    {
        var subCategories = await _subCategoryRepository.GetAllSubCategories();

        return new Result<IEnumerable<SubCategory>>
        {
            Succeeded = true,
            Data = subCategories
        };
    }
}