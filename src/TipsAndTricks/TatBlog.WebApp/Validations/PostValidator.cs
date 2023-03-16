using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
    public class PostValidator : AbstractValidator<PostEditModel>
    {
        private readonly IBlogRepository _blogRepository;

        public PostValidator(IBlogRepository blogRepository) 
        {
            _blogRepository = blogRepository;

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Tiêu đề không được để trống")
                .MaximumLength(500)
                .WithMessage("Tiêu đề tối đa 500 ký tự");

			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.WithMessage("Tên định danh không được để trống")
				.MaximumLength(1000)
				.WithMessage("Tên định danh tối đa 1000 ký tự");

			RuleFor(x => x.UrlSlug)
				.MustAsync(async (postModel, slug, cancellationToken) =>
				!await blogRepository.IsPostSlugExistedAsync(
					postModel.Id, slug, cancellationToken))
				.WithMessage("Slug '{PropertyValue}' đã được sử dụng");

			RuleFor(x => x.ShortDescription)
                .NotEmpty()
                .WithMessage("Giới thiệu không được để trống")
                .MaximumLength(2000)
                .WithMessage("Giới thiệu tối đa 2000 ký tự");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Nội dung không được để trống")
                .MaximumLength(5000)
                .WithMessage("Nội dung tối đa 5000 ký tự");

            RuleFor(x => x.Meta)
                .NotEmpty()
                .WithMessage("Metadata không được để trống")
                .MaximumLength(1000)
                .WithMessage("Metadata tối đa 1000 ký tự");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn chủ đề cho bài viết");

            RuleFor(x => x.AuthorId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn tác giả cho bài viết");

            RuleFor(x => x.SelectedTags)
                .Must(HasAtLeastOneTag)
                .WithMessage("Bạn phải nhập ít nhất một thẻ");

            When(x => x.Id <= 0, () =>
            {
                RuleFor(x => x.ImageFile)
                    .Must(x => x is { Length: > 0 })
                    .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
            })
            .Otherwise(() =>
            {
                RuleFor(x => x.ImageFile)
                    .MustAsync(SetImageIfNotExist)
                    .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
            });
        }

        // Kiểm tra xem người dùng đã nhập ít nhất 1 thẻ (tag)
        private bool HasAtLeastOneTag(
            PostEditModel postModel, string selectedTags)
        {
            return postModel.GetSelectedTags().Any();
        }

        // Kiểm tra xem bài viết đã có hình ảnh chưa.
        // Nếu chưa có, bắt buộc người dùng phải chọn file.
        private async Task<bool> SetImageIfNotExist(
            PostEditModel postModel,
            IFormFile imageFile,
            CancellationToken cancellationToken)
        { 
            // Lấy thông tin bài viết từ CSDL
            var post = await _blogRepository.GetPostByIdAsync(postModel.Id, false, cancellationToken);

            // Nếu bài viết đã có hình ảnh => Không bắt buộc chọn file
            if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
                return true;

            // Ngược lại (bài viết chưa có hình ảnh), kiểm tra xem
            // người dùng đã chọn file hay chưa. Nếu chưa thì báo lỗi
            return imageFile is { Length: > 0 };
        }
    }
}
