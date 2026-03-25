using AutoMapper;
using ThreeCSchool.Core.Domain.Models.Data;
using ThreeCSchool.Shared.DTOs.Categories;
using ThreeCSchool.Shared.DTOs.Courses;
using ThreeCSchool.Shared.DTOs.Lessons;

namespace ThreeCSchool.Core.Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Categories 

            // SubCategory , SubCategoryDto
            CreateMap<Category, SubCategoryDto>();

            // Category , CategoryDto
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.SubCategories,opt => opt.MapFrom(src =>src.SubCategories.Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToList()));

            // CreateCategoryDto , Category
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.IsActive,opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.SubCategories,opt => opt.Ignore())
                .ForMember(dest => dest.Courses,opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory,opt => opt.Ignore());

            // Course , CourseDto
            CreateMap<Course, CourseDto>()
                .ForMember(dest=> dest.CategoryName, opt=> opt.MapFrom(src=>src.Category.NameAr))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.DisplayName));

            // Lesson , LessonDto
            CreateMap<Lesson, LessonDto>();

        }
    }
}