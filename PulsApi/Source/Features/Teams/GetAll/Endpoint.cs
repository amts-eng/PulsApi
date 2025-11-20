using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Teams.GetAll
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/teams");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var teams = await Db.Teams
                .Include(t => t.BusinessUnit)
                    .ThenInclude(bu => bu.Organization)
                .ToListAsync(c);

            await SendAsync(new Response
            {
                Teams = teams.Select(t => new TeamDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    BusinessUnitId = t.BusinessUnitId,
                    BusinessUnitName = t.BusinessUnit.Name,
                    OrganizationId = t.BusinessUnit.OrganizationId,
                    OrganizationName = t.BusinessUnit.Organization.Name
                }).ToList()
            }, cancellation: c);
        }
    }
}
