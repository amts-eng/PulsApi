namespace PulsApi.Teams.GetByBusinessUnit
{
    sealed class Response
    {
        public List<TeamDto> Teams { get; set; } = new();
    }

    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
    }
}
