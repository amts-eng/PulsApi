using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Organizations.Delete
{
    sealed class Endpoint : EndpointWithoutRequest
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Delete("/api/organizations/{id}");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var id = Route<int>("id");
            var organization = await Db.Organizations.FindAsync(new object[] { id }, c);

            if (organization == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            Db.Organizations.Remove(organization);
            await Db.SaveChangesAsync(c);

            await SendNoContentAsync(c);
        }
    }
}
