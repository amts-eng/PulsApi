using FluentValidation;

namespace PulsApi.Auth.Register
{
    sealed class Request
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        internal sealed class Validator : Validator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(256);
                
                RuleFor(x => x.Password)
                    .NotEmpty()
                    .MinimumLength(8)
                    .MaximumLength(100)
                    .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                    .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                    .Matches(@"[0-9]").WithMessage("Password must contain at least one number");
            }
        }
    }
}
