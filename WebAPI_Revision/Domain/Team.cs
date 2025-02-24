using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI_Revision.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(40)]
        public string? Name { get; set; }
        [Required]
        public DateOnly EstablishingDate { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = null;
        public bool IsDeleted { get; set; }

        public virtual ICollection<Player>? Players { get; set; } = new List<Player>(); // NP 
        // virtual to apply Lazy Loading
    }

    /// <summary>
    /// => Important Notes: 
    /// - Controller apply the annotations constraints if only you not use the annotation [apiController] before the controller.
    /// - EstablishingDate prop from type 'DateOnly' to determine only date, While AddedDate & UpdatedDate props from type 'DateTime' to determine the date and time.
    /// - UpdatedDate prop be a null untill you update the object.
    /// </summary>

}
