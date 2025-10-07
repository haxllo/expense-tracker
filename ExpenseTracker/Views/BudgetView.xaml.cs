using System.Windows;
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
    }
}
