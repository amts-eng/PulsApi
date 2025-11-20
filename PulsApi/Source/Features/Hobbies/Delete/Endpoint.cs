using PulsApi.Data;

namespace PulsApi.Hobbies.Delete
{
    sealed class Endpoint : EndpointWithoutRequest
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Delete("/api/hobbies/{id}");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var id = Route<int>("id");
            var hobby = await Db.Hobbies.FindAsync(new object[] { id }, c);

            if (hobby == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            Db.Hobbies.Remove(hobby);
            await Db.SaveChangesAsync(c);

            await SendNoContentAsync(c);
        }
    }
}
