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
            var person = await Db.People
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.Id == id, c);

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

            // Verify team exists if provided
            if (r.TeamId.HasValue && person.TeamId != r.TeamId)
            {
                var team = await Db.Teams.FindAsync(new object[] { r.TeamId.Value }, c);
                if (team == null)
                {
                    AddError("Team not found");
                    await SendErrorsAsync(404, c);
                    return;
                }
            }

            person.FirstName = r.FirstName;
            person.LastName = r.LastName;
            person.Email = r.Email;
            person.TeamId = r.TeamId;
            await Db.SaveChangesAsync(c);

            // Reload team if changed
            if (person.TeamId.HasValue)
            {
                await Db.Entry(person).Reference(p => p.Team).LoadAsync(c);
            }

            await SendAsync(new Response
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                TeamId = person.TeamId,
                TeamName = person.Team?.Name
            }, cancellation: c);
        }
    }
}
