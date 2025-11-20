using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.People.GetHobbies
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/people/{personId}/hobbies");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var personId = Route<int>("personId");
            
            // Verify person exists
            var personExists = await Db.People.AnyAsync(p => p.Id == personId, c);
            if (!personExists)
            {
                await SendNotFoundAsync(c);
                return;
            }

            var hobbies = await Db.PersonHobbies
                .Where(ph => ph.PersonId == personId)
                .Include(ph => ph.Hobby)
                .Select(ph => new HobbyDto
                {
                    Id = ph.Hobby.Id,
                    Name = ph.Hobby.Name,
                    Description = ph.Hobby.Description,
                    AssignedAt = ph.AssignedAt
                })
                .ToListAsync(c);

            await SendAsync(new Response
            {
                Hobbies = hobbies
            }, cancellation: c);
        }
    }
}
