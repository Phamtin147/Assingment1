namespace FUMiniHotelSystem.BusinessObjects.DTOs
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int CustomerID { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
    }
}



