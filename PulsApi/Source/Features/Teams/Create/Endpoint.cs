using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Teams.Create
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/teams");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            // Verify business unit exists and load organization
            var businessUnit = await Db.BusinessUnits
                .Include(bu => bu.Organization)
                .FirstOrDefaultAsync(bu => bu.Id == r.BusinessUnitId, c);
                
            if (businessUnit == null)
            {
                AddError("BusinessUnit not found");
                await SendErrorsAsync(404, c);
                return;
            }

            var team = new Models.Team
            {
                Name = r.Name,
                Description = r.Description,
                BusinessUnitId = r.BusinessUnitId
            };

            Db.Teams.Add(team);
            await Db.SaveChangesAsync(c);

            await SendAsync(new Response
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                BusinessUnitId = team.BusinessUnitId,
                BusinessUnitName = businessUnit.Name,
                OrganizationId = businessUnit.OrganizationId,
                OrganizationName = businessUnit.Organization.Name
            }, 201, c);
        }
    }
}
