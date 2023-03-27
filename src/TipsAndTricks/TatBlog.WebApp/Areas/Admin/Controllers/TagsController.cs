using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class TagsController : Controller
	{
		private readonly ILogger<TagEditModel> _logger;
		private readonly IValidator<TagEditModel> _validator;
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;

		public TagsController(
			IBlogRepository blogRepository,
			IMapper mapper,
			IValidator<TagEditModel> validator)
		{
			_blogRepository = blogRepository;
			_mapper = mapper;
			_validator = validator;
		}

		public async Task<IActionResult> Index(
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 5)
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

			var model = await _blogRepository.GetPagedTagsAsync(pageNumber, pageSize);

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			// Id = 0 <=> Thêm bài viết mới
			// Id > 0: Đọc bài viết từ CSDL
			var tag = id > 0
				? await _blogRepository.GetTagByIdAsync(id)
				: null;

			// Tạo view model từ dữ liệu bài viết
			var model = tag == null
				? new TagEditModel()
				: _mapper.Map<TagEditModel>(tag);

			// Gán các giá trị khác cho view model
			//await PopulateCategoryEditModeAsync(model);

			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> Edit(
			[FromServices] IValidator<TagEditModel> validator,
			TagEditModel model)
		{
			var validationResult = await validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState );
			}

			var tag = model.Id > 0
				? await _blogRepository.GetTagByIdAsync(model.Id)
				: null;

			if (tag == null)
			{
				tag = _mapper.Map<Tag>(model);

				tag.Id = 0;
			}
			else
			{
				_mapper.Map(model, tag);
			}


			await _blogRepository.CreateOrUpdateTagAsync(tag);

			return RedirectToAction(nameof(Index));

		}

		[HttpPost]
		public async Task<IActionResult> VerifyTagSlug(
			int id, string urlSlug)
		{
			var slugExisted = await _blogRepository
				.IsTagSlugExistedAsync(id, urlSlug);

			return slugExisted
				? Json($"Slug '{urlSlug}' đã được sử dụng")
				: Json(true);
		}

		public async Task<IActionResult> DeleteTag(int id = 0)
		{
			await _blogRepository.DeleteTagByIdAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
