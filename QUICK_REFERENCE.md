# 🚀 Quick Reference Guide

## Build & Run Commands

```bash
# Navigate to project
cd C:\Projects\expence-tracker\ExpenseTracker

# Build
dotnet build

# Run
dotnet run

# Clean build
dotnet clean && dotnet build
```

## Key Files

| File | Purpose |
|------|---------|
| `MainWindow.xaml` | Main UI layout |
| `MainViewModel.cs` | Main window logic |
| `AddExpenseView.xaml` | Add/Edit expense form |
| `AddExpenseViewModel.cs` | Add/Edit logic |
| `DataService.cs` | Database operations |
| `DatabaseContext.cs` | EF Core configuration |

## Database Location

```
C:\Users\[YourUsername]\AppData\Local\ExpenseTracker\expenses.db
```

## Default Categories

1. 🍔 Food & Dining (#FF5722)
2. 🚗 Transportation (#2196F3)
3. 🛍️ Shopping (#9C27B0)
4. 🎮 Entertainment (#E91E63)
5. 💡 Bills & Utilities (#FFC107)
6. ⚕️ Healthcare (#00BCD4)
7. 📚 Education (#673AB7)
8. 💰 Others (#607D8B)

## Common Tasks

### Add New Category
```csharp
// In DatabaseContext.cs → SeedData()
new Category { 
    Id = 9, 
    Name = "Travel", 
    Icon = "✈️", 
    Color = "#4CAF50", 
    IsDefault = true 
}
```

### Add New Property to Expense
```csharp
// 1. Add to Models/Expense.cs
public string? Notes { get; set; }

// 2. Delete database (will be recreated)
// 3. Run app
```

### Add New Command
```csharp
// In ViewModel
public ICommand MyCommand { get; }

// In constructor
MyCommand = new RelayCommand(_ => MyMethod());

// Method
private void MyMethod()
{
    // Logic here
}

// In XAML
<Button Command="{Binding MyCommand}" Content="Click Me"/>
```

## Useful Code Snippets

### Show Message Box
```csharp
MessageBox.Show("Message", "Title", 
    MessageBoxButton.OK, MessageBoxImage.Information);
```

### Async Data Loading
```csharp
private async Task LoadDataAsync()
{
    using (var service = new DataService())
    {
        var data = await service.GetDataAsync();
        // Use data
    }
}
```

### XAML Data Binding
```xaml
<!-- One-way binding -->
<TextBlock Text="{Binding PropertyName}" />

<!-- Two-way binding -->
<TextBox Text="{Binding PropertyName, UpdateSourceTrigger=PropertyChanged}" />

<!-- Command binding -->
<Button Command="{Binding CommandName}" />
```

## Keyboard Shortcuts (To Implement)

```csharp
// In MainWindow.xaml
<Window.InputBindings>
    <KeyBinding Key="N" Modifiers="Ctrl" Command="{Binding AddExpenseCommand}"/>
    <KeyBinding Key="F" Modifiers="Ctrl" Command="{Binding FocusSearchCommand}"/>
    <KeyBinding Key="R" Modifiers="Ctrl" Command="{Binding RefreshCommand}"/>
</Window.InputBindings>
```

## Debug Tips

### View Debug Output
```csharp
System.Diagnostics.Debug.WriteLine($"Value: {myValue}");
```

### Breakpoint Locations
- `AddExpenseAsync()` - Check if expense is being saved
- `LoadDataAsync()` - Check if data is being loaded
- `FilterEndDate` setter - Check date filter updates

### Check Database
```bash
# Install SQLite browser
# Open: C:\Users\[User]\AppData\Local\ExpenseTracker\expenses.db
```

## Common Errors & Fixes

| Error | Fix |
|-------|-----|
| "dotnet not recognized" | Install .NET 8.0 SDK |
| "Database locked" | Dispose DataService properly |
| "Expenses not showing" | Check FilterEndDate includes today |
| "Sum() error" | Load to memory before aggregating |

## Git Commands

```bash
# Initialize repo
git init
git add .
git commit -m "Initial commit"

# Create .gitignore (already exists)
# Commit changes
git add .
git commit -m "Add feature X"

# Check status
git status

# View changes
git diff
```

## Resources

- **Docs**: See README.md
- **Improvements**: See IMPROVEMENTS.md
- **Changelog**: See CHANGELOG.md
- **Summary**: See SUMMARY.md

## Quick Feature Examples

### 1. Add Expense Count
```csharp
// MainViewModel.cs
public int ExpenseCount => Expenses.Count;

// MainWindow.xaml
<TextBlock Text="{Binding ExpenseCount, StringFormat='Count: {0}'}" />
```

### 2. Add Clear Filters Button
```csharp
// MainViewModel.cs
ClearFiltersCommand = new RelayCommand(_ => ClearFilters());

private void ClearFilters()
{
    SearchText = string.Empty;
    FilterStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    FilterEndDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
}

// MainWindow.xaml
<Button Command="{Binding ClearFiltersCommand}" Content="Clear Filters"/>
```

### 3. Add Delete Confirmation
```csharp
// MainViewModel.cs
private async void DeleteExpense()
{
    if (SelectedExpense == null) return;

    var result = MessageBox.Show(
        $"Delete expense of ₹{SelectedExpense.Amount}?",
        "Confirm Delete",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question
    );

    if (result == MessageBoxResult.Yes)
    {
        using (var service = new DataService())
        {
            await service.DeleteExpenseAsync(SelectedExpense.Id);
        }
        await LoadDataAsync();
    }
}
```

---

**Happy Coding! 🚀**
