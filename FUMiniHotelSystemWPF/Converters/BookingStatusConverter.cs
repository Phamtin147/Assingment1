using System;
using System.Globalization;
using System.Windows.Data;

namespace FUMiniHotelSystemWPF.Converters
{
    public class BookingStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                return status switch
                {
                    1 => "Chờ xác nhận",
                    2 => "Đã xác nhận",
                    3 => "Đã hủy",
                    _ => "Không xác định"
                };
            }
            return "Không xác định";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

