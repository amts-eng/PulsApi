using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Organizations.GetTree
{
    sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/organizations/{id}/tree");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var id = Route<int>("id");
            
            // Load organization with all nested entities
            var organization = await Db.Organizations
                .Where(o => o.Id == id)
                .Select(o => new Response
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description,
                    BusinessUnits = Db.BusinessUnits
                        .Where(bu => bu.OrganizationId == o.Id)
                        .Select(bu => new BusinessUnitDto
                        {
                            Id = bu.Id,
                            Name = bu.Name,
                            Description = bu.Description,
                            Teams = Db.Teams
                                .Where(t => t.BusinessUnitId == bu.Id)
                                .Select(t => new TeamDto
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                    Description = t.Description,
                                    People = Db.People
                                        .Where(p => p.TeamId == t.Id)
                                        .Select(p => new PersonDto
                                        {
                                            Id = p.Id,
                                            FirstName = p.FirstName,
                                            LastName = p.LastName,
                                            Email = p.Email,
                                            Hobbies = Db.PersonHobbies
                                                .Where(ph => ph.PersonId == p.Id)
                                                .Select(ph => new HobbyDto
                                                {
                                                    Id = ph.Hobby.Id,
                                                    Name = ph.Hobby.Name,
                                                    Description = ph.Hobby.Description,
                                                    AssignedAt = ph.AssignedAt
                                                })
                                                .ToList()
                                        })
                                        .ToList()
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(c);

            if (organization == null)
            {
                await SendNotFoundAsync(c);
                return;
            }

            await SendAsync(organization, cancellation: c);
        }
    }
}
