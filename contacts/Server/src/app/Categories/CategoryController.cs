using contacts.Server.Categories.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace contacts.Server.Categories;

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

        _logger.LogInformation("Returning result with subcategories of size " +
                               response.Data!.ToList().Count);
        return Ok(response.Data);
    }
}