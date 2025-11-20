using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Teams.Delete
{
    sealed class Endpoint : EndpointWithoutRequest
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Delete("/api/teams/{id}");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var id = Route<int>("id");
            var team = await Db.Teams.FindAsync(new object[] { id }, c);

            if (team == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            Db.Teams.Remove(team);
            await Db.SaveChangesAsync(c);

            await SendNoContentAsync(c);
        }
    }
}
