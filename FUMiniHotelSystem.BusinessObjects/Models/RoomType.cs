using System.ComponentModel.DataAnnotations;

namespace FUMiniHotelSystem.BusinessObjects.Models
{
    public class RoomType
    {
        public int RoomTypeID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string RoomTypeName { get; set; } = string.Empty;
        
        [StringLength(250)]
        public string TypeDescription { get; set; } = string.Empty;
        
        [StringLength(250)]
        public string TypeNote { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<RoomInformation> RoomInformations { get; set; } = new List<RoomInformation>();
    }
}

