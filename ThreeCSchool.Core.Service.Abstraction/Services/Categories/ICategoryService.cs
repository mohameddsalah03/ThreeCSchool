using ThreeCSchool.Shared.DTOs.Categories;

namespace ThreeCSchool.Core.Service.Abstraction.Services.Categories
{
    public interface ICategoryService
    {
        // every top-level categories with subcategories
        Task<IEnumerable<CategoryDto>> GetAllWithSubsAsync();

        // category + subcategories with slug  
        Task<CategoryDto> GetBySlugAsync(string slug);

        // ParentCategoryId = null  → top-level
        // ParentCategoryId = int   → subcategory
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);

        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto);
        Task DeleteAsync(int id);
    }
}