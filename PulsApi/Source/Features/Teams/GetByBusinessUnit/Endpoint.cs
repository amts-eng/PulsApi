using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Teams.GetByBusinessUnit
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/businessunits/{businessUnitId}/teams");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var businessUnitId = Route<int>("businessUnitId");
            
            // Verify business unit exists
            var businessUnitExists = await Db.BusinessUnits.AnyAsync(bu => bu.Id == businessUnitId, c);
            if (!businessUnitExists)
            {
                await SendNotFoundAsync(c);
                return;
            }

            var teams = await Db.Teams
                .Include(t => t.BusinessUnit)
                    .ThenInclude(bu => bu.Organization)
                .Where(t => t.BusinessUnitId == businessUnitId)
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
