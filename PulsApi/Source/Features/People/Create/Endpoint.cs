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

            // Verify team exists if provided
            string? teamName = null;
            if (r.TeamId.HasValue)
            {
                var team = await Db.Teams.FindAsync(new object[] { r.TeamId.Value }, c);
                if (team == null)
                {
                    AddError("Team not found");
                    await SendErrorsAsync(404, c);
                    return;
                }
                teamName = team.Name;
            }

            var person = new Models.Person
            {
                FirstName = r.FirstName,
                LastName = r.LastName,
                Email = r.Email,
                TeamId = r.TeamId
            };

            Db.People.Add(person);
            await Db.SaveChangesAsync(c);

            await SendAsync(new Response
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                TeamId = person.TeamId,
                TeamName = teamName
            }, 201, c);
        }
    }
}
