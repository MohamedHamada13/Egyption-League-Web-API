using WebAPI_Revision.DTOs.PlayerDtos;

namespace WebAPI_Revision.DTOs.TeamDtos
{
    public class TeamWithPlayersGetDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateOnly EstablishingDate { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<PlayerGetDto2>? Players { get; set; } // 'List<PlayerGetDto>' not 'List<Player>' to avoid circular issue
    }
}
