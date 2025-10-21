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
        private decimal _weeklyTotal;
        private decimal _averageDaily;
        private string _topCategory = "N/A";
        private string _trendIndicator = "";
        private string _trendColor = "#666";
        private string _searchText = string.Empty;
        private DateTime _filterStartDate;
        private DateTime _filterEndDate;
        private string _themeIcon = "🌙";
        private string _currentSortColumn = "Date";
        private bool _sortAscending = false;

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
            ShowBudgetCommand = new RelayCommand(async _ => await ShowBudgetAsync());
            ShowReportsCommand = new RelayCommand(_ => ShowReports());
            ToggleThemeCommand = new RelayCommand(_ => ToggleTheme());
            SortCommand = new RelayCommand(param => SortExpenses(param?.ToString() ?? "Date"));

            UpdateThemeIcon();
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

        public decimal WeeklyTotal
        {
            get => _weeklyTotal;
            set => SetProperty(ref _weeklyTotal, value);
        }

        public decimal AverageDaily
        {
            get => _averageDaily;
            set => SetProperty(ref _averageDaily, value);
        }

        public string TopCategory
        {
            get => _topCategory;
            set => SetProperty(ref _topCategory, value);
        }

        public string TrendIndicator
        {
            get => _trendIndicator;
            set => SetProperty(ref _trendIndicator, value);
        }

        public string TrendColor
        {
            get => _trendColor;
            set => SetProperty(ref _trendColor, value);
        }

        public string ThemeIcon
        {
            get => _themeIcon;
            set => SetProperty(ref _themeIcon, value);
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
        public ICommand ShowBudgetCommand { get; }
        public ICommand ShowReportsCommand { get; }
        public ICommand ToggleThemeCommand { get; }
        public ICommand SortCommand { get; }

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
                
                // Calculate monthly total
                var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                MonthlyTotal = await freshDataService.GetTotalExpensesAsync(currentMonth, DateTime.Now);
                
                // Calculate last month total for trend
                var lastMonth = currentMonth.AddMonths(-1);
                var lastMonthEnd = currentMonth.AddDays(-1);
                var lastMonthTotal = await freshDataService.GetTotalExpensesAsync(lastMonth, lastMonthEnd);
                
                // Calculate trend
                if (lastMonthTotal > 0)
                {
                    var change = MonthlyTotal - lastMonthTotal;
                    var percentChange = (change / lastMonthTotal) * 100;
                    
                    if (change > 0)
                    {
                        TrendIndicator = $"↑ {percentChange:F1}%";
                        TrendColor = "#F44336"; // Red for increase
                    }
                    else if (change < 0)
                    {
                        TrendIndicator = $"↓ {Math.Abs(percentChange):F1}%";
                        TrendColor = "#4CAF50"; // Green for decrease
                    }
                    else
                    {
                        TrendIndicator = "→ 0%";
                        TrendColor = "#666"; // Gray for no change
                    }
                }
                else
                {
                    TrendIndicator = "New";
                    TrendColor = "#2196F3"; // Blue for new
                }
                
                // Calculate weekly total (last 7 days)
                var weekAgo = DateTime.Now.AddDays(-7);
                WeeklyTotal = await freshDataService.GetTotalExpensesAsync(weekAgo, DateTime.Now);
                
                // Calculate average daily (this month)
                var daysInMonth = (DateTime.Now - currentMonth).Days + 1;
                AverageDaily = daysInMonth > 0 ? MonthlyTotal / daysInMonth : 0;
                
                // Get top category
                var categoryExpenses = await freshDataService.GetExpensesByCategoryAsync(currentMonth, DateTime.Now);
                TopCategory = categoryExpenses.OrderByDescending(x => x.Value).FirstOrDefault().Key ?? "N/A";
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

            var result = System.Windows.MessageBox.Show(
                $"Are you sure you want to delete this expense?\n\n" +
                $"Amount: ${SelectedExpense.Amount:N2}\n" +
                $"Category: {SelectedExpense.Category?.Name}\n" +
                $"Description: {SelectedExpense.Description}\n" +
                $"Date: {SelectedExpense.Date:MMM d, yyyy}",
                "Confirm Delete",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result != System.Windows.MessageBoxResult.Yes)
                return;

            using (var freshDataService = new DataService())
            {
                await freshDataService.DeleteExpenseAsync(SelectedExpense.Id);
            }
            
            await LoadDataAsync();
        }

        private async Task ShowBudgetAsync()
        {
            var budgetView = new Views.BudgetView();
            if (budgetView.ShowDialog() == true)
            {
                await LoadDataAsync();
            }
        }

        private void ShowReports()
        {
            var reportsView = new Views.ReportsView();
            reportsView.Show();
        }

        private void ToggleTheme()
        {
            ThemeManager.ToggleTheme();
            UpdateThemeIcon();
        }

        private void UpdateThemeIcon()
        {
            var currentTheme = ThemeManager.GetCurrentTheme();
            ThemeIcon = currentTheme == "Light" ? "🌙" : "☀️";
        }

        private void SortExpenses(string column)
        {
            if (_currentSortColumn == column)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _currentSortColumn = column;
                _sortAscending = true;
            }

            var sortedList = column switch
            {
                "Date" => _sortAscending 
                    ? Expenses.OrderBy(e => e.Date).ToList() 
                    : Expenses.OrderByDescending(e => e.Date).ToList(),
                "Amount" => _sortAscending 
                    ? Expenses.OrderBy(e => e.Amount).ToList() 
                    : Expenses.OrderByDescending(e => e.Amount).ToList(),
                "Category" => _sortAscending 
                    ? Expenses.OrderBy(e => e.Category?.Name).ToList() 
                    : Expenses.OrderByDescending(e => e.Category?.Name).ToList(),
                "Description" => _sortAscending 
                    ? Expenses.OrderBy(e => e.Description).ToList() 
                    : Expenses.OrderByDescending(e => e.Description).ToList(),
                _ => Expenses.OrderByDescending(e => e.Date).ToList()
            };

            Expenses.Clear();
            foreach (var expense in sortedList)
            {
                Expenses.Add(expense);
            }
        }
    }
}
