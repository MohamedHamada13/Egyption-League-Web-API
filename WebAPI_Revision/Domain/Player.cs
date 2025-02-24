using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Revision.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; } // Data Annotation constraints applyed in Dtos
        public int JerseyNumber { get; set; }
        public DateOnly BirthOfDate { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
        public DateTime? Updatedate { get; set; } = null;
        public bool IsDeleted { get; set; }

        [ForeignKey("Team")]
        public int? TeamId { get; set; } // FK
        public virtual Team? Team { get; set; } // NP
    }
}
