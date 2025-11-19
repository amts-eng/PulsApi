using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.BusinessUnits.Delete
{
    sealed class Endpoint : EndpointWithoutRequest
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Delete("/api/businessunits/{id}");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var id = Route<int>("id");
            var businessUnit = await Db.BusinessUnits.FindAsync(new object[] { id }, c);

            if (businessUnit == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            Db.BusinessUnits.Remove(businessUnit);
            await Db.SaveChangesAsync(c);

            await SendNoContentAsync(c);
        }
    }
}
