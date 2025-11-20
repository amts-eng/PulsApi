using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.People.Update
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Put("/api/people/{id}");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var id = Route<int>("id");
            var person = await Db.People.FindAsync(new object[] { id }, c);

            if (person == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            // Check if email is being changed and if it already exists
            if (person.Email != r.Email)
            {
                var emailExists = await Db.People.AnyAsync(p => p.Email == r.Email && p.Id != id, c);
                if (emailExists)
                {
                    AddError("Email already exists");
                    await SendErrorsAsync(409, c);
                    return;
                }
            }

            person.FirstName = r.FirstName;
            person.LastName = r.LastName;
            person.Email = r.Email;
            await Db.SaveChangesAsync(c);

            await SendAsync(new Response
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email
            }, cancellation: c);
        }
    }
}
