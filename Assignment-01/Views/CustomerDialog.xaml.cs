using System.Windows;
using Assignment_01.ViewModels;

namespace Assignment_01.Views
{
    public partial class CustomerDialog : Window
    {
        public CustomerDialog()
        {
            InitializeComponent();
        }

        public CustomerDialog(CustomerDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is CustomerDialogViewModel viewModel)
            {
                viewModel.Password = ((System.Windows.Controls.PasswordBox)sender).Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is CustomerDialogViewModel viewModel)
            {
                viewModel.ConfirmPassword = ((System.Windows.Controls.PasswordBox)sender).Password;
            }
        }
    }
}
