using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThreeCSchool.Core.Domain.Contracts.Persistence;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data
{
    public class DataSeeding(
        ThreeCDbContext _context,
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager
        ) : IDataSeeding
    {
        public async Task InitializeAsync()
        {
            var pending = await _context.Database.GetPendingMigrationsAsync();
            if (pending.Any())
                await _context.Database.MigrateAsync();
        }

        public async Task DataSeedAsync()
        {
            try
            {
                await SeedRolesAsync();
                await SeedUsersAsync();
                await SeedCategoriesAsync();
                await SeedPricingPlansAsync();
                await SeedFAQsAsync();
                await SeedTestimonialsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in DataSeeding: {ex.Message}", ex);
            }
        }

        private async Task SeedRolesAsync()
        {
            if (_roleManager.Roles.Any()) return;

            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("Student"));
            await _roleManager.CreateAsync(new IdentityRole("Instructor"));
            await _roleManager.CreateAsync(new IdentityRole("Organization"));
        }

        private async Task SeedUsersAsync()
        {
            if (_userManager.Users.Any()) return;

            var admin = new ApplicationUser
            {
                Id = "admin-seed-id-001",
                Email = "admin@3cschool.net",
                UserName = "admin_3cschool",
                DisplayName = "3C Admin",
                PhoneNumber = "01000000000",
                EmailConfirmed = true,
                IsActive = true,
                TimeZone = "Africa/Cairo"
            };

            var instructor1 = new ApplicationUser
            {
                Id = "instructor-seed-id-001",
                Email = "instructor1@3cschool.net",
                UserName = "instructor_ahmed",
                DisplayName = "Ahmed Instructor",
                PhoneNumber = "01111111111",
                EmailConfirmed = true,
                IsActive = true,
                TimeZone = "Africa/Cairo"
            };

            var student1 = new ApplicationUser
            {
                Id = "student-seed-id-001",
                Email = "student1@3cschool.net",
                UserName = "student_omar",
                DisplayName = "Omar Student",
                PhoneNumber = "01222222222",
                EmailConfirmed = true,
                IsActive = true,
                TimeZone = "Africa/Cairo"
            };

            await _userManager.CreateAsync(admin, "Admin@123");
            await _userManager.CreateAsync(instructor1, "Instructor@123");
            await _userManager.CreateAsync(student1, "Student@123");

            await _userManager.AddToRoleAsync(admin, "Admin");
            await _userManager.AddToRoleAsync(instructor1, "Instructor");
            await _userManager.AddToRoleAsync(student1, "Student");
        }

        private async Task SeedCategoriesAsync()
        {
            if (_context.Categories.Any()) return;

            var categories = new List<Category>
            {
                new() { NameEn = "AI Learning Program", NameAr = "برنامج تعلم الذكاء الاصطناعي", Slug = "ai-learning-program", DisplayOrder = 1 },
                new() { NameEn = "Text-based Coding", NameAr = "البرمجة النصية", Slug = "text-based-coding", DisplayOrder = 2 },
                new() { NameEn = "Visual Programming", NameAr = "البرمجة المرئية", Slug = "visual-programming", DisplayOrder = 3 },
                new() { NameEn = "Web Development", NameAr = "تطوير الويب", Slug = "web-development", DisplayOrder = 4 },
                new() { NameEn = "Blocks-based Programming", NameAr = "البرمجة بالكتل", Slug = "blocks-based-programming", DisplayOrder = 5 },
                new() { NameEn = "Standard Level", NameAr = "المستوى القياسي", Slug = "standard-level", DisplayOrder = 6 },
                new() { NameEn = "Professional Level", NameAr = "المستوى الاحترافي", Slug = "professional-level", DisplayOrder = 7 },
                new() { NameEn = "Graphic", NameAr = "جرافيك", Slug = "graphic", DisplayOrder = 8 },
                new() { NameEn = "Reattention Sessions", NameAr = "جلسات إعادة التركيز", Slug = "reattention-sessions", DisplayOrder = 9 }
            };

            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            // Subcategories
            var textBased = _context.Categories.First(c => c.Slug == "text-based-coding");
            var webDev = _context.Categories.First(c => c.Slug == "web-development");
            var blocksBasedId = _context.Categories.First(c => c.Slug == "blocks-based-programming").Id;
            var standardId = _context.Categories.First(c => c.Slug == "standard-level").Id;
            var professionalId = _context.Categories.First(c => c.Slug == "professional-level").Id;

            var subCategories = new List<Category>
            {
                // Text-based Coding
                new() { NameEn = "Text Based Coding – Python", NameAr = "برمجة نصية - بايثون", Slug = "text-based-coding-python", ParentCategoryId = textBased.Id, DisplayOrder = 1 },
                // Web Development
                new() { NameEn = "Web Juniors", NameAr = "ويب للمبتدئين", Slug = "web-juniors", ParentCategoryId = webDev.Id, DisplayOrder = 1 },
                new() { NameEn = "Intro to Web – HTML and CSS", NameAr = "مقدمة للويب", Slug = "intro-to-web-html-css", ParentCategoryId = webDev.Id, DisplayOrder = 2 },
                new() { NameEn = "Web Development", NameAr = "تطوير الويب المتقدم", Slug = "web-development-advanced", ParentCategoryId = webDev.Id, DisplayOrder = 3 },
                // Blocks-based Programming
                new() { NameEn = "App Inventor – Scratch", NameAr = "اب انفنتور - سكراتش", Slug = "app-inventor-scratch", ParentCategoryId = blocksBasedId, DisplayOrder = 1 },
                // Standard Level
                new() { NameEn = "Standard Level – Scratch & APP Inventor", NameAr = "مستوى قياسي - سكراتش", Slug = "standard-scratch-app-inventor", ParentCategoryId = standardId, DisplayOrder = 1 },
                new() { NameEn = "Standard Level – AI Junior", NameAr = "مستوى قياسي - ذكاء اصطناعي", Slug = "standard-ai-junior", ParentCategoryId = standardId, DisplayOrder = 2 },
                new() { NameEn = "Standard Level – Python", NameAr = "مستوى قياسي - بايثون", Slug = "standard-python", ParentCategoryId = standardId, DisplayOrder = 3 },
                new() { NameEn = "Standard Level – Html & Css", NameAr = "مستوى قياسي - HTML", Slug = "standard-html-css", ParentCategoryId = standardId, DisplayOrder = 4 },
                // Professional Level
                new() { NameEn = "Web Full Stack", NameAr = "فول ستاك ويب", Slug = "web-full-stack", ParentCategoryId = professionalId, DisplayOrder = 1 },
                new() { NameEn = "Mobile Apps Development", NameAr = "تطوير تطبيقات الجوال", Slug = "mobile-apps-development", ParentCategoryId = professionalId, DisplayOrder = 2 },
                new() { NameEn = "Games Development", NameAr = "تطوير الألعاب", Slug = "games-development", ParentCategoryId = professionalId, DisplayOrder = 3 },
                new() { NameEn = "Artificial Intelligence", NameAr = "الذكاء الاصطناعي", Slug = "artificial-intelligence", ParentCategoryId = professionalId, DisplayOrder = 4 },
                new() { NameEn = "Cybersecurity", NameAr = "الأمن السيبراني", Slug = "cybersecurity", ParentCategoryId = professionalId, DisplayOrder = 5 }
            };

            await _context.Categories.AddRangeAsync(subCategories);
            await _context.SaveChangesAsync();
        }

        private async Task SeedPricingPlansAsync()
        {
            if (_context.PricingPlans.Any()) return;

            var plans = new List<PricingPlan>
            {
                new()
                {
                    NameEn = "Quarter Plan", NameAr = "الخطة الربعية",
                    SubtitleEn = "3-Month Plan", SubtitleAr = "خطة 3 أشهر",
                    DurationInMonths = 3,
                    Price = 3950, OriginalPrice = 5900,
                    LevelCompletionCount = 1,
                    IsFeatured = false, IsActive = true, DisplayOrder = 1,
                    Features = new List<PlanFeature>
                    {
                        new() { DescriptionEn = "live online session", DescriptionAr = "جلسة مباشرة أونلاين", IsIncluded = true, DisplayOrder = 1 },
                        new() { DescriptionEn = "assessment & quizzes", DescriptionAr = "تقييمات واختبارات", IsIncluded = true, DisplayOrder = 2 },
                        new() { DescriptionEn = "Compilation Certificate", DescriptionAr = "شهادة إتمام", IsIncluded = true, DisplayOrder = 3 },
                        new() { DescriptionEn = "Technical Guidance", DescriptionAr = "توجيه تقني", IsIncluded = true, DisplayOrder = 4 },
                        new() { DescriptionEn = "Limited Group", DescriptionAr = "مجموعة محدودة", IsIncluded = true, DisplayOrder = 5 },
                        new() { DescriptionEn = "graduation projects", DescriptionAr = "مشاريع تخرج", IsIncluded = true, DisplayOrder = 6 },
                        new() { DescriptionEn = "1-Level Completion", DescriptionAr = "إتمام مستوى واحد", IsIncluded = true, DisplayOrder = 7 }
                    }
                },
                new()
                {
                    NameEn = "Half Annual Plan", NameAr = "خطة نصف السنة",
                    SubtitleEn = "6-Month Plan", SubtitleAr = "خطة 6 أشهر",
                    DurationInMonths = 6,
                    Price = 6950, OriginalPrice = 10800,
                    LevelCompletionCount = 2,
                    IsFeatured = true, IsActive = true, DisplayOrder = 2,
                    Features = new List<PlanFeature>
                    {
                        new() { DescriptionEn = "live online session", DescriptionAr = "جلسة مباشرة أونلاين", IsIncluded = true, DisplayOrder = 1 },
                        new() { DescriptionEn = "assessment & quizzes", DescriptionAr = "تقييمات واختبارات", IsIncluded = true, DisplayOrder = 2 },
                        new() { DescriptionEn = "Compilation Certificate", DescriptionAr = "شهادة إتمام", IsIncluded = true, DisplayOrder = 3 },
                        new() { DescriptionEn = "Technical Guidance", DescriptionAr = "توجيه تقني", IsIncluded = true, DisplayOrder = 4 },
                        new() { DescriptionEn = "Limited Group", DescriptionAr = "مجموعة محدودة", IsIncluded = true, DisplayOrder = 5 },
                        new() { DescriptionEn = "graduation projects", DescriptionAr = "مشاريع تخرج", IsIncluded = true, DisplayOrder = 6 },
                        new() { DescriptionEn = "2-Level Completion", DescriptionAr = "إتمام مستويين", IsIncluded = true, DisplayOrder = 7 }
                    }
                },
                new()
                {
                    NameEn = "Annual Plan", NameAr = "الخطة السنوية",
                    SubtitleEn = "12-Month Plan", SubtitleAr = "خطة 12 شهر",
                    DurationInMonths = 12,
                    Price = 11800, OriginalPrice = 18800,
                    LevelCompletionCount = 4,
                    IsFeatured = false, IsActive = true, DisplayOrder = 3,
                    Features = new List<PlanFeature>
                    {
                        new() { DescriptionEn = "live online session", DescriptionAr = "جلسة مباشرة أونلاين", IsIncluded = true, DisplayOrder = 1 },
                        new() { DescriptionEn = "assessment & quizzes", DescriptionAr = "تقييمات واختبارات", IsIncluded = true, DisplayOrder = 2 },
                        new() { DescriptionEn = "Compilation Certificate", DescriptionAr = "شهادة إتمام", IsIncluded = true, DisplayOrder = 3 },
                        new() { DescriptionEn = "Technical Guidance", DescriptionAr = "توجيه تقني", IsIncluded = true, DisplayOrder = 4 },
                        new() { DescriptionEn = "Limited Group", DescriptionAr = "مجموعة محدودة", IsIncluded = true, DisplayOrder = 5 },
                        new() { DescriptionEn = "graduation projects", DescriptionAr = "مشاريع تخرج", IsIncluded = true, DisplayOrder = 6 },
                        new() { DescriptionEn = "4-Level Completion", DescriptionAr = "إتمام 4 مستويات", IsIncluded = true, DisplayOrder = 7 }
                    }
                }
            };

            await _context.PricingPlans.AddRangeAsync(plans);
            await _context.SaveChangesAsync();
        }

        private async Task SeedFAQsAsync()
        {
            if (_context.FAQs.Any()) return;

            var faqs = new List<FAQ>
            {
                new() { QuestionEn = "Why is it important for kids to learn coding from an early age?", QuestionAr = "لماذا من المهم أن يتعلم الأطفال البرمجة في سن مبكرة؟", AnswerEn = "Learning coding at an early age helps children develop problem-solving skills, logical thinking, and creativity.", AnswerAr = "يساعد تعلم البرمجة في سن مبكرة الأطفال على تطوير مهارات حل المشكلات والتفكير المنطقي والإبداع.", DisplayOrder = 1, IsActive = true },
                new() { QuestionEn = "What is the best age to start learning coding?", QuestionAr = "ما هو أفضل سن لبدء تعلم البرمجة؟", AnswerEn = "At 3C, we accept students from 6 to 18 years old. The earlier, the better!", AnswerAr = "في 3C نقبل الطلاب من سن 6 إلى 18 سنة. كلما بدأ مبكرًا كان أفضل!", DisplayOrder = 2, IsActive = true },
                new() { QuestionEn = "How can a 6-year-old learn to code and understand programming?", QuestionAr = "كيف يمكن لطفل عمره 6 سنوات أن يتعلم البرمجة؟", AnswerEn = "We use visual, block-based programming tools that make coding intuitive and fun for young children.", AnswerAr = "نستخدم أدوات برمجة بصرية ومرئية تجعل البرمجة سهلة وممتعة للأطفال الصغار.", DisplayOrder = 3, IsActive = true },
                new() { QuestionEn = "What's the coding curriculum like at 3C?", QuestionAr = "ما هو المنهج الدراسي في 3C؟", AnswerEn = "Our curriculum covers +48 professional tech tools including AI, web development, mobile apps, and more.", AnswerAr = "يغطي منهجنا أكثر من 48 أداة تقنية احترافية تشمل الذكاء الاصطناعي وتطوير الويب وتطبيقات الجوال والمزيد.", DisplayOrder = 4, IsActive = true },
                new() { QuestionEn = "What do kids actually learn at 3C?", QuestionAr = "ماذا يتعلم الأطفال فعلياً في 3C؟", AnswerEn = "Kids learn real text-based programming from day one using fun, gamified methods.", AnswerAr = "يتعلم الأطفال البرمجة النصية الحقيقية من اليوم الأول باستخدام أساليب ممتعة وقائمة على الألعاب.", DisplayOrder = 5, IsActive = true },
                new() { QuestionEn = "How do you teach a language like Python to young kids?", QuestionAr = "كيف تُدرّسون لغة مثل بايثون للأطفال الصغار؟", AnswerEn = "We break down Python concepts into simple, digestible lessons designed specifically for young learners.", AnswerAr = "نقوم بتبسيط مفاهيم بايثون إلى دروس بسيطة مصممة خصيصًا للمتعلمين الصغار.", DisplayOrder = 6, IsActive = true },
                new() { QuestionEn = "Do kids need any special skills to learn coding?", QuestionAr = "هل يحتاج الأطفال إلى مهارات خاصة لتعلم البرمجة؟", AnswerEn = "No prior skills are needed. All a child needs is curiosity and a computer!", AnswerAr = "لا حاجة لمهارات مسبقة. كل ما يحتاجه الطفل هو الفضول وجهاز كمبيوتر!", DisplayOrder = 7, IsActive = true },
                new() { QuestionEn = "How can I choose the right program for my child?", QuestionAr = "كيف يمكنني اختيار البرنامج المناسب لطفلي؟", AnswerEn = "Our team will assess your child's age and interest and recommend the best program.", AnswerAr = "سيقوم فريقنا بتقييم عمر طفلك واهتماماته والتوصية بأفضل برنامج مناسب.", DisplayOrder = 8, IsActive = true },
                new() { QuestionEn = "Can I track my child's progress?", QuestionAr = "هل يمكنني متابعة تقدم طفلي؟", AnswerEn = "Yes! Parents get regular progress reports and access to our learning platform.", AnswerAr = "نعم! يحصل الآباء على تقارير تقدم منتظمة والوصول إلى منصة التعلم لدينا.", DisplayOrder = 9, IsActive = true },
                new() { QuestionEn = "How do I enroll my child?", QuestionAr = "كيف أسجل طفلي؟", AnswerEn = "Simply fill out the registration form on our website and our team will contact you within 24 hours.", AnswerAr = "ما عليك سوى ملء نموذج التسجيل على موقعنا وسيتواصل معك فريقنا خلال 24 ساعة.", DisplayOrder = 10, IsActive = true }
            };

            await _context.FAQs.AddRangeAsync(faqs);
            await _context.SaveChangesAsync();
        }

        private async Task SeedTestimonialsAsync()
        {
            if (_context.Testimonials.Any()) return;

            var testimonials = new List<Testimonial>
            {
                new() { PersonName = "Mostafa", PersonType = "Student", YoutubeVideoId = "dQw4w9WgXcQ", TitleEn = "Our students | Mostafa", TitleAr = "طلابنا | مصطفى", IsActive = true, DisplayOrder = 1 },
                new() { PersonName = "Omar", PersonType = "Student", YoutubeVideoId = "dQw4w9WgXcQ", TitleEn = "Our students | Omar", TitleAr = "طلابنا | عمر", IsActive = true, DisplayOrder = 2 },
                new() { PersonName = "Abdullah", PersonType = "Student", YoutubeVideoId = "dQw4w9WgXcQ", TitleEn = "Our students | Abdullah", TitleAr = "طلابنا | عبدالله", IsActive = true, DisplayOrder = 3 },
                new() { PersonName = "Parent 1", PersonType = "Parent", YoutubeVideoId = "dQw4w9WgXcQ", TitleEn = "Parent Experience at 3C", TitleAr = "تجربة أحد أولياء الأمور", IsActive = true, DisplayOrder = 1 },
                new() { PersonName = "Parent 2", PersonType = "Parent", YoutubeVideoId = "dQw4w9WgXcQ", TitleEn = "Why I chose 3C", TitleAr = "لماذا اخترت 3C", IsActive = true, DisplayOrder = 2 }
            };

            await _context.Testimonials.AddRangeAsync(testimonials);
            await _context.SaveChangesAsync();
        }
    }
}