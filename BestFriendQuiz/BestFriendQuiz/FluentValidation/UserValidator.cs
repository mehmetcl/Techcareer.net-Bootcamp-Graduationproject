using BestFriendQuiz.EntityLayer.Concrete;
using BestFriendQuiz.Models;
using FluentValidation;

namespace BestFriendQuiz.FluentValidation
{
    public class UserValidator : AbstractValidator<GenericViewModel>
    {
        public UserValidator()

        {
        //    RuleFor(x => x.Username)
        //      .NotEmpty().WithMessage("Kullanıcı adı boş geçilemez")
        //      .MaximumLength(30).WithMessage("Kullanıcı adı en fazla 30 karakter olmalıdır")
        //      .Must(BeValidEmail).WithMessage("Kullanıcı adı email formatında olmalıdır");

        //    RuleFor(x => x.Password)
        //        .NotEmpty().WithMessage("Şifre boş geçilemez")
        //        .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır");
        //
        }
          private bool BeValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }
    }
}
