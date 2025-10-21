using System.Windows;
using System.Windows.Input;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views
{
    public partial class BudgetView : Window
    {
        public BudgetView()
        {
            InitializeComponent();
            var viewModel = new BudgetViewModel();
            viewModel.SetWindow(this);
            DataContext = viewModel;
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
