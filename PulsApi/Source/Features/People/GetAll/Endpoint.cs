using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.People.GetAll
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/people");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var people = await Db.People
                .Include(p => p.Team)
                .ToListAsync(c);

            await SendAsync(new Response
            {
                People = people.Select(p => new PersonDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    TeamId = p.TeamId,
                    TeamName = p.Team?.Name
                }).ToList()
            }, cancellation: c);
        }
    }
}
