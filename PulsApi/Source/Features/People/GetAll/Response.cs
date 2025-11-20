namespace PulsApi.People.GetAll
{
    sealed class Response
    {
        public List<PersonDto> People { get; set; } = new();
    }

    public class PersonDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
