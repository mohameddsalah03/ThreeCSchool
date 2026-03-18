using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreeCSchool.APIs.Controllers.Controllers.Base;
using ThreeCSchool.Core.Service.Abstraction.Services;
using ThreeCSchool.Shared.DTOs.Categories;

namespace ThreeCSchool.APIs.Controllers.Controllers.Categories
{
    public class CategoriesController(IServiceManager _serviceManager)
        : BaseApiController
    {

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
            => Ok(await _serviceManager.CategoryService.GetAllWithSubsAsync());

       
        // Example: GET /api/categories/web-development
        [AllowAnonymous]
        [HttpGet("{slug}")]
        public async Task<ActionResult<CategoryDto>> GetBySlug(string slug)
            =>Ok(await _serviceManager.CategoryService.GetBySlugAsync(slug));


        #region Admin Only 

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
        {
            var result = await _serviceManager.CategoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetBySlug), new { slug = result.Slug }, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryDto dto)
            => Ok(await _serviceManager.CategoryService.UpdateAsync(id, dto));

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _serviceManager.CategoryService.DeleteAsync(id);
            return Ok(new { message = "Category deactivated successfully." });
        } 

        #endregion


    }
}
