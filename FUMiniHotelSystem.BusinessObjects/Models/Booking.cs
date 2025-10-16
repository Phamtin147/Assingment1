namespace FUMiniHotelSystem.BusinessObjects.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int RoomID { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int BookingStatus { get; set; } // 1: Confirmed, 2: Completed, 3: Cancelled
        public DateTime CreatedDate { get; set; }
        
        // Navigation properties
        public Customer? Customer { get; set; }
        public RoomInformation? Room { get; set; }
    }
}

