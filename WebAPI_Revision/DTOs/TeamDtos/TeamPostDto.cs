using System.ComponentModel.DataAnnotations;

namespace WebAPI_Revision.DTOs.TeamDtos
{
    public class TeamPostDto
    {
        [Required, MaxLength(40)]
        public required string Name { get; set; }
        [Required]
        public DateOnly EstablishingDate { get; set; }
    }
}
