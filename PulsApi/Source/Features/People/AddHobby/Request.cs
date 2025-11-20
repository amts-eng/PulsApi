using FluentValidation;

namespace PulsApi.People.AddHobby
{
    sealed class Request
    {
        public int HobbyId { get; set; }

        internal sealed class Validator : Validator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.HobbyId)
                    .GreaterThan(0)
                    .WithMessage("HobbyId is required");
            }
        }
    }
}
