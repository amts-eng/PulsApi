using PulsApi.Data;

namespace PulsApi.Hobbies.Update
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Put("/api/hobbies/{id}");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var id = Route<int>("id");
            var hobby = await Db.Hobbies.FindAsync(new object[] { id }, c);

            if (hobby == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            hobby.Name = r.Name;
            hobby.Description = r.Description;
            await Db.SaveChangesAsync(c);

            await SendAsync(new Response
            {
                Id = hobby.Id,
                Name = hobby.Name,
                Description = hobby.Description
            }, cancellation: c);
        }
    }
}
