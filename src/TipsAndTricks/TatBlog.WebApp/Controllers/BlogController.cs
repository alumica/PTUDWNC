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
            [FromQuery(Name = "p")] int pageNumber = 1,
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

        public async Task<IActionResult> Category(
            string slug = null,
            string keyword = null,
            int pageNumber = 1,
            int pageSize = 5)
        {
            // Tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                PublishedOnly= true,
                Keyword = keyword,
                CategorySlug = slug
            };

            var postList = await _blogRepository
                .GetPagedPostQueryAsync(postQuery, pageNumber, pageSize);

            ViewBag.PostQuery = postQuery;

            return View(postList);
        }

        public async Task<IActionResult> Author(
            string slug = null,
            string keyword = null,
            int pageNumber = 1,
            int pageSize = 5)
        {
            // Tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                Keyword = keyword,
                AuthorSlug = slug
            };

            var postList = await _blogRepository
                .GetPagedPostQueryAsync(postQuery, pageNumber, pageSize);

            ViewBag.PostQuery = postQuery;

            return View(postList);
        }

        public async Task<IActionResult> Tag(
            string slug = null,
            string keyword = null,
            int pageNumber = 1,
            int pageSize = 5)
        {
            // Tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                Keyword = keyword,
                TagSlug = slug
            };

            var postList = await _blogRepository
                .GetPagedPostQueryAsync(postQuery, pageNumber, pageSize);

            ViewBag.PostQuery = postQuery;

            return View(postList);
        }

        public async Task<IActionResult> Post(
            int year, int month, string slug = null)
        {
            // Tạo đối tượng chứa các điều kiện truy vấn

            var post = await _blogRepository
                .GetPostAsync(year, month, slug);
            await _blogRepository.IncreaseViewCountAsync(post.Id);

            //ViewBag.PostQuery = postQuery;

            return View(post);
        }

        public async Task<IActionResult> Archives(
            int year, int month,
            string keyword = null,
            int pageNumber = 1,
            int pageSize = 5)
        {
            // Tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                Keyword = keyword,
                PostedYear = year,
                PostedMonth = month,
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
