using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class ArchivesWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public ArchivesWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy danh sách chủ đề
            var archives = await _blogRepository.CountPostsNMonthAsync(12);

            return View(archives);
        }
    }
}
