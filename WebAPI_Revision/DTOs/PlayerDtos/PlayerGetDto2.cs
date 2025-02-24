namespace WebAPI_Revision.DTOs.PlayerDtos
{
    public class PlayerGetDto2
    {
        /// Impor Note: 'PlayerGetDto2' is defined for only 'TeamWithPlayerGetDto' to get player without 'TeamId' prop to avoid "Circular issue".
        public int Id { get; set; }
        public required string Name { get; set; }
        public int JerseyNumber { get; set; }
        public DateOnly BirthOfDate { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
