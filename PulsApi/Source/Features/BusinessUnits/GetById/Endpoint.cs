using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.BusinessUnits.GetById
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/businessunits/{id}");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var id = Route<int>("id");
            var businessUnit = await Db.BusinessUnits
                .Include(bu => bu.Organization)
                .FirstOrDefaultAsync(bu => bu.Id == id, c);

            if (businessUnit == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            await SendAsync(new Response
            {
                Id = businessUnit.Id,
                Name = businessUnit.Name,
                Description = businessUnit.Description,
                OrganizationId = businessUnit.OrganizationId,
                OrganizationName = businessUnit.Organization.Name
            }, cancellation: c);
        }
    }
}
