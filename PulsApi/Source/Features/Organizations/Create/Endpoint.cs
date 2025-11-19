using PulsApi.Data;

namespace Organizations.Create
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/organizations");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var organization = new Models.Organization
            {
                Name = r.Name,
                Description = r.Description
            };

            Db.Organizations.Add(organization);
            await Db.SaveChangesAsync(c);

            await SendAsync(new Response
            {
                Id = organization.Id,
                Name = organization.Name,
                Description = organization.Description
            }, 201, c);
        }
    }
}
