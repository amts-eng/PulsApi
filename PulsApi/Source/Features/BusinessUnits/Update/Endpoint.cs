using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.BusinessUnits.Update
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Put("/api/businessunits/{id}");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
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

            // Verify new organization exists if changed
            if (businessUnit.OrganizationId != r.OrganizationId)
            {
                var organization = await Db.Organizations.FindAsync(new object[] { r.OrganizationId }, c);
                if (organization == null)
                {
                    AddError("Organization not found");
                    await SendErrorsAsync(404, c);
                    return;
                }
            }

            businessUnit.Name = r.Name;
            businessUnit.Description = r.Description;
            businessUnit.OrganizationId = r.OrganizationId;
            await Db.SaveChangesAsync(c);

            // Reload organization if changed
            if (businessUnit.Organization.Id != r.OrganizationId)
            {
                await Db.Entry(businessUnit).Reference(bu => bu.Organization).LoadAsync(c);
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
