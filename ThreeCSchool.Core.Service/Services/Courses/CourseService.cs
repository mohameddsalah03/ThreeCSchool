using ThreeCSchool.Core.Domain.Contracts.Persistence;
using ThreeCSchool.Core.Domain.Models.Data;
using ThreeCSchool.Core.Domain.Models.Data.Enums;
using ThreeCSchool.Core.Domain.Specifications.Courses;
using ThreeCSchool.Core.Domain.Specifications.Lessons;
using ThreeCSchool.Core.Service.Abstraction.Services.Courses;
using ThreeCSchool.Shared.DTOs;
using ThreeCSchool.Shared.DTOs.Courses;
using ThreeCSchool.Shared.DTOs.Lessons;
using ThreeCSchool.Shared.Exceptions;
using AutoMapper;

namespace ThreeCSchool.Core.Service.Services.Courses
{
    public class CourseService(IUnitOfWork _uow, IMapper _mapper) : ICourseService
    {
        // ── GET ALL ────────────────────────────────────────────────────────────
        public async Task<PagedResultDto<CourseDto>> GetAllAsync(CourseFilterDto filter)
        {
            var spec = new CourseFilterSpec(filter);
            var countSpec = new CourseFilterCountSpec(filter);

            var courses = await _uow.GetRepo<Course, int>().GetAllWithSpecAsync(spec);
            var totalCount = await _uow.GetRepo<Course, int>().GetCountAsync(countSpec);

            return new PagedResultDto<CourseDto>
            {
                Items = _mapper.Map<IEnumerable<CourseDto>>(courses).ToList(),
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        // ── GET BY SLUG ────────────────────────────────────────────────────────
        public async Task<CourseDto> GetBySlugAsync(string slug)
        {
            var course = await _uow.GetRepo<Course, int>()
                             .GetWithSpecAsync(new CourseWithDetailsSpec(slug))
                         ?? throw new NotFoundException($"Course '{slug}' not found.");

            return _mapper.Map<CourseDto>(course);
        }

        // ── GET BY CATEGORY ────────────────────────────────────────────────────
        public async Task<PagedResultDto<CourseDto>> GetByCategoryAsync(
            string categorySlug, int page, int pageSize)
        {
            var spec = new CourseByCategorySpec(categorySlug, page, pageSize);
            var countSpec = new CourseByCategoryCountSpec(categorySlug);

            var courses = await _uow.GetRepo<Course, int>().GetAllWithSpecAsync(spec);
            var totalCount = await _uow.GetRepo<Course, int>().GetCountAsync(countSpec);

            return new PagedResultDto<CourseDto>
            {
                Items = _mapper.Map<IEnumerable<CourseDto>>(courses).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        // ── CREATE ─────────────────────────────────────────────────────────────
        public async Task<CourseDto> CreateAsync(CreateCourseDto dto, string instructorId)
        {
            var slugExists = await _uow.GetRepo<Course, int>()
                .GetWithSpecAsync(new CourseBySlugExistsSpec(dto.Slug));

            if (slugExists is not null)
                throw new BadRequestException("Slug already exists.");

            var course = new Course
            {
                TitleEn = dto.TitleEn,
                TitleAr = dto.TitleAr,
                Slug = dto.Slug,
                DescriptionEn = dto.DescriptionEn,
                DescriptionAr = dto.DescriptionAr,
                ThumbnailUrl = dto.ThumbnailUrl,
                Type = (CourseType)dto.Type,
                Price = dto.Price,
                DiscountPrice = dto.DiscountPrice,
                IsDiscount = dto.IsDiscount,
                IsFree = dto.IsFree,
                IsDownloadable = dto.IsDownloadable,
                IsUpcoming = dto.IsUpcoming,
                CategoryId = dto.CategoryId,
                InstructorId = instructorId
            };

            await _uow.GetRepo<Course, int>().AddAsync(course);
            await _uow.SaveChangesAsync();

            var created = await _uow.GetRepo<Course, int>()
                              .GetWithSpecAsync(new CourseWithDetailsSpec(course.Id))
                          ?? throw new NotFoundException("Course not found after creation.");

            return _mapper.Map<CourseDto>(created);
        }

        // ── UPDATE ─────────────────────────────────────────────────────────────
        public async Task<CourseDto> UpdateAsync(
            int id, UpdateCourseDto dto, string requesterId, bool isAdmin)
        {
            var course = await _uow.GetRepo<Course, int>()
                             .GetWithSpecAsync(new CourseWithDetailsSpec(id))
                         ?? throw new NotFoundException("Course not found.");

            if (!isAdmin && course.InstructorId != requesterId)
                throw new ForbiddenException("You don't own this course.");

            if (dto.TitleEn != null) course.TitleEn = dto.TitleEn;
            if (dto.TitleAr != null) course.TitleAr = dto.TitleAr;
            if (dto.DescriptionEn != null) course.DescriptionEn = dto.DescriptionEn;
            if (dto.DescriptionAr != null) course.DescriptionAr = dto.DescriptionAr;
            if (dto.ThumbnailUrl != null) course.ThumbnailUrl = dto.ThumbnailUrl;
            if (dto.Type != null) course.Type = (CourseType)dto.Type;
            if (dto.Price != null) course.Price = dto.Price.Value;
            if (dto.DiscountPrice != null) course.DiscountPrice = dto.DiscountPrice;
            if (dto.IsDiscount != null) course.IsDiscount = dto.IsDiscount.Value;
            if (dto.IsFree != null) course.IsFree = dto.IsFree.Value;
            if (dto.IsDownloadable != null) course.IsDownloadable = dto.IsDownloadable.Value;
            if (dto.IsUpcoming != null) course.IsUpcoming = dto.IsUpcoming.Value;
            if (dto.CategoryId != null) course.CategoryId = dto.CategoryId.Value;

            course.UpdatedAt = DateTime.UtcNow;

            _uow.GetRepo<Course, int>().Update(course);
            await _uow.SaveChangesAsync();

            return _mapper.Map<CourseDto>(course);
        }

        // ── DELETE (Soft) ──────────────────────────────────────────────────────
        public async Task DeleteAsync(int id)
        {
            var course = await _uow.GetRepo<Course, int>().GetByIdAsync(id)
                         ?? throw new NotFoundException("Course not found.");

            course.IsDeleted = true;
            course.UpdatedAt = DateTime.UtcNow;

            _uow.GetRepo<Course, int>().Update(course);
            await _uow.SaveChangesAsync();
        }

        // ── TOGGLE PUBLISH ─────────────────────────────────────────────────────
        public async Task<CourseDto> TogglePublishAsync(int id)
        {
            var course = await _uow.GetRepo<Course, int>()
                             .GetWithSpecAsync(new CourseWithDetailsSpec(id))
                         ?? throw new NotFoundException("Course not found.");

            course.IsPublished = !course.IsPublished;
            course.UpdatedAt = DateTime.UtcNow;

            _uow.GetRepo<Course, int>().Update(course);
            await _uow.SaveChangesAsync();

            return _mapper.Map<CourseDto>(course);
        }

        // ── GET LESSONS ────────────────────────────────────────────────────────
        public async Task<List<LessonDto>> GetLessonsAsync(int courseId)
        {
            _ = await _uow.GetRepo<Course, int>().GetByIdAsync(courseId)
                ?? throw new NotFoundException("Course not found.");

            var lessons = await _uow.GetRepo<Lesson, int>()
                .GetAllWithSpecAsync(new LessonByCourseSpec(courseId));

            return _mapper.Map<IEnumerable<LessonDto>>(lessons).ToList();
        }

        // ── ADD LESSON ─────────────────────────────────────────────────────────
        public async Task<LessonDto> AddLessonAsync(int courseId, CreateLessonDto dto)
        {
            _ = await _uow.GetRepo<Course, int>().GetByIdAsync(courseId)
                ?? throw new NotFoundException("Course not found.");

            var lesson = new Lesson
            {
                CourseId = courseId,
                TitleEn = dto.TitleEn,
                TitleAr = dto.TitleAr,
                VideoUrl = dto.VideoUrl,
                ContentText = dto.ContentText,
                DurationInMinutes = dto.DurationInMinutes,
                DisplayOrder = dto.DisplayOrder,
                IsFreePreview = dto.IsFreePreview,
            };

            await _uow.GetRepo<Lesson, int>().AddAsync(lesson);
            await _uow.SaveChangesAsync();

            return _mapper.Map<LessonDto>(lesson);
        }

        // ── UPDATE LESSON ──────────────────────────────────────────────────────
        public async Task<LessonDto> UpdateLessonAsync(
            int courseId, int lessonId, UpdateLessonDto dto)
        {
            var lesson = await _uow.GetRepo<Lesson, int>()
                             .GetWithSpecAsync(new LessonByIdAndCourseSpec(courseId, lessonId))
                         ?? throw new NotFoundException("Lesson not found.");

            if (dto.TitleEn != null) lesson.TitleEn = dto.TitleEn;
            if (dto.TitleAr != null) lesson.TitleAr = dto.TitleAr;
            if (dto.VideoUrl != null) lesson.VideoUrl = dto.VideoUrl;
            if (dto.ContentText != null) lesson.ContentText = dto.ContentText;
            if (dto.DurationInMinutes != null) lesson.DurationInMinutes = dto.DurationInMinutes;
            if (dto.DisplayOrder != null) lesson.DisplayOrder = dto.DisplayOrder.Value;
            if (dto.IsFreePreview != null) lesson.IsFreePreview = dto.IsFreePreview.Value;

            lesson.UpdatedAt = DateTime.UtcNow;

            _uow.GetRepo<Lesson, int>().Update(lesson);
            await _uow.SaveChangesAsync();

            return _mapper.Map<LessonDto>(lesson);
        }

        // ── DELETE LESSON ──────────────────────────────────────────────────────
        public async Task DeleteLessonAsync(int courseId, int lessonId)
        {
            var lesson = await _uow.GetRepo<Lesson, int>()
                             .GetWithSpecAsync(new LessonByIdAndCourseSpec(courseId, lessonId))
                         ?? throw new NotFoundException("Lesson not found.");

            lesson.IsDeleted = true;
            lesson.UpdatedAt = DateTime.UtcNow;

            _uow.GetRepo<Lesson, int>().Update(lesson);
            await _uow.SaveChangesAsync();
        }
    }
}