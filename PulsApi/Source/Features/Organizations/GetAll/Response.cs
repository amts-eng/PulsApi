namespace PulsApi.Organizations.GetAll
{
    sealed class Response
    {
        public List<OrganizationDto> Organizations { get; set; } = new();
    }

    public class OrganizationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
