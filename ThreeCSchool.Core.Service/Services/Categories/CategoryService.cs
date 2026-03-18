using AutoMapper;
using ThreeCSchool.Core.Domain.Contracts.Persistence;
using ThreeCSchool.Core.Domain.Models.Data;
using ThreeCSchool.Core.Domain.Specifications.Categories;
using ThreeCSchool.Core.Service.Abstraction.Services.Categories;
using ThreeCSchool.Shared.DTOs.Categories;
using ThreeCSchool.Shared.Exceptions;

namespace ThreeCSchool.Core.Service.Services.Categories
{
    public class CategoryService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : ICategoryService
    {
        private readonly IGenericRepository<Category, int> _repo =
            _unitOfWork.GetRepo<Category, int>();

        
        public async Task<IEnumerable<CategoryDto>> GetAllWithSubsAsync()
        {
            var spec = new CategoryWithSubsSpecification();
            var categories = await _repo.GetAllWithSpecAsync(spec);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        
        public async Task<CategoryDto> GetBySlugAsync(string slug)
        {
            var spec = new CategoryWithSubsSpecification(slug);
            var category = await _repo.GetWithSpecAsync(spec);

            if (category is null)
                throw new NotFoundException($"Category '{slug}' was not found.");

            return _mapper.Map<CategoryDto>(category);
        }

       
        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            // 1. فحص تكرار الـ Slug
            var slugTaken = await _repo.GetWithSpecAsync(new CategorySlugExistsSpecification(dto.Slug));

            if (slugTaken is not null)
                throw new BadRequestException($"Slug '{dto.Slug}' is already used by another category.");

            // 2. لو فيه ParentCategoryId — تحقق إنه موجود وإنه مش subcategory
            if (dto.ParentCategoryId.HasValue)
            {
                var parent = await _repo.GetByIdAsync(dto.ParentCategoryId.Value);

                if (parent is null)
                    throw new NotFoundException( "Category", dto.ParentCategoryId.Value);

                // الـ UI بيظهر مستويين بس — نمنع المستوى الثالث
                if (parent.ParentCategoryId.HasValue)
                    throw new BadRequestException("Maximum 2 levels allowed. " + "Cannot create a subcategory under another subcategory.");
            }

            // 3. Map + Save
            var category = _mapper.Map<Category>(dto);

            await _repo.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            // 4. Fetch with SubCategories للـ Response
            var spec = new CategoryWithSubsSpecification(category.Id, true);
            var created = await _repo.GetWithSpecAsync(spec);

            return _mapper.Map<CategoryDto>(created!);
        }

       
        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            // 1. جيب الـ Category
            var spec = new CategoryWithSubsSpecification(id, true);
            var category = await _repo.GetWithSpecAsync(spec);

            if (category is null)
                throw new NotFoundException("Category", id);

            // 2. لو الـ Slug اتغير — تحقق مش متكررش
            if (!string.IsNullOrWhiteSpace(dto.Slug)
                && dto.Slug != category.Slug)
            {
                var slugTaken = await _repo.GetWithSpecAsync(new CategorySlugExistsSpecification(dto.Slug, id));

                if (slugTaken is not null)
                    throw new BadRequestException($"Slug '{dto.Slug}' is already used by another category.");
            }

            // 3. Partial Update — بس الحقول اللي اتبعتت (مش null)
            if (!string.IsNullOrWhiteSpace(dto.NameEn))
                category.NameEn = dto.NameEn;

            if (!string.IsNullOrWhiteSpace(dto.NameAr))
                category.NameAr = dto.NameAr;

            if (!string.IsNullOrWhiteSpace(dto.Slug))
                category.Slug = dto.Slug;

            if (!string.IsNullOrWhiteSpace(dto.IconUrl))
                category.IconUrl = dto.IconUrl;

            if (dto.DisplayOrder.HasValue)
                category.DisplayOrder = dto.DisplayOrder.Value;

            if (dto.IsActive.HasValue)
                category.IsActive = dto.IsActive.Value;

            // 4. Save
            _repo.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        
        public async Task DeleteAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);

            if (category is null)
                throw new NotFoundException("Category", id);

            // soft delete
            category.IsActive = false;

            _repo.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}