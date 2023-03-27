using FluentValidation;
using TatBlog.Services.Authors;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
	public class AuthorValidator : AbstractValidator<AuthorEditModel>
	{
		private readonly IAuthorRepository _authorRepository;

		public AuthorValidator(IAuthorRepository authorRepository)
		{
			_authorRepository = authorRepository;

			RuleFor(x => x.FullName)
				.NotEmpty()
				.WithMessage("Họ tên không được để trống")
				.MaximumLength(150)
				.WithMessage("Tiêu đề tối đa 150 ký tự");

			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.WithMessage("Tên định danh không được để trống")
				.MaximumLength(1000)
				.WithMessage("Tên định danh tối đa 1000 ký tự");

			RuleFor(x => x.UrlSlug)
				.MustAsync(async (authorModel, slug, cancellationToken) =>
				!await _authorRepository.IsAuthorSlugExistedAsync(
					authorModel.Id, slug, cancellationToken))
				.WithMessage("Slug '{PropertyValue}' đã được sử dụng");


			RuleFor(x => x.Notes)
				.NotEmpty()
				.WithMessage("Ghi chú không được để trống")
				.MaximumLength(3000)
				.WithMessage("Nội dung tối đa 3000 ký tự");

			RuleFor(x => x.Email)
				.NotEmpty()
				.WithMessage("Email không được để trống")
				.MaximumLength(200)
				.WithMessage("Email tối đa 200 ký tự");

			When(x => x.Id <= 0, () =>
			{
				RuleFor(x => x.ImageFile)
					.Must(x => x is { Length: > 0 })
					.WithMessage("Bạn phải chọn hình ảnh cho tác giả");
			})
			.Otherwise(() =>
			{
				RuleFor(x => x.ImageFile)
					.MustAsync(SetImageIfNotExist)
					.WithMessage("Bạn phải chọn hình ảnh cho tác giả");
			});
		}

		// Kiểm tra xem người dùng đã nhập ít nhất 1 thẻ (tag)
		//private bool HasAtLeastOneTag(
		//	AuthorEditModel auhtorModel, string selectedTags)
		//{
		//	return auhtorModel.GetSelectedTags().Any();
		//}

		// Kiểm tra xem bài viết đã có hình ảnh chưa.
		// Nếu chưa có, bắt buộc người dùng phải chọn file.
		private async Task<bool> SetImageIfNotExist(
			AuthorEditModel auhtorModel,
			IFormFile imageFile,
			CancellationToken cancellationToken)
		{
			// Lấy thông tin bài viết từ CSDL
			var author = await _authorRepository.GetAuthorByIdAsync(auhtorModel.Id, cancellationToken);

			// Nếu bài viết đã có hình ảnh => Không bắt buộc chọn file
			if (!string.IsNullOrWhiteSpace(author?.ImageUrl))
				return true;

			// Ngược lại (bài viết chưa có hình ảnh), kiểm tra xem
			// người dùng đã chọn file hay chưa. Nếu chưa thì báo lỗi
			return imageFile is { Length: > 0 };
		}
	}
}
