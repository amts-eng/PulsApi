using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.People.AddHobby
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/people/{personId}/hobbies");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var personId = Route<int>("personId");
            
            // Verify person exists
            var person = await Db.People.FindAsync(new object[] { personId }, c);
            if (person == null)
            {
                AddError("Person not found");
                await SendErrorsAsync(404, c);
                return;
            }

            // Verify hobby exists
            var hobby = await Db.Hobbies.FindAsync(new object[] { r.HobbyId }, c);
            if (hobby == null)
            {
                AddError("Hobby not found");
                await SendErrorsAsync(404, c);
                return;
            }

            // Check if already assigned
            var existing = await Db.PersonHobbies
                .FirstOrDefaultAsync(ph => ph.PersonId == personId && ph.HobbyId == r.HobbyId, c);
            
            if (existing != null)
            {
                AddError("Hobby already assigned to this person");
                await SendErrorsAsync(409, c);
                return;
            }

            var personHobby = new Hobbies.Models.PersonHobby
            {
                PersonId = personId,
                HobbyId = r.HobbyId,
                AssignedAt = DateTime.UtcNow
            };

            Db.PersonHobbies.Add(personHobby);
            await Db.SaveChangesAsync(c);

            await SendAsync(new Response
            {
                PersonId = personId,
                HobbyId = r.HobbyId,
                HobbyName = hobby.Name,
                AssignedAt = personHobby.AssignedAt
            }, 201, c);
        }
    }
}
