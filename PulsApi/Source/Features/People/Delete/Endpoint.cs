using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.People.Delete
{
    sealed class Endpoint : EndpointWithoutRequest
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Delete("/api/people/{id}");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var id = Route<int>("id");
            var person = await Db.People.FindAsync(new object[] { id }, c);

            if (person == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            Db.People.Remove(person);
            await Db.SaveChangesAsync(c);

            await SendNoContentAsync(c);
        }
    }
}
