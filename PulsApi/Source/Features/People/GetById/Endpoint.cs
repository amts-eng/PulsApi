using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.People.GetById
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/people/{id}");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var id = Route<int>("id");
            var person = await Db.People
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.Id == id, c);

            if (person == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            await SendAsync(new Response
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                TeamId = person.TeamId,
                TeamName = person.Team?.Name
            }, cancellation: c);
        }
    }
}
