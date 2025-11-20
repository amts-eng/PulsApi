using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.People.Create
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/people");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            // Check if email already exists
            var existingPerson = await Db.People.FirstOrDefaultAsync(p => p.Email == r.Email, c);
            if (existingPerson != null)
            {
                AddError("Email already exists");
                await SendErrorsAsync(409, c);
                return;
            }

            var person = new Models.Person
            {
                FirstName = r.FirstName,
                LastName = r.LastName,
                Email = r.Email
            };

            Db.People.Add(person);
            await Db.SaveChangesAsync(c);

            await SendAsync(new Response
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email
            }, 201, c);
        }
    }
}
