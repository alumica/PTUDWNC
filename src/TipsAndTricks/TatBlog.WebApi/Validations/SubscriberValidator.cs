using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class SubscriberValidator : AbstractValidator<SubscriberEditModel>
    {
        public SubscriberValidator() 
        {
            //RuleFor(a => a.Email)
            //    .NotEmpty()
            //    .WithMessage("Email không được để trống")
            //    .MaximumLength(100)
            //    .WithMessage("Email tối đa 100 ký tự");

            RuleFor(a => a.ResonUnsubscribe)
                .NotEmpty()
                .WithMessage("Lý do hủy đăng ký không được để trống")
                .MaximumLength(500)
                .WithMessage("Lý do hủy đăng ký tối đa 100 ký tự");

            RuleFor(a => a.Notes)
                .MaximumLength(500)
                .WithMessage("Ghi chú tối đa 500 ký tự");

            RuleFor(a => a.TypeReason)
               .Must(x => x == false || x == true)
               .WithMessage("Phải có loại hủy đăng ký");
        }
    }
}