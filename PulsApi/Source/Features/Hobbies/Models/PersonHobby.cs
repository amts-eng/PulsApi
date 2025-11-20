namespace PulsApi.Hobbies.Models
{
    public sealed class PersonHobby
    {
        public int PersonId { get; set; }
        public People.Models.Person Person { get; set; } = null!;
        
        public int HobbyId { get; set; }
        public Hobby Hobby { get; set; } = null!;
        
        public DateTime AssignedAt { get; set; }
    }
}
