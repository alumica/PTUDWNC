using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class ContactValidator : AbstractValidator<ContactEditModel>
    {
        public ContactValidator() 
        {
            RuleFor(a => a.FullName)
                .NotEmpty()
                .WithMessage("Tên tác giả không được để trống")
                .MaximumLength(100)
                .WithMessage("Tên tác giả tối đa 100 ký tự");

            RuleFor(a => a.Email)
                .NotEmpty()
                .WithMessage("Email không được để trống")
                .MaximumLength(100)
                .WithMessage("Email tối đa 100 ký tự");

            RuleFor(a => a.Subject)
                .MaximumLength(500)
                .WithMessage("Ghi chú tối đa 500 ký tự");

            RuleFor(a => a.Description)
                .MaximumLength(1000)
                .WithMessage("Nội dung tối đa 500 ký tự");
        }
    }
}