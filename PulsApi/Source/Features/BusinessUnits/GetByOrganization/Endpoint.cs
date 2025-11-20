using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.BusinessUnits.GetByOrganization
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/organizations/{organizationId}/businessunits");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var organizationId = Route<int>("organizationId");
            
            // Verify organization exists
            var organizationExists = await Db.Organizations.AnyAsync(o => o.Id == organizationId, c);
            if (!organizationExists)
            {
                await SendNotFoundAsync(c);
                return;
            }

            var businessUnits = await Db.BusinessUnits
                .Include(bu => bu.Organization)
                .Where(bu => bu.OrganizationId == organizationId)
                .ToListAsync(c);

            await SendAsync(new Response
            {
                BusinessUnits = businessUnits.Select(bu => new BusinessUnitDto
                {
                    Id = bu.Id,
                    Name = bu.Name,
                    Description = bu.Description,
                    OrganizationId = bu.OrganizationId,
                    OrganizationName = bu.Organization.Name
                }).ToList()
            }, cancellation: c);
        }
    }
}
