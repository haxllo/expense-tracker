using System;
using System.Windows;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views
{
    public partial class AddExpenseView : Window
    {
        public AddExpenseView()
        {
            InitializeComponent();
            var viewModel = new AddExpenseViewModel();
            viewModel.SetWindow(this);
            DataContext = viewModel;
            
            // Dispose ViewModel when window closes
            Closed += (sender, args) =>
            {
                if (DataContext is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            };
        }
    }
}
