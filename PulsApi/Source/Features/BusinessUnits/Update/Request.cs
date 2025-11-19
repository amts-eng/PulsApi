using FluentValidation;

namespace PulsApi.BusinessUnits.Update
{
    sealed class Request
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrganizationId { get; set; }

        internal sealed class Validator : Validator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MinimumLength(3)
                    .MaximumLength(100);
                
                RuleFor(x => x.Description)
                    .MaximumLength(500);
                
                RuleFor(x => x.OrganizationId)
                    .GreaterThan(0)
                    .WithMessage("OrganizationId is required");
            }
        }
    }
}
