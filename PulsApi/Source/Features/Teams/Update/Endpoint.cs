using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Teams.Update
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Put("/api/teams/{id}");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
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

            // Verify new business unit exists if changed
            if (team.BusinessUnitId != r.BusinessUnitId)
            {
                var businessUnit = await Db.BusinessUnits
                    .Include(bu => bu.Organization)
                    .FirstOrDefaultAsync(bu => bu.Id == r.BusinessUnitId, c);
                    
                if (businessUnit == null)
                {
                    AddError("BusinessUnit not found");
                    await SendErrorsAsync(404, c);
                    return;
                }
            }

            team.Name = r.Name;
            team.Description = r.Description;
            team.BusinessUnitId = r.BusinessUnitId;
            await Db.SaveChangesAsync(c);

            // Reload business unit and organization if changed
            if (team.BusinessUnit.Id != r.BusinessUnitId)
            {
                await Db.Entry(team)
                    .Reference(t => t.BusinessUnit)
                    .Query()
                    .Include(bu => bu.Organization)
                    .LoadAsync(c);
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
