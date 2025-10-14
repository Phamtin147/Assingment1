using System.Configuration;
using System.Data;
using System.Windows;
using FUMiniHotelSystem.DataAccess.Database;

namespace Assignment_01
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Initialize SQL Server Database
            try
            {
                var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=FUMiniHotelSystem;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true";
                var dbContext = new HotelDbContext(connectionString);
                await dbContext.InitializeDatabaseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
