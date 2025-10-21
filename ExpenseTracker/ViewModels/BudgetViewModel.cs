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
        private string _selectedCategory = string.Empty;
        private decimal? _newBudgetAmount;
        private string _selectedPeriod = "Monthly";
        private ObservableCollection<string> _availableCategories;
        private decimal _masterMonthlyBudget;

        public BudgetViewModel()
        {
            _categoryBudgets = new ObservableCollection<CategoryBudget>();
            _availableCategories = new ObservableCollection<string>();
            _currentMonth = DateTime.Now.Month;
            _currentYear = DateTime.Now.Year;

            SaveCommand = new RelayCommand(async _ => await SaveBudgetsAsync());
            CancelCommand = new RelayCommand(_ => Cancel());
            AddBudgetCommand = new RelayCommand(_ => AddNewBudget(), _ => CanAddBudget());
            DeleteBudgetCommand = new RelayCommand<CategoryBudget>(async budget => await DeleteBudget(budget));

            LoadMasterBudget();
            _ = LoadBudgetsAsync();
        }

        public ObservableCollection<CategoryBudget> CategoryBudgets
        {
            get => _categoryBudgets;
            set
            {
                if (SetProperty(ref _categoryBudgets, value))
                {
                    UpdateStatCards();
                }
            }
        }

        public CategoryBudget? SelectedBudget
        {
            get => _selectedBudget;
            set => SetProperty(ref _selectedBudget, value);
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    System.Windows.Input.CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public decimal? NewBudgetAmount
        {
            get => _newBudgetAmount;
            set
            {
                if (SetProperty(ref _newBudgetAmount, value))
                {
                    System.Windows.Input.CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string SelectedPeriod
        {
            get => _selectedPeriod;
            set => SetProperty(ref _selectedPeriod, value);
        }

        public ObservableCollection<string> AvailableCategories
        {
            get => _availableCategories;
            set => SetProperty(ref _availableCategories, value);
        }

        public decimal MasterMonthlyBudget
        {
            get => _masterMonthlyBudget;
            set
            {
                if (SetProperty(ref _masterMonthlyBudget, value))
                {
                    SaveMasterBudget();
                    UpdateStatCards();
                }
            }
        }

        // Calculated properties
        public decimal TotalBudget => CategoryBudgets.Sum(cb => cb.BudgetLimit);
        
        public decimal TotalSpent => CategoryBudgets.Sum(cb => cb.Spent);
        
        public decimal RemainingBudget => TotalBudget - TotalSpent;
        
        public decimal UnallocatedBudget => MasterMonthlyBudget - TotalBudget;
        
        public decimal TotalAvailable => MasterMonthlyBudget - TotalSpent;
        
        public double TotalPercentage => TotalBudget > 0 ? (double)(TotalSpent / TotalBudget * 100) : 0;
        
        public double AllocatedPercentage => MasterMonthlyBudget > 0 ? (double)(TotalBudget / MasterMonthlyBudget * 100) : 0;
        
        public bool HasMasterBudget => MasterMonthlyBudget > 0;
        
        public bool IsOverAllocated => TotalBudget > MasterMonthlyBudget && MasterMonthlyBudget > 0;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddBudgetCommand { get; }
        public ICommand DeleteBudgetCommand { get; }

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
                AvailableCategories.Clear();
                
                foreach (var category in categories)
                {
                    var budget = budgets.FirstOrDefault(b => b.CategoryId == category.Id);
                    var spent = expenses.Where(e => e.CategoryId == category.Id).Sum(e => e.Amount);

                    var categoryBudget = new CategoryBudget
                    {
                        CategoryId = category.Id,
                        CategoryName = category.Name,
                        CategoryIcon = category.Icon,
                        CategoryColor = category.Color,
                        BudgetLimit = budget?.MonthlyLimit ?? 0,
                        Spent = spent,
                        Month = _currentMonth,
                        Year = _currentYear,
                        Period = "Monthly"
                    };
                    
                    // Subscribe to property changes to update stat cards
                    categoryBudget.PropertyChanged += (s, e) => UpdateStatCards();
                    
                    CategoryBudgets.Add(categoryBudget);
                    AvailableCategories.Add(category.Name);
                }
                
                UpdateStatCards();
            }
        }
        
        private void UpdateStatCards()
        {
            OnPropertyChanged(nameof(TotalBudget));
            OnPropertyChanged(nameof(TotalSpent));
            OnPropertyChanged(nameof(RemainingBudget));
            OnPropertyChanged(nameof(TotalPercentage));
            OnPropertyChanged(nameof(UnallocatedBudget));
            OnPropertyChanged(nameof(TotalAvailable));
            OnPropertyChanged(nameof(AllocatedPercentage));
            OnPropertyChanged(nameof(HasMasterBudget));
            OnPropertyChanged(nameof(IsOverAllocated));
        }
        
        private void LoadMasterBudget()
        {
            var settings = SettingsManager.LoadSettings();
            _masterMonthlyBudget = settings.MonthlyBudgetAllocation;
        }
        
        private void SaveMasterBudget()
        {
            var settings = SettingsManager.LoadSettings();
            settings.MonthlyBudgetAllocation = MasterMonthlyBudget;
            SettingsManager.SaveSettings(settings);
        }
        
        private bool CanAddBudget()
        {
            return !string.IsNullOrEmpty(SelectedCategory) && NewBudgetAmount.HasValue && NewBudgetAmount.Value > 0;
        }
        
        private async void AddNewBudget()
        {
            if (!NewBudgetAmount.HasValue) return;
            
            var existingBudget = CategoryBudgets.FirstOrDefault(cb => cb.CategoryName == SelectedCategory);
            if (existingBudget != null)
            {
                existingBudget.BudgetLimit = NewBudgetAmount.Value;
                
                // Save to database
                using (var dataService = new DataService())
                {
                    await dataService.InitializeDatabaseAsync();
                    
                    var budget = new Budget
                    {
                        CategoryId = existingBudget.CategoryId,
                        MonthlyLimit = NewBudgetAmount.Value,
                        Month = _currentMonth,
                        Year = _currentYear
                    };
                    
                    await dataService.AddOrUpdateBudgetAsync(budget);
                }
                
                MessageBox.Show($"Budget for {SelectedCategory} set to ${NewBudgetAmount.Value:N2}!", "Budget Added", 
                              MessageBoxButton.OK, MessageBoxImage.Information);
                
                UpdateStatCards();
            }
            
            // Reset form
            SelectedCategory = string.Empty;
            NewBudgetAmount = null;
        }
        
        private async Task DeleteBudget(CategoryBudget? budget)
        {
            if (budget == null) return;
            
            var result = MessageBox.Show(
                $"Are you sure you want to remove the budget for {budget.CategoryName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
                
            if (result == MessageBoxResult.Yes)
            {
                budget.BudgetLimit = 0;
                await SaveBudgetsAsync();
                await LoadBudgetsAsync();
            }
        }

        private async Task SaveBudgetsAsync()
        {
            try
            {
                using (var dataService = new DataService())
                {
                    foreach (var categoryBudget in CategoryBudgets)
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
        private string _period = "Monthly";

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

        public string Period
        {
            get => _period;
            set => SetProperty(ref _period, value);
        }

        public decimal Remaining => BudgetLimit - Spent;

        public double PercentageUsed => BudgetLimit > 0 ? (double)(Spent / BudgetLimit * 100) : 0;

        public Brush ProgressBarColor
        {
            get
            {
                if (BudgetLimit == 0) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5E5E5")); // Gray
                var percentage = PercentageUsed;
                if (percentage >= 100) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444")); // Red
                if (percentage >= 80) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B"));  // Amber
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#009689")); // Teal
            }
        }

        public string StatusText
        {
            get
            {
                if (BudgetLimit == 0) return "No budget set";
                var percentage = PercentageUsed;
                if (percentage >= 100) return "Over Budget!";
                if (percentage >= 80) return "Warning";
                return "On Track";
            }
        }
    }
}
