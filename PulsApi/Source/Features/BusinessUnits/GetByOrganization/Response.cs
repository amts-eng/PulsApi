namespace PulsApi.BusinessUnits.GetByOrganization
{
    sealed class Response
    {
        public List<BusinessUnitDto> BusinessUnits { get; set; } = new();
    }

    public class BusinessUnitDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
    }
}
