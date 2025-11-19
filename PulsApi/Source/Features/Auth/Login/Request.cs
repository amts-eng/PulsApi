using FluentValidation;

namespace PulsApi.Auth.Login
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
                    .EmailAddress();
                
                RuleFor(x => x.Password)
                    .NotEmpty();
            }
        }
    }
}
