using FluentValidation;
using TatBlog.Services.Authors;
using TatBlog.Services.Blogs;
using TatBlog.Services.Subscribers;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
	public class SubscriberValidator : AbstractValidator<SubscriberEditModel>
	{
		private readonly ISubscriberRepository _subscriberRepository;

		public SubscriberValidator(ISubscriberRepository subscriberRepository)
		{
			_subscriberRepository = subscriberRepository;

			RuleFor(x => x.TypeReason)
				.NotEmpty()
				.WithMessage("Phải chọn trạng thái đăng ký");

			RuleFor(x => x.ResonUnsubscribe)
				.NotEmpty()
				.WithMessage("Lý do hủy đăng ký không được để trống")
				.MaximumLength(500)
				.WithMessage("Tên định danh tối đa 1000 ký tự");



			RuleFor(x => x.Notes)
				.NotEmpty()
				.WithMessage("Ghi chú không được để trống")
				.MaximumLength(3000)
				.WithMessage("Nội dung tối đa 3000 ký tự");
		}
	}
}
