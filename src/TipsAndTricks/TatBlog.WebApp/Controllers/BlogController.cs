using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name ="k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 2,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            // Tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // Chỉ lấy những bài viết có trạng thái Pushlished
                PublishedOnly = true,

                // Tìm bài viết theo từ khóa
                Keyword = keyword
            };

            var postList = await _blogRepository
                .GetPagedPostQueryAsync(postQuery, pageNumber, pageSize);

            ViewBag.PostQuery = postQuery;

            return View(postList);
        }

        public IActionResult About()
            => View();

        public IActionResult Contact()
            => View();

        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhật");
    }
}
