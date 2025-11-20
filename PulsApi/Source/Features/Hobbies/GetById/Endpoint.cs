using PulsApi.Data;

namespace PulsApi.Hobbies.GetById
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/hobbies/{id}");
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

            await SendAsync(new Response
            {
                Id = hobby.Id,
                Name = hobby.Name,
                Description = hobby.Description
            }, cancellation: c);
        }
    }
}
