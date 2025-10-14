using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUMiniHotelSystem.BusinessObjects.Models
{
    public class RoomInformation
    {
        public int RoomID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string RoomNumber { get; set; } = string.Empty;
        
        [StringLength(220)]
        public string RoomDescription { get; set; } = string.Empty;
        
        [Required]
        public int RoomMaxCapacity { get; set; }
        
        public int RoomStatus { get; set; } = 1; // 1 = Active, 2 = Deleted
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RoomPricePerDate { get; set; }
        
        [Required]
        public int RoomTypeID { get; set; }
        
        // Navigation properties
        [ForeignKey("RoomTypeID")]
        public virtual RoomType RoomType { get; set; } = null!;
        
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

