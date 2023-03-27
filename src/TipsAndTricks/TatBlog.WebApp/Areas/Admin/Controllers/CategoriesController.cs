using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class CategoriesController : Controller
	{
        private readonly ILogger<PostsController> _logger;
		private readonly IValidator<CategoryEditModel> _validator;
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;

        public CategoriesController(
			IBlogRepository blogRepository,
			IMapper mapper,
            IValidator<CategoryEditModel> validator)
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

            var model = await _blogRepository.GetPagedCategoriesAsync(pageNumber, pageSize);

            return View(model);
        }

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			// Id = 0 <=> Thêm bài viết mới
			// Id > 0: Đọc bài viết từ CSDL
			var category = id > 0
				? await _blogRepository.GetCategoryByIdAsync(id)
				: null;

			// Tạo view model từ dữ liệu bài viết
			var model = category == null
				? new CategoryEditModel()
				: _mapper.Map<CategoryEditModel>(category);

			// Gán các giá trị khác cho view model
			//await PopulateCategoryEditModeAsync(model);

			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> Edit(
			CategoryEditModel model)
		{
			var validationResult = await _validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}

			var category = model.Id > 0
				? await _blogRepository.GetCategoryByIdAsync(model.Id)
				: null;

			if (category == null)
			{
                category = _mapper.Map<Category>(model);

                category.Id = 0;
			}
			else
			{
				_mapper.Map(model, category);
			}

		
			await _blogRepository.CreateOrUpdateCategoryAsync(category);

			return RedirectToAction(nameof(Index));

		}

        [HttpPost]
        public async Task<IActionResult> VerifyCategorySlug(
            int id, string urlSlug)
        {
            var slugExisted = await _blogRepository
                .IsCategorySlugExistedAsync(id, urlSlug);

            return slugExisted
                ? Json($"Slug '{urlSlug}' đã được sử dụng")
                : Json(true);
        }


        public async Task<IActionResult> ToggleShowOnMenu(int id = 0)
		{
			await _blogRepository.ToggleShowOnMenuAsync(id);
			return RedirectToAction(nameof(Index));
		}

        public async Task<IActionResult> DeleteCategory(int id = 0)
        {
            await _blogRepository.DeleteCategoryByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
