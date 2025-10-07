# Quick Start Guide

## Prerequisites Check

Before running the application, ensure you have:

1. **.NET 8.0 SDK or later** installed
   - Check by running: `dotnet --version`
   - If not installed, download from: https://dotnet.microsoft.com/download/dotnet/8.0

2. **Visual Studio 2022** (Optional but recommended)
   - Community Edition (free) is sufficient
   - Make sure ".NET desktop development" workload is installed

## Opening the Project

### Option 1: Using Visual Studio

1. Double-click `ExpenseTracker.sln` in the root directory
2. Visual Studio will automatically restore NuGet packages
3. Press `F5` to build and run the application

### Option 2: Using Command Line

```bash
# Navigate to project directory
cd C:\Projects\expence-tracker\ExpenseTracker

# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

## First Run

On first launch, the application will:
- Create a local SQLite database at: `%LocalAppData%\ExpenseTracker\expenses.db`
- Seed default expense categories
- Display the main dashboard window

## Project Structure Overview

```
ExpenseTracker/
│
├── Models/                      # Data Models
│   ├── Expense.cs              # Main expense entity
│   ├── Category.cs             # Expense categories
│   └── Budget.cs               # Budget tracking
│
├── ViewModels/                  # MVVM ViewModels
│   ├── MainViewModel.cs        # Main window logic
│   └── AddExpenseViewModel.cs  # Add/Edit expense logic
│
├── Views/                       # UI Views
│   ├── AddExpenseView.xaml     # Add/Edit expense window
│   └── AddExpenseView.xaml.cs
│
├── Services/                    # Data Layer
│   ├── DatabaseContext.cs      # EF Core DbContext
│   ├── IDataService.cs         # Data service interface
│   └── DataService.cs          # Data service implementation
│
├── Helpers/                     # Utility Classes
│   ├── ViewModelBase.cs        # Base ViewModel with INotifyPropertyChanged
│   ├── RelayCommand.cs         # ICommand implementation
│   └── Converters.cs           # Value converters for XAML
│
├── MainWindow.xaml             # Main application window
├── MainWindow.xaml.cs
├── App.xaml                    # Application entry point
└── ExpenseTracker.csproj       # Project file
```

## Key Features

### 1. Adding an Expense
- Click "➕ Add Expense" button
- Fill in: Amount, Category, Description, Date, Payment Method
- Click "💾 Save"

### 2. Editing an Expense
- Select an expense from the list
- Click "✏️ Edit"
- Modify fields
- Click "💾 Save"

### 3. Deleting an Expense
- Select an expense
- Click "🗑️ Delete"

### 4. Filtering Expenses
- Use date pickers to filter by date range
- Type in search box to search by description/category
- Click "🔄 Refresh" to reload

## Common Issues & Solutions

### Issue: "dotnet" command not found
**Solution**: Install .NET 8.0 SDK from https://dotnet.microsoft.com/download/dotnet/8.0

### Issue: Build errors about missing packages
**Solution**: Run `dotnet restore` in the ExpenseTracker folder

### Issue: Database errors on first run
**Solution**: Ensure you have write permissions to `%LocalAppData%\ExpenseTracker\`

### Issue: Application won't start
**Solution**: 
1. Check if .NET 8.0 Runtime is installed
2. Try building from command line: `dotnet build`
3. Check for error messages in output window

## Development Notes

### MVVM Pattern
- Views bind to ViewModels via DataContext
- ViewModels implement INotifyPropertyChanged
- Commands use RelayCommand pattern
- No code-behind logic (except initialization)

### Database
- SQLite with Entity Framework Core 8.0
- Code-First approach
- Automatic migrations on startup
- Database location: `%LocalAppData%\ExpenseTracker\expenses.db`

### Adding New Features

To add a new view:
1. Create XAML file in `Views/` folder
2. Create corresponding ViewModel in `ViewModels/`
3. Wire up DataContext in code-behind
4. Add navigation logic in parent ViewModel

## Building for Release

```bash
# Build in Release mode
dotnet build -c Release

# Publish as self-contained executable (includes .NET runtime)
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true

# Output will be in: bin\Release\net8.0-windows\win-x64\publish\
```

## Next Steps

1. **Customize Categories**: Modify `DatabaseContext.cs` → `SeedData()` method
2. **Add Charts**: Install LiveCharts or OxyPlot NuGet package
3. **Export Data**: Implement CSV/Excel export using EPPlus
4. **Add Reports**: Create new ReportsView for statistics
5. **Recurring Expenses**: Add RecurringExpense model and logic

## Support

For issues or questions:
1. Check the README.md for detailed documentation
2. Review the code comments
3. Check the system requirements

## License

Free for personal and educational use.
