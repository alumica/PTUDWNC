using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Pkcs;
using TatBlog.Core.Entities;
using TatBlog.Services.Authors;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.Services.Subscribers;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class SubscribersController : Controller
	{
		private readonly ILogger<SubscribersController> _logger;
		private readonly IBlogRepository _blogRepository;
		private readonly IAuthorRepository _authorRepository;
		private readonly ISubscriberRepository _subscriberRepository;
		private readonly IMapper _mapper;
		private readonly IMediaManager _mediaManager;
		private readonly IValidator<SubscriberEditModel> _validator;

		public SubscribersController(
			ILogger<SubscribersController> logger,
			IBlogRepository blogRepository,
			IAuthorRepository authorRepository,
			ISubscriberRepository subscriberRepository,
			IMapper mapper, IMediaManager mediaManager,
			IValidator<SubscriberEditModel> validator)
		{
			_logger = logger;
			_blogRepository = blogRepository;
			_authorRepository = authorRepository;
			_mapper = mapper;
			_mediaManager = mediaManager;
			_validator = validator;
			_subscriberRepository = subscriberRepository;
		}

		public async Task<IActionResult> Index(
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 5)
		{

			var model = await _subscriberRepository.GetPagedSubscribersAsync(pageNumber, pageSize);

			return View(model);
		}

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            // Id = 0 <=> Thêm bài viết mới
            // Id > 0: Đọc bài viết từ CSDL
            var subscriber = id > 0
                ? await _subscriberRepository.GetSubscriberByIdAsync(id)
                : null;

            // Tạo view model từ dữ liệu bài viết
            var model = subscriber == null
                ? new SubscriberEditModel()
                : _mapper.Map<SubscriberEditModel>(subscriber);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(
            SubscriberEditModel model)
        {
            var validationResult = await _validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }

            var subscriber = model.Id > 0
                ? await _subscriberRepository.GetSubscriberByIdAsync(model.Id)
                : null;

                _mapper.Map(model, subscriber);
			subscriber.UnsubscribeDate = DateTime.Now;

			var type = subscriber.TypeReason == null || subscriber.TypeReason == true
				? true 
				: false;

            await _subscriberRepository.ToggleBlockSubscriberAsync(subscriber, type);

            return RedirectToAction(nameof(Index));

        }

    }
}
