using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class CategoryValidator : AbstractValidator<CategoryEditModel>
    {
        public CategoryValidator() 
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Tên tác giả không được để trống")
                .MaximumLength(100)
                .WithMessage("Tên tác giả tối đa 100 ký tự");

            RuleFor(a => a.UrlSlug)
                .NotEmpty()
                .WithMessage("UrlSlug không được để trống")
                .MaximumLength(100)
                .WithMessage("UrlSlug tối đa 100 ký tự");

            RuleFor(a => a.Description)
                .MaximumLength(500)
                .WithMessage("Ghi chú tối đa 500 ký tự");

            RuleFor(a => a.ShowOnMenu)
               .Must(x => x == false || x == true)
               .WithMessage("Hiển thị trên menu phải là 'Có/Không'");
        }
    }
}