using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Printing;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Authors;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class PostsController : Controller
	{
		private readonly ILogger<PostsController> _logger;
		private readonly IBlogRepository _blogRepository;
		private readonly IAuthorRepository _authorRepository;
		private readonly IMapper _mapper;
		private readonly IMediaManager _mediaManager;
		private readonly IValidator<PostEditModel> _validator;


		public PostsController(
			ILogger<PostsController> logger,
			IBlogRepository blogRepository,
			IAuthorRepository authorRepository,
			IMapper mapper,
			IMediaManager mediaManager,
			IValidator<PostEditModel> validator)
		{
            _logger = logger;
            _blogRepository = blogRepository;
			_authorRepository = authorRepository;
			_mapper = mapper;
			_mediaManager = mediaManager;
			_validator = validator;

		}

		public async Task<IActionResult> Index(
            PostFilterModel model,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
		{
			_logger.LogInformation("Tạo điều kiện truy vấn");
			// Sử dụng Mapster để tạo đối tượng PostQuery
			// từ đối tượng PostFilterModel model
			var postQuery = _mapper.Map<PostQuery>(model);

			_logger.LogInformation("Lấy danh sách bài viết từ CSDL");
			ViewBag.PostsList = await _blogRepository
				.GetPagedPostQueryAsync(postQuery, pageNumber, pageSize);

			_logger.LogInformation("Chuẩn bị dữ liệu trong ViewModel");
			await PopulatePostFilterModeAsync(model);

			return View(model);
		}

        public async Task<IActionResult> TogglePublished(int id)
        {
            await _blogRepository.SwitchPublisedAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeletePost(int id)
		{
			await _blogRepository.DeletePostByIdAsync(id);
			return RedirectToAction(nameof(Index));
		}


        [HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			// Id = 0 <=> Thêm bài viết mới
			// Id > 0: Đọc bài viết từ CSDL
			var post = id > 0
				? await _blogRepository.GetPostByIdAsync(id, true)
				: null;

			// Tạo view model từ dữ liệu bài viết
			var model = post == null
				? new PostEditModel()
				: _mapper.Map<PostEditModel>(post);

			// Gán các giá trị khác cho view model
			await PopulatePostEditModeAsync(model);

			return View(model);
		}

		
		[HttpPost]
		public async Task<IActionResult> Edit(
			PostEditModel model)
		{
			var validationResult = await _validator.ValidateAsync(model);
			
			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}
			
			if (!ModelState.IsValid)
			{
				await PopulatePostEditModeAsync(model);
				return View(model);
			}

			var post = model.Id > 0
				? await _blogRepository.GetPostByIdAsync(model.Id)
				: null;

			if (post == null)
			{
				post = _mapper.Map<Post>(model);

				post.Id = 0;
				post.PostedDate = DateTime.Now;
			}
			else
			{
				_mapper.Map(model, post);
				post.Category = null;
				post.ModifiedDate = DateTime.Now;
			}

			// Nếu người dùng có upload hình ảnh minh họa cho bài viết
			if (model.ImageFile?.Length > 0)
			{
				// Thì thực hiện lưu tập tin vào thư mục uploads
				var newImagePath = await _mediaManager.SaveFileAsync(
					model.ImageFile.OpenReadStream(),
					model.ImageFile.FileName,
					model.ImageFile.ContentType);

				// Nếu lưu thành công, xóa tập tin hình ảnh cũ (nếu có)
				if (!string.IsNullOrWhiteSpace(newImagePath))
				{
					await _mediaManager.DeleteFileAsync(post.ImageUrl);
					post.ImageUrl = newImagePath;
				}
			}

			await _blogRepository.CreateOrUpdatePostAsync(
				post,
				model.GetSelectedTags());

			return RedirectToAction(nameof(Index));

		}

		[HttpPost]
		public async Task<IActionResult> VerifyPostSlug(
			int id, string urlSlug)
		{
			var slugExisted = await _blogRepository
				.IsPostSlugExistedAsync(id, urlSlug);

			return slugExisted
				? Json($"Slug '{urlSlug}' đã được sử dụng")
				: Json(true);
		}

		[HttpPost]
		public async Task<IActionResult> DeletePost(
			int id,
            PostFilterModel model,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5
            )
        {
            var post = id > 0
                ? await _blogRepository.DeletePostByIdAsync(id)
                : false;

            return RedirectToAction("Index", "Posts", new { area = "Admin", model = model, pageNumber = pageNumber, pageSize = pageSize});
        }

        private async Task PopulatePostEditModeAsync(PostEditModel model)
        {
			var authors = await _authorRepository.GetAuthorsAsync();

			var categories = await _blogRepository.GetCategoriesAsync();

			model.AuthorList = authors.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.Id.ToString()
			});

			model.CategoryList = categories.Select(c => new SelectListItem()
			{
				Text = c.Name,
				Value = c.Id.ToString()
			});
		}

        private async Task PopulatePostFilterModeAsync(PostFilterModel model)
		{
			var authors = await _authorRepository.GetAuthorsAsync();

			var categories = await _blogRepository.GetCategoriesAsync();

			model.AuthorList = authors.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.Id.ToString()
			});

			model.CategoryList = categories.Select(c => new SelectListItem()
			{
				Text = c.Name,
				Value = c.Id.ToString()
			});
		}
	}
}
