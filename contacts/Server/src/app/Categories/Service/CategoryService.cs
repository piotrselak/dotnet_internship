using contacts.Server.Categories.Dto;
using contacts.Server.Categories.Repository;
using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Server.Categories.Service;

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

    public async Task<Result<int>> CreateSubCategory(SubCategory subCategory)
    {
        _subCategoryRepository.CreateSubCategory(subCategory);
        await _subCategoryRepository.SaveAsync();
        return new Result<int>
        {
            Succeeded = true,
            Data = ((await _subCategoryRepository.FindSubCategoryByName(
                subCategory.Name))!).Id,
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

    public async Task<Result<CategoriesDto>> HandleCategoriesFromContact(
        int categoryId, int? subCategoryId, string? subCategoryName)
    {
        var category =
            await _categoryRepository.GetCategoryById(categoryId);
        var categoryNotExists = category == null;

        if (categoryNotExists)
            return new Result<CategoriesDto>
            {
                Succeeded = false,
                Error = new Error(404,
                    "Given category does not exist")
            };

        var subCategories =
            await _subCategoryRepository.FindAllByCategoryId(categoryId);

        int? finalSubCategoryId = null;

        if (category!.Name == "Other")
        {
            _logger.LogInformation(subCategoryId.ToString());
            if (subCategoryId != null)
            {
                var finalSubCategory =
                    subCategories.FirstOrDefault(e => e.Id == subCategoryId);
                if (finalSubCategory == null)
                    return new Result<CategoriesDto>
                    {
                        Succeeded = false,
                        Error = new Error(404,
                            "Given SubCategory was not found")
                    };
                finalSubCategoryId = finalSubCategory.Id;
            }
            else
            {
                if (string.IsNullOrEmpty(subCategoryName))
                    return new Result<CategoriesDto>
                    {
                        Succeeded = false,
                        Error = new Error(400,
                            "Neither subcategory was passed nor its name")
                    };
                var subByName = await
                    _subCategoryRepository.FindSubCategoryByName(
                        subCategoryName);
                if (subByName == null)
                {
                    _subCategoryRepository.CreateSubCategory(new SubCategory
                    {
                        Name = subCategoryName,
                        CategoryId = categoryId,
                    });
                    await _subCategoryRepository.SaveAsync();
                    finalSubCategoryId =
                        (await _subCategoryRepository.FindSubCategoryByName(
                            subCategoryName))!.Id;
                }
                else
                {
                    finalSubCategoryId = subByName.Id;
                }
            }
        }
        else
        {
            var finalSubCategory =
                subCategories.FirstOrDefault(e => e.Id == subCategoryId);
            if (finalSubCategory == null) finalSubCategoryId = null;
            else finalSubCategoryId = finalSubCategory.Id;
        }


        return new Result<CategoriesDto>
        {
            Succeeded = true,
            Data = new CategoriesDto
            {
                CategoryId = categoryId,
                SubCategoryId = finalSubCategoryId
            }
        };
    }
}