namespace FUMiniHotelSystem.BusinessObjects.Models
{
    public class RoomInformation
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string? RoomDescription { get; set; }
        public int RoomMaxCapacity { get; set; }
        public int RoomStatus { get; set; } // 1: Active, 2: Deleted
        public decimal RoomPricePerDate { get; set; }
        public int RoomTypeID { get; set; }
        
        // Navigation property
        public RoomType? RoomType { get; set; }
    }
}


