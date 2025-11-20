using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.People.RemoveHobby
{
    sealed class Endpoint : EndpointWithoutRequest
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Delete("/api/people/{personId}/hobbies/{hobbyId}");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var personId = Route<int>("personId");
            var hobbyId = Route<int>("hobbyId");
            
            var personHobby = await Db.PersonHobbies
                .FirstOrDefaultAsync(ph => ph.PersonId == personId && ph.HobbyId == hobbyId, c);
            
            if (personHobby == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            Db.PersonHobbies.Remove(personHobby);
            await Db.SaveChangesAsync(c);

            await SendNoContentAsync(c);
        }
    }
}
