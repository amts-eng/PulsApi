using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Teams.GetById
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/teams/{id}");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var id = Route<int>("id");
            var team = await Db.Teams
                .Include(t => t.BusinessUnit)
                    .ThenInclude(bu => bu.Organization)
                .FirstOrDefaultAsync(t => t.Id == id, c);

            if (team == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            await SendAsync(new Response
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                BusinessUnitId = team.BusinessUnitId,
                BusinessUnitName = team.BusinessUnit.Name,
                OrganizationId = team.BusinessUnit.OrganizationId,
                OrganizationName = team.BusinessUnit.Organization.Name
            }, cancellation: c);
        }
    }
}
