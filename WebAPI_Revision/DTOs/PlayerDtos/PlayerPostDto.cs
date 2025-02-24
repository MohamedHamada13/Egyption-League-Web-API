using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI_Revision.Entities;

namespace WebAPI_Revision.DTOs.PlayerDtos
{
    public class PlayerPostDto
    {
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        [Required]
        public int JerseyNumber { get; set; }
        [Required]
        public DateOnly BirthOfDate { get; set; }
        [Required]
        public int TeamId { get; set; } // FK

        /// Here I maked the FK 'TeamId' as Required, while in the main model(Player) I maked the FK 'TeamId' as Nullable.
        /// Required in 'PlayerPostDto' to enforce the user to put each new adding player in a specific team.
        /// Nullable in 'Player' to avoid the issue that occurs when deleteing a team, then this team players refers to null.
        // جرب البوست بقي, وخلي البلاير تبع تيم بعدين امسح التيم وشوف هيحصل مشكله ولا لا والبلاير هيرفرنس علي اي 
    }
}
