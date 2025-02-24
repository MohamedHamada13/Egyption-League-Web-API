namespace WebAPI_Revision.DTOs.PlayerDtos
{
    public class PlayerGetDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int JerseyNumber { get; set; }
        public DateOnly BirthOfDate { get; set; }
        public DateTime AddedDate { get; set; }
        public int? TeamId { get; set; } // FK
    }
}
