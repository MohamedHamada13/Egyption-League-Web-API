using System.ComponentModel.DataAnnotations;

namespace WebAPI_Revision.DTOs.TeamDtos
{
    public class TeamUpdateDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public DateOnly EstablishingDate { get; set; }
    }
}
