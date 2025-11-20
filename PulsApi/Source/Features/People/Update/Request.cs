using FluentValidation;

namespace PulsApi.People.Update
{
    sealed class Request
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        internal sealed class Validator : Validator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(100);
                
                RuleFor(x => x.LastName)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(100);
                
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(256);
            }
        }
    }
}
