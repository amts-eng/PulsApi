namespace PulsApi.Hobbies.GetAll
{
    sealed class Response
    {
        public List<HobbyDto> Hobbies { get; set; } = new();
    }

    public class HobbyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
