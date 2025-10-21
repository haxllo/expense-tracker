using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ExpenseTracker.Helpers;
using ExpenseTracker.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace ExpenseTracker.ViewModels
{
    public class CategoryLegendItem
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public double Percentage { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class BudgetComparisonItem
    {
        public string Category { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal Spent { get; set; }
        public double Percentage { get; set; }
        public string BarColor { get; set; } = "#009689";
        public double BarWidth { get; set; }
        public string PercentageColor { get; set; } = "#737373";
    }

    public class ReportsViewModel : ViewModelBase
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private IEnumerable<ISeries> _categoryPieChart;
        private IEnumerable<ISeries> _monthlyBarChart;
        private IEnumerable<ISeries> _dailyLineChart;
        private ObservableCollection<string> _monthlyLabels;
        private ObservableCollection<string> _dailyLabels;
        private IEnumerable<LiveChartsCore.Kernel.Sketches.ICartesianAxis> _monthlyXAxis;
        private IEnumerable<LiveChartsCore.Kernel.Sketches.ICartesianAxis> _dailyXAxis;
        private decimal _totalAmount;
        private decimal _averageDaily;
        private string _topCategory;
        private ObservableCollection<CategoryLegendItem> _categoryLegendItems;
        private ObservableCollection<BudgetComparisonItem> _budgetComparisonItems;

        public ReportsViewModel()
        {
            _startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _endDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            _categoryPieChart = Array.Empty<ISeries>();
            _monthlyBarChart = Array.Empty<ISeries>();
            _dailyLineChart = Array.Empty<ISeries>();
            _monthlyLabels = new ObservableCollection<string>();
            _dailyLabels = new ObservableCollection<string>();
            _monthlyXAxis = new[] { new Axis { Labels = _monthlyLabels } };
            _dailyXAxis = new[] { new Axis { Labels = _dailyLabels } };
            _topCategory = "N/A";
            _categoryLegendItems = new ObservableCollection<CategoryLegendItem>();
            _budgetComparisonItems = new ObservableCollection<BudgetComparisonItem>();

            RefreshCommand = new RelayCommand(async _ => await LoadChartsDataAsync());
            
            _ = LoadChartsDataAsync();
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (SetProperty(ref _startDate, value))
                {
                    _ = LoadChartsDataAsync();
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (SetProperty(ref _endDate, value))
                {
                    _ = LoadChartsDataAsync();
                }
            }
        }

        public IEnumerable<ISeries> CategoryPieChart
        {
            get => _categoryPieChart;
            set => SetProperty(ref _categoryPieChart, value);
        }

        public IEnumerable<ISeries> MonthlyBarChart
        {
            get => _monthlyBarChart;
            set => SetProperty(ref _monthlyBarChart, value);
        }

        public IEnumerable<ISeries> DailyLineChart
        {
            get => _dailyLineChart;
            set => SetProperty(ref _dailyLineChart, value);
        }

        public ObservableCollection<string> MonthlyLabels
        {
            get => _monthlyLabels;
            set => SetProperty(ref _monthlyLabels, value);
        }

        public ObservableCollection<string> DailyLabels
        {
            get => _dailyLabels;
            set => SetProperty(ref _dailyLabels, value);
        }

        public IEnumerable<LiveChartsCore.Kernel.Sketches.ICartesianAxis> MonthlyXAxis
        {
            get => _monthlyXAxis;
            set => SetProperty(ref _monthlyXAxis, value);
        }

        public IEnumerable<LiveChartsCore.Kernel.Sketches.ICartesianAxis> DailyXAxis
        {
            get => _dailyXAxis;
            set => SetProperty(ref _dailyXAxis, value);
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set => SetProperty(ref _totalAmount, value);
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

        public ObservableCollection<CategoryLegendItem> CategoryLegendItems
        {
            get => _categoryLegendItems;
            set => SetProperty(ref _categoryLegendItems, value);
        }

        public ObservableCollection<BudgetComparisonItem> BudgetComparisonItems
        {
            get => _budgetComparisonItems;
            set => SetProperty(ref _budgetComparisonItems, value);
        }

        public ICommand RefreshCommand { get; }

        private async Task LoadChartsDataAsync()
        {
            using (var dataService = new DataService())
            {
                var expenses = await dataService.GetExpensesAsync(StartDate, EndDate);

                if (!expenses.Any())
                {
                    CategoryPieChart = Array.Empty<ISeries>();
                    MonthlyBarChart = Array.Empty<ISeries>();
                    DailyLineChart = Array.Empty<ISeries>();
                    CategoryLegendItems = new ObservableCollection<CategoryLegendItem>();
                    TotalAmount = 0;
                    AverageDaily = 0;
                    TopCategory = "No Data";
                    return;
                }

                // Category Pie Chart
                var categoryData = expenses
                    .GroupBy(e => e.Category?.Name ?? "Unknown")
                    .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
                    .OrderByDescending(x => x.Total)
                    .ToList();

                var totalExpenses = categoryData.Sum(x => x.Total);
                
                // Define colors for categories
                var categoryColors = new Dictionary<string, string>
                {
                    { "Food & Dining", "#FF6B35" },      // Orange
                    { "Transportation", "#4A90E2" },     // Blue
                    { "Shopping", "#9B59B6" },           // Purple
                    { "Entertainment", "#E91E63" },      // Pink
                    { "Bills & Utilities", "#F59E0B" },  // Amber
                    { "Healthcare", "#EF4444" },         // Red
                    { "Education", "#10B981" },          // Green
                    { "Others", "#8F8F8F" },             // Grey
                    { "Unknown", "#CCCCCC" }             // Light Grey
                };

                var pieSeriesList = new List<ISeries>();
                var legendItems = new List<CategoryLegendItem>();
                int colorIndex = 0;
                var defaultColors = new[] { "#FF6B35", "#4A90E2", "#9B59B6", "#E91E63", "#F59E0B", "#EF4444", "#10B981", "#8F8F8F" };
                
                foreach (var item in categoryData)
                {
                    var color = categoryColors.ContainsKey(item.Category) 
                        ? categoryColors[item.Category] 
                        : defaultColors[colorIndex % defaultColors.Length];
                    
                    pieSeriesList.Add(new PieSeries<double>
                    {
                        Name = item.Category,
                        Values = new[] { (double)item.Total },
                        DataLabelsSize = 14,
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        DataLabelsFormatter = point => $"${point.PrimaryValue:N0}",
                        Fill = new SolidColorPaint(SKColor.Parse(color))
                    });
                    
                    legendItems.Add(new CategoryLegendItem
                    {
                        Category = item.Category,
                        Amount = item.Total,
                        Percentage = totalExpenses > 0 ? (double)(item.Total / totalExpenses * 100) : 0,
                        Color = color
                    });
                    
                    colorIndex++;
                }
                CategoryPieChart = pieSeriesList;
                CategoryLegendItems = new ObservableCollection<CategoryLegendItem>(legendItems);

                // Monthly Bar Chart (last 6 months)
                var sixMonthsAgo = DateTime.Now.AddMonths(-6);
                var monthlyData = expenses
                    .Where(e => e.Date >= sixMonthsAgo)
                    .GroupBy(e => new { e.Date.Year, e.Date.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Total = g.Sum(e => e.Amount)
                    })
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    .ToList();

                MonthlyLabels.Clear();
                foreach (var m in monthlyData)
                {
                    MonthlyLabels.Add(new DateTime(m.Year, m.Month, 1).ToString("MMM yyyy"));
                }

                MonthlyBarChart = new List<ISeries>
                {
                    new ColumnSeries<double>
                    {
                        Name = "Monthly Expenses",
                        Values = monthlyData.Select(m => (double)m.Total).ToArray(),
                        DataLabelsSize = 12,
                        DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                        DataLabelsFormatter = point => $"${point.PrimaryValue:N0}",
                        Fill = new SolidColorPaint(SKColors.DeepSkyBlue),
                        MaxBarWidth = 50
                    }
                };

                // Daily Line Chart (last 30 days)
                var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                var dailyData = new List<(DateTime Date, decimal Total)>();
                
                for (var date = thirtyDaysAgo.Date; date <= DateTime.Now.Date; date = date.AddDays(1))
                {
                    var total = expenses
                        .Where(e => e.Date.Date == date)
                        .Sum(e => e.Amount);
                    dailyData.Add((date, total));
                }

                DailyLabels.Clear();
                foreach (var d in dailyData)
                {
                    DailyLabels.Add(d.Date.ToString("MM/dd"));
                }

                DailyLineChart = new List<ISeries>
                {
                    new LineSeries<double>
                    {
                        Name = "Daily Expenses",
                        Values = dailyData.Select(d => (double)d.Total).ToArray(),
                        GeometrySize = 8,
                        LineSmoothness = 0.5,
                        Fill = null,
                        Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = 3 }
                    }
                };

                // Summary Statistics
                TotalAmount = expenses.Sum(e => e.Amount);
                var days = (EndDate - StartDate).Days + 1;
                AverageDaily = days > 0 ? TotalAmount / days : 0;
                TopCategory = categoryData.FirstOrDefault()?.Category ?? "N/A";

                // Budget vs Actual
                await dataService.InitializeDatabaseAsync();
                var budgets = await dataService.GetBudgetsAsync(StartDate.Month, StartDate.Year);
                var budgetItems = new List<BudgetComparisonItem>();

                foreach (var budget in budgets.Where(b => b.MonthlyLimit > 0))
                {
                    var categoryExpenses = expenses
                        .Where(e => e.CategoryId == budget.CategoryId)
                        .Sum(e => e.Amount);

                    var percentage = budget.MonthlyLimit > 0 
                        ? (double)(categoryExpenses / budget.MonthlyLimit * 100) 
                        : 0;

                    var barColor = percentage > 100 ? "#EF4444" : 
                                   percentage > 80 ? "#F59E0B" : 
                                   "#009689";

                    var percentageColor = percentage > 100 ? "#EF4444" : "#737373";

                    budgetItems.Add(new BudgetComparisonItem
                    {
                        Category = budget.Category?.Name ?? "Unknown",
                        Budget = budget.MonthlyLimit,
                        Spent = categoryExpenses,
                        Percentage = percentage,
                        BarColor = barColor,
                        BarWidth = Math.Min(percentage, 100) * 4,
                        PercentageColor = percentageColor
                    });
                }

                BudgetComparisonItems = new ObservableCollection<BudgetComparisonItem>(budgetItems.OrderByDescending(b => b.Spent));
            }
        }
    }
}
