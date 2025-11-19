using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace Organizations.GetAll
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/organizations");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var organizations = await Db.Organizations.ToListAsync(c);

            await SendAsync(new Response
            {
                Organizations = organizations.Select(o => new OrganizationDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description
                }).ToList()
            }, cancellation: c);
        }
    }
}
