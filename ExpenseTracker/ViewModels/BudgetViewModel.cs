using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels
{
    public class BudgetViewModel : ViewModelBase
    {
        private ObservableCollection<CategoryBudget> _categoryBudgets;
        private CategoryBudget? _selectedBudget;
        private int _currentMonth;
        private int _currentYear;
        private Window? _window;

        public BudgetViewModel()
        {
            _categoryBudgets = new ObservableCollection<CategoryBudget>();
            _currentMonth = DateTime.Now.Month;
            _currentYear = DateTime.Now.Year;

            SaveCommand = new RelayCommand(async _ => await SaveBudgetsAsync());
            CancelCommand = new RelayCommand(_ => Cancel());

            _ = LoadBudgetsAsync();
        }

        public ObservableCollection<CategoryBudget> CategoryBudgets
        {
            get => _categoryBudgets;
            set => SetProperty(ref _categoryBudgets, value);
        }

        public CategoryBudget? SelectedBudget
        {
            get => _selectedBudget;
            set => SetProperty(ref _selectedBudget, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        private async Task LoadBudgetsAsync()
        {
            using (var dataService = new DataService())
            {
                await dataService.InitializeDatabaseAsync();

                var categories = await dataService.GetCategoriesAsync();
                var budgets = await dataService.GetBudgetsAsync(_currentMonth, _currentYear);
                var currentMonthStart = new DateTime(_currentYear, _currentMonth, 1);
                var expenses = await dataService.GetExpensesAsync(currentMonthStart, DateTime.Now);

                CategoryBudgets.Clear();
                foreach (var category in categories)
                {
                    var budget = budgets.FirstOrDefault(b => b.CategoryId == category.Id);
                    var spent = expenses.Where(e => e.CategoryId == category.Id).Sum(e => e.Amount);

                    CategoryBudgets.Add(new CategoryBudget
                    {
                        CategoryId = category.Id,
                        CategoryName = category.Name,
                        CategoryIcon = category.Icon,
                        CategoryColor = category.Color,
                        BudgetLimit = budget?.MonthlyLimit ?? 0,
                        Spent = spent,
                        Month = _currentMonth,
                        Year = _currentYear
                    });
                }
            }
        }

        private async Task SaveBudgetsAsync()
        {
            try
            {
                using (var dataService = new DataService())
                {
                    foreach (var categoryBudget in CategoryBudgets.Where(cb => cb.BudgetLimit > 0))
                    {
                        var budget = new Budget
                        {
                            CategoryId = categoryBudget.CategoryId,
                            MonthlyLimit = categoryBudget.BudgetLimit,
                            Month = _currentMonth,
                            Year = _currentYear,
                            CreatedAt = DateTime.Now
                        };

                        await dataService.AddOrUpdateBudgetAsync(budget);
                    }
                }

                MessageBox.Show("Budgets saved successfully!", "Success",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                if (_window != null)
                {
                    _window.DialogResult = true;
                    _window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving budgets: {ex.Message}", "Error",
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
    }

    public class CategoryBudget : ViewModelBase
    {
        private int _categoryId;
        private string _categoryName = string.Empty;
        private string _categoryIcon = string.Empty;
        private string _categoryColor = string.Empty;
        private decimal _budgetLimit;
        private decimal _spent;
        private int _month;
        private int _year;

        public int CategoryId
        {
            get => _categoryId;
            set => SetProperty(ref _categoryId, value);
        }

        public string CategoryName
        {
            get => _categoryName;
            set => SetProperty(ref _categoryName, value);
        }

        public string CategoryIcon
        {
            get => _categoryIcon;
            set => SetProperty(ref _categoryIcon, value);
        }

        public string CategoryColor
        {
            get => _categoryColor;
            set => SetProperty(ref _categoryColor, value);
        }

        public decimal BudgetLimit
        {
            get => _budgetLimit;
            set
            {
                if (SetProperty(ref _budgetLimit, value))
                {
                    OnPropertyChanged(nameof(Remaining));
                    OnPropertyChanged(nameof(PercentageUsed));
                    OnPropertyChanged(nameof(ProgressBarColor));
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }

        public decimal Spent
        {
            get => _spent;
            set
            {
                if (SetProperty(ref _spent, value))
                {
                    OnPropertyChanged(nameof(Remaining));
                    OnPropertyChanged(nameof(PercentageUsed));
                    OnPropertyChanged(nameof(ProgressBarColor));
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }

        public int Month
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }

        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        public decimal Remaining => BudgetLimit - Spent;

        public double PercentageUsed => BudgetLimit > 0 ? (double)(Spent / BudgetLimit * 100) : 0;

        public Brush ProgressBarColor
        {
            get
            {
                if (BudgetLimit == 0) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0")); // Gray
                var percentage = PercentageUsed;
                if (percentage >= 100) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336")); // Red
                if (percentage >= 80) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800"));  // Orange
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50")); // Green
            }
        }

        public string StatusText
        {
            get
            {
                if (BudgetLimit == 0) return "No budget set";
                var percentage = PercentageUsed;
                if (percentage >= 100) return "⚠️ Over Budget!";
                if (percentage >= 80) return "⚠️ Warning";
                return "✓ On Track";
            }
        }
    }
}
