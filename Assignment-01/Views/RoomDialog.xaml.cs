using System.Windows;
using Assignment_01.ViewModels;

namespace Assignment_01.Views
{
    public partial class RoomDialog : Window
    {
        public RoomDialog()
        {
            InitializeComponent();
        }

        public RoomDialog(RoomDialogViewModel viewModel)
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
