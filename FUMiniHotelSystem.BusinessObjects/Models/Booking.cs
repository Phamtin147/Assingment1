using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUMiniHotelSystem.BusinessObjects.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        
        [Required]
        public int CustomerID { get; set; }
        
        [Required]
        public int RoomID { get; set; }
        
        [Required]
        public DateTime BookingDate { get; set; }
        
        [Required]
        public DateTime CheckInDate { get; set; }
        
        [Required]
        public DateTime CheckOutDate { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        public int BookingStatus { get; set; } = 1; // 1 = Pending, 2 = Confirmed, 3 = Cancelled, 4 = Completed
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; } = null!;
        
        [ForeignKey("RoomID")]
        public virtual RoomInformation RoomInformation { get; set; } = null!;
    }
}

