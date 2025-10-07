using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private ObservableCollection<Expense> _expenses;
        private ObservableCollection<Category> _categories;
        private Expense? _selectedExpense;
        private decimal _totalExpenses;
        private decimal _monthlyTotal;
        private string _searchText = string.Empty;
        private DateTime _filterStartDate;
        private DateTime _filterEndDate;

        public MainViewModel()
        {
            _dataService = new DataService();
            _expenses = new ObservableCollection<Expense>();
            _categories = new ObservableCollection<Category>();
            _filterStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _filterEndDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1); // End of today

            AddExpenseCommand = new RelayCommand(_ => AddExpense());
            EditExpenseCommand = new RelayCommand(_ => EditExpense(), _ => SelectedExpense != null);
            DeleteExpenseCommand = new RelayCommand(_ => DeleteExpense(), _ => SelectedExpense != null);
            RefreshCommand = new RelayCommand(async _ => await LoadDataAsync());
            SearchCommand = new RelayCommand(async _ => await SearchExpensesAsync());

            InitializeAsync();
        }

        public ObservableCollection<Expense> Expenses
        {
            get => _expenses;
            set => SetProperty(ref _expenses, value);
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public Expense? SelectedExpense
        {
            get => _selectedExpense;
            set => SetProperty(ref _selectedExpense, value);
        }

        public decimal TotalExpenses
        {
            get => _totalExpenses;
            set => SetProperty(ref _totalExpenses, value);
        }

        public decimal MonthlyTotal
        {
            get => _monthlyTotal;
            set => SetProperty(ref _monthlyTotal, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public DateTime FilterStartDate
        {
            get => _filterStartDate;
            set
            {
                if (SetProperty(ref _filterStartDate, value))
                {
                    _ = LoadDataAsync();
                }
            }
        }

        public DateTime FilterEndDate
        {
            get => _filterEndDate;
            set
            {
                if (SetProperty(ref _filterEndDate, value))
                {
                    _ = LoadDataAsync();
                }
            }
        }

        public ICommand AddExpenseCommand { get; }
        public ICommand EditExpenseCommand { get; }
        public ICommand DeleteExpenseCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }

        private async void InitializeAsync()
        {
            await _dataService.InitializeDatabaseAsync();
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            // Update filter end date to include all expenses up to now
            if (FilterEndDate.Date == DateTime.Now.Date)
            {
                FilterEndDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1); // End of today
            }
            
            // Create a fresh DataService to ensure we get latest data
            using (var freshDataService = new DataService())
            {
                var expenses = await freshDataService.GetExpensesAsync(FilterStartDate, FilterEndDate);
                Expenses.Clear();
                foreach (var expense in expenses)
                {
                    Expenses.Add(expense);
                }

                var categories = await freshDataService.GetCategoriesAsync();
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                TotalExpenses = await freshDataService.GetTotalExpensesAsync();
                MonthlyTotal = await freshDataService.GetTotalExpensesAsync(
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                    DateTime.Now
                );
            }
        }

        private async Task SearchExpensesAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadDataAsync();
                return;
            }

            using (var freshDataService = new DataService())
            {
                var allExpenses = await freshDataService.GetExpensesAsync(FilterStartDate, FilterEndDate);
                var filtered = allExpenses.Where(e => 
                    e.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    e.Category?.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true
                ).ToList();

                Expenses.Clear();
                foreach (var expense in filtered)
                {
                    Expenses.Add(expense);
                }
            }
        }

        private async void AddExpense()
        {
            var addExpenseView = new Views.AddExpenseView();
            if (addExpenseView.ShowDialog() == true)
            {
                // Small delay to ensure database file is fully written
                await Task.Delay(50);
                await LoadDataAsync();
            }
        }

        private async void EditExpense()
        {
            if (SelectedExpense == null) return;
            
            var addExpenseView = new Views.AddExpenseView();
            var viewModel = addExpenseView.DataContext as AddExpenseViewModel;
            
            if (viewModel != null)
            {
                viewModel.LoadExpense(SelectedExpense);
            }
            
            if (addExpenseView.ShowDialog() == true)
            {
                await LoadDataAsync();
            }
        }

        private async void DeleteExpense()
        {
            if (SelectedExpense == null) return;

            using (var freshDataService = new DataService())
            {
                await freshDataService.DeleteExpenseAsync(SelectedExpense.Id);
            }
            
            await LoadDataAsync();
        }
    }
}
