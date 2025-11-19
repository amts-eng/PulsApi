using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.BusinessUnits.GetAll
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/businessunits");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var businessUnits = await Db.BusinessUnits
                .Include(bu => bu.Organization)
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
