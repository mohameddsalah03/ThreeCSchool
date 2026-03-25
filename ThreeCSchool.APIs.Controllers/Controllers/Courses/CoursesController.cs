using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThreeCSchool.Core.Service.Abstraction.Services.Courses;
using ThreeCSchool.Shared.DTOs;
using ThreeCSchool.Shared.DTOs.Courses;
using ThreeCSchool.Shared.DTOs.Lessons;

namespace ThreeCSchool.Api.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController(ICourseService _courseService) : ControllerBase
    {
        // GET /api/courses
        [HttpGet]
        public async Task<ActionResult<PagedResultDto<CourseDto>>> GetAll(
            [FromQuery] CourseFilterDto filter)
        {
            var result = await _courseService.GetAllAsync(filter);
            return Ok(result);
        }

        // GET /api/courses/{slug}
        [HttpGet("{slug}")]
        public async Task<ActionResult<CourseDto>> GetBySlug(string slug)
        {
            var result = await _courseService.GetBySlugAsync(slug);
            return Ok(result);
        }

        // GET /api/courses/category/{categorySlug}
        [HttpGet("category/{categorySlug}")]
        public async Task<ActionResult<PagedResultDto<CourseDto>>> GetByCategory(
            string categorySlug,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _courseService.GetByCategoryAsync(categorySlug, page, pageSize);
            return Ok(result);
        }

        // POST /api/courses
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<CourseDto>> Create([FromBody] CreateCourseDto dto)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _courseService.CreateAsync(dto, instructorId);

            return CreatedAtAction(nameof(GetBySlug), new { slug = result.Slug }, result);
        }

        // PUT /api/courses/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<CourseDto>> Update(
            int id, [FromBody] UpdateCourseDto dto)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdmin = User.IsInRole("Admin");
            var result = await _courseService.UpdateAsync(id, dto, requesterId, isAdmin);

            return Ok(result);
        }

        // DELETE /api/courses/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteAsync(id);
            return NoContent();
        }

        // PUT /api/courses/{id}/publish
        [HttpPut("{id:int}/publish")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CourseDto>> TogglePublish(int id)
        {
            var result = await _courseService.TogglePublishAsync(id);
            return Ok(result);
        }

        // GET /api/courses/{id}/lessons
        [HttpGet("{id:int}/lessons")]
        public async Task<ActionResult<List<LessonDto>>> GetLessons(int id)
        {
            var result = await _courseService.GetLessonsAsync(id);
            return Ok(result);
        }

        // POST /api/courses/{id}/lessons
        [HttpPost("{id:int}/lessons")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<LessonDto>> AddLesson(
            int id, [FromBody] CreateLessonDto dto)
        {
            var result = await _courseService.AddLessonAsync(id, dto);
            return CreatedAtAction(nameof(GetLessons), new { id }, result);
        }

        // PUT /api/courses/{id}/lessons/{lessonId}
        [HttpPut("{id:int}/lessons/{lessonId:int}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<LessonDto>> UpdateLesson(
            int id, int lessonId, [FromBody] UpdateLessonDto dto)
        {
            var result = await _courseService.UpdateLessonAsync(id, lessonId, dto);
            return Ok(result);
        }

        // DELETE /api/courses/{id}/lessons/{lessonId}
        [HttpDelete("{id:int}/lessons/{lessonId:int}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> DeleteLesson(int id, int lessonId)
        {
            await _courseService.DeleteLessonAsync(id, lessonId);
            return NoContent();
        }
    }
}