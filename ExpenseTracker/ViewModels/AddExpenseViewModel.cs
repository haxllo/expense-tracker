using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels
{
    public class AddExpenseViewModel : ViewModelBase, IDisposable
    {
        private IDataService? _dataService;
        private decimal? _amount;
        private int _selectedCategoryId;
        private string _description = string.Empty;
        private DateTime _date;
        private string _paymentMethod = string.Empty;
        private ObservableCollection<Category> _categories;
        private bool _isEditMode;
        private int _expenseId;
        private Window? _window;

        public AddExpenseViewModel()
        {
            _dataService = new DataService();
            _categories = new ObservableCollection<Category>();

            SaveCommand = new RelayCommand(async _ => await SaveExpenseAsync(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => Cancel());

            LoadCategoriesAsync();
        }

        public decimal? Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public int SelectedCategoryId
        {
            get => _selectedCategoryId;
            set => SetProperty(ref _selectedCategoryId, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public string PaymentMethod
        {
            get => _paymentMethod;
            set => SetProperty(ref _paymentMethod, value);
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        public void LoadExpense(Expense expense)
        {
            IsEditMode = true;
            _expenseId = expense.Id;
            Amount = expense.Amount;
            SelectedCategoryId = expense.CategoryId;
            Description = expense.Description;
            Date = expense.Date;
            PaymentMethod = expense.PaymentMethod;
            
            if (_window != null)
            {
                _window.Title = "Edit Expense";
            }
        }

        private async void LoadCategoriesAsync()
        {
            if (_dataService == null) return;
            
            var categories = await _dataService.GetCategoriesAsync();
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }

            // Don't auto-select first category - let user choose
        }

        private bool CanSave()
        {
            return Amount.HasValue && Amount.Value > 0 && SelectedCategoryId > 0;
        }

        private async Task SaveExpenseAsync()
        {
            if (!CanSave())
            {
                MessageBox.Show("Please fill in all required fields correctly.", "Validation Error", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var expense = new Expense
                {
                    Amount = Amount.Value,
                    CategoryId = SelectedCategoryId,
                    Description = Description,
                    Date = IsEditMode ? Date : DateTime.Now,
                    PaymentMethod = PaymentMethod
                };

                if (_dataService != null)
                {
                    if (IsEditMode)
                    {
                        expense.Id = _expenseId;
                        await _dataService.UpdateExpenseAsync(expense);
                    }
                    else
                    {
                        await _dataService.AddExpenseAsync(expense);
                    }
                    
                    // Dispose the data service to ensure database is released
                    _dataService.Dispose();
                    _dataService = null;
                }

                if (_window != null)
                {
                    _window.DialogResult = true;
                    _window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving expense: {ex.Message}", "Error", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            if (_window != null)
            {
                _window.DialogResult = false;
                _window.Close();
            }
        }

        public void Dispose()
        {
            _dataService?.Dispose();
            _dataService = null;
        }
    }
}
