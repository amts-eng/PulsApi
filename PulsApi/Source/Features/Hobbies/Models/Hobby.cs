namespace PulsApi.Hobbies.Models
{
    public sealed class Hobby
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Navigation property for many-to-many
        public ICollection<PersonHobby> PersonHobbies { get; set; } = new List<PersonHobby>();
    }
}
