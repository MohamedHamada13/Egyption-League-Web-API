using WebAPI_Revision.DTOs.PlayerDtos;

namespace WebAPI_Revision.DTOs.TeamDtos
{
    public class TeamGetDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateOnly EstablishingDate { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        /// You must declare the 'UpdatedDate' prop as nullable, to can get the null value from Team object if it's value is null. 
    }
}
