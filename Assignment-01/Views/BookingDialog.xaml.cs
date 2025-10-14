using System.Windows;
using Assignment_01.ViewModels;

namespace Assignment_01.Views
{
    public partial class BookingDialog : Window
    {
        public BookingDialog()
        {
            InitializeComponent();
        }

        public BookingDialog(BookingDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
