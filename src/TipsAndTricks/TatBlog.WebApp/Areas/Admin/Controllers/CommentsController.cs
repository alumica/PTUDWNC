using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class CommentsController : Controller
	{
        private readonly ILogger<CommentsController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public CommentsController(
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            //_logger.LogInformation("Tạo điều kiện truy vấn");
            //// Sử dụng Mapster để tạo đối tượng PostQuery
            //// từ đối tượng PostFilterModel model
            //var postQuery = _mapper.Map<PostQuery>(model);

            //_logger.LogInformation("Lấy danh sách bài viết từ CSDL");
            //ViewBag.CategoriesList = await _blogRepository
            //    .GetPagedCategoriesAsync(pageNumber, pageSize);

            //_logger.LogInformation("Chuẩn bị dữ liệu trong ViewModel");
            //await PopulatePostFilterModeAsync(model);

            var model = await _blogRepository.GetPagedCommentAsync(pageNumber, pageSize);

            return View(model);
        }
      

        public async Task<IActionResult> ToggleApproved(int id = 0)
        {
            await _blogRepository.ToggleApprovedAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteComment(int id = 0)
        {
            await _blogRepository.DeleteCommentByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
