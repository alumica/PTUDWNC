using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class CommentValidator : AbstractValidator<CommentEditModel>
    {
        public CommentValidator() 
        {
            RuleFor(a => a.FullName)
                .NotEmpty()
                .WithMessage("Họ tên không được để trống")
                .MaximumLength(100)
                .WithMessage("Họ tên giả tối đa 100 ký tự");

            RuleFor(a => a.Gender)
                .Must(x => x == false || x == true);

            RuleFor(a => a.Approved)
                .Must(x => x == false || x == true)
                .WithMessage("Trạng thái duyệt bình luận phải là 'Có/Không'");

            RuleFor(a => a.Description)
                .MaximumLength(500)
                .WithMessage("Ghi chú tối đa 500 ký tự");
        }
    }
}