using PulsApi.Data;

namespace PulsApi.Hobbies.Create
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/hobbies");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var hobby = new Models.Hobby
            {
                Name = r.Name,
                Description = r.Description
            };

            Db.Hobbies.Add(hobby);
            await Db.SaveChangesAsync(c);

            await SendAsync(new Response
            {
                Id = hobby.Id,
                Name = hobby.Name,
                Description = hobby.Description
            }, 201, c);
        }
    }
}
