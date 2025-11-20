namespace PulsApi.Teams.Create
{
    sealed class Response
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
