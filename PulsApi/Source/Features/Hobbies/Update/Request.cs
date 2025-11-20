using FluentValidation;

namespace PulsApi.Hobbies.Update
{
    sealed class Request
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        internal sealed class Validator : Validator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(100);
                
                RuleFor(x => x.Description)
                    .MaximumLength(500);
            }
        }
    }
}
