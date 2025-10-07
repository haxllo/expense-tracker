using System.Windows;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views
{
    public partial class ReportsView : Window
    {
        public ReportsView()
        {
            InitializeComponent();
            DataContext = new ReportsViewModel();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
