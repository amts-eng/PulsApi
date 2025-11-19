using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace Organizations.GetById
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/organizations/{id}");
            AllowAnonymous();
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

            await SendAsync(new Response
            {
                Id = organization.Id,
                Name = organization.Name,
                Description = organization.Description
            }, cancellation: c);
        }
    }
}
