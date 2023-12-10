using contacts.Server.CategoryFeature.Service;
using Microsoft.AspNetCore.Mvc;
using contacts.Shared;
using Microsoft.AspNetCore.Authorization;

namespace contacts.Server.CategoryFeature;

[ApiController]
[Route("/api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService,
        ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var response = await _categoryService.GetAllCategories();

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        _logger.LogInformation("Returning result with categories of size " +
                               response.Data!.ToList().Count);
        return Ok(response.Data);
    }

    [AllowAnonymous]
    [HttpGet("sub")]
    public async Task<IActionResult> GetAllSubCategories()
    {
        var response = await _categoryService.GetAllSubCategories();

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        _logger.LogInformation("Returning result with categories of size " +
                               response.Data!.ToList().Count);
        return Ok(response.Data);
    }

    [Authorize]
    [HttpPost("{id:int}")]
    public async Task<IActionResult> CreateSubCategory(
        [FromBody] SubCategory subCategory, int id)
    {
        subCategory.CategoryId = id;
        var response = await _categoryService.CreateSubCategory(subCategory);

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        return Ok(response.Data);
    }
}