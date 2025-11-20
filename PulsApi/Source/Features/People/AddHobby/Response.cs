namespace PulsApi.People.AddHobby
{
    sealed class Response
    {
        public int PersonId { get; set; }
        public int HobbyId { get; set; }
        public string HobbyName { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
    }
}
