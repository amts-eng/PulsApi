using FluentValidation;

namespace PulsApi.Teams.Update
{
    sealed class Request
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int BusinessUnitId { get; set; }

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
                
                RuleFor(x => x.BusinessUnitId)
                    .GreaterThan(0)
                    .WithMessage("BusinessUnitId is required");
            }
        }
    }
}
