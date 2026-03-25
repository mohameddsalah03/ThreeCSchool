using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeCSchool.Shared.DTOs;
using ThreeCSchool.Shared.DTOs.Courses;
using ThreeCSchool.Shared.DTOs.Lessons;

namespace ThreeCSchool.Core.Service.Abstraction.Services.Courses
{
    public interface ICourseService
    {
        Task<PagedResultDto<CourseDto>> GetAllAsync(CourseFilterDto filter);
        Task<CourseDto> GetBySlugAsync(string slug);
        Task<PagedResultDto<CourseDto>> GetByCategoryAsync(string categorySlug, int page, int pageSize);
        Task<CourseDto> CreateAsync(CreateCourseDto dto, string instructorId);
        Task<CourseDto> UpdateAsync(int id, UpdateCourseDto dto, string requesterId, bool isAdmin);
        Task DeleteAsync(int id);
        Task<CourseDto> TogglePublishAsync(int id);
        Task<List<LessonDto>> GetLessonsAsync(int courseId);
        Task<LessonDto> AddLessonAsync(int courseId, CreateLessonDto dto);
        Task<LessonDto> UpdateLessonAsync(int courseId, int lessonId, UpdateLessonDto dto);
        Task DeleteLessonAsync(int courseId, int lessonId);
    }
}
