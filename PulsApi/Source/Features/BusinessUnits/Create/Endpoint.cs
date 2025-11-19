using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.BusinessUnits.Create
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/businessunits");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            // Verify organization exists
            var organization = await Db.Organizations.FindAsync(new object[] { r.OrganizationId }, c);
            if (organization == null)
            {
                AddError("Organization not found");
                await SendErrorsAsync(404, c);
                return;
            }

            var businessUnit = new Models.BusinessUnit
            {
                Name = r.Name,
                Description = r.Description,
                OrganizationId = r.OrganizationId
            };

            Db.BusinessUnits.Add(businessUnit);
            await Db.SaveChangesAsync(c);

            await SendAsync(new Response
            {
                Id = businessUnit.Id,
                Name = businessUnit.Name,
                Description = businessUnit.Description,
                OrganizationId = businessUnit.OrganizationId,
                OrganizationName = organization.Name
            }, 201, c);
        }
    }
}
