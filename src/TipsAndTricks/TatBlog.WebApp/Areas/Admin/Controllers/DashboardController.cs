using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Authors;
using TatBlog.Services.Blogs;
using TatBlog.Services.Subscribers;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class DashboardController : Controller
	{
		private readonly IBlogRepository _blogRepository;
		private readonly ISubscriberRepository _subscriberRepository;
		private readonly IAuthorRepository _authorRepository;

        public DashboardController(
			IBlogRepository blogRepository,
			ISubscriberRepository subscriberRepository,
			IAuthorRepository authorRepository)
		{
			_blogRepository = blogRepository;
            _subscriberRepository = subscriberRepository;
            _authorRepository = authorRepository;
        }

		public async Task<IActionResult> Index()
		{
			ViewBag.TotalPosts = await _blogRepository.GetTotalPostsAsync();
			ViewBag.NumberPostsUnpublished = await _blogRepository.NumberPostsUnpublishedAsync();
			ViewBag.NumberCategories = await _blogRepository.NumberCategoriesAsync();
			ViewBag.NumberAuthors = await _authorRepository.NumberAuthorsAsync();
			ViewBag.NumberCommentsUnapproved = await _blogRepository.NumberCommentsUnApprovedAsync();
			ViewBag.NumberSubscribers = await _subscriberRepository.NumberSubscribersAsync();
			ViewBag.NumberSubscribersToday = await _subscriberRepository.NumberSubscribersTodayAsync();
            return View();
		}
	}
}
