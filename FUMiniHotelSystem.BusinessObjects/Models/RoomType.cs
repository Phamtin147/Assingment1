namespace FUMiniHotelSystem.BusinessObjects.Models
{
    public class RoomType
    {
        public int RoomTypeID { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;
        public string? TypeDescription { get; set; }
        public string? TypeNote { get; set; }
    }
}


