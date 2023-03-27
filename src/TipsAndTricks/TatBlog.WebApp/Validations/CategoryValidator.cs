using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
    public class CategoryValidator : AbstractValidator<CategoryEditModel>
    {
        private readonly IBlogRepository _blogRepository;

        public CategoryValidator(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tiêu đề không được để trống")
                .MaximumLength(100)
                .WithMessage("Tiêu đề tối đa 100 ký tự");

            RuleFor(x => x.UrlSlug)
                .NotEmpty()
                .WithMessage("Tên định danh không được để trống")
                .MaximumLength(1000)
                .WithMessage("Tên định danh tối đa 1000 ký tự");

            RuleFor(x => x.UrlSlug)
                .MustAsync(async (categoryModel, slug, cancellationToken) =>
                !await _blogRepository.IsCategorySlugExistedAsync(
                    categoryModel.Id, slug, cancellationToken))
                .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Nội dung không được để trống")
                .MaximumLength(5000)
                .WithMessage("Nội dung tối đa 5000 ký tự");
        }
    }
}
