namespace PulsApi.People.GetHobbies
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
        public DateTime AssignedAt { get; set; }
    }
}
