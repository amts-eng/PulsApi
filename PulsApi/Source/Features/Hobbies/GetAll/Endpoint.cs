using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Hobbies.GetAll
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/hobbies");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var hobbies = await Db.Hobbies.ToListAsync(c);

            await SendAsync(new Response
            {
                Hobbies = hobbies.Select(h => new HobbyDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description
                }).ToList()
            }, cancellation: c);
        }
    }
}
