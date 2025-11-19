using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace Organizations.Update
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Put("/api/organizations/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var id = Route<int>("id");
            var organization = await Db.Organizations.FindAsync(new object[] { id }, c);

            if (organization == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            organization.Name = r.Name;
            organization.Description = r.Description;
            await Db.SaveChangesAsync(c);

            await SendAsync(new Response
            {
                Id = organization.Id,
                Name = organization.Name,
                Description = organization.Description
            }, cancellation: c);
        }
    }
}
