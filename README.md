# Expense Tracker - WPF Application

Short description: A desktop app to track personal expenses, manage budgets, and view spending reports.

A modern, feature-rich expense tracking application built with C# and WPF using the MVVM pattern.

## Features

- 💰 **Track Expenses**: Add, edit, and delete expenses with detailed information
- 📊 **Categories**: Organize expenses into predefined categories (Food, Transport, Shopping, etc.)
- 🔍 **Search & Filter**: Search expenses by description or category, filter by date range
- 📈 **Summary Dashboard**: View total expenses and monthly totals at a glance
- 💾 **Local Database**: All data stored securely in SQLite database
- 🎨 **Modern UI**: Clean, intuitive interface with Material Design-inspired elements

## Project Structure

```
ExpenseTracker/
├── Models/              # Data models (Expense, Category, Budget)
├── ViewModels/          # MVVM ViewModels with business logic
├── Views/               # XAML views and windows
├── Services/            # Data access layer and database context
├── Helpers/             # Utility classes (ViewModelBase, RelayCommand, Converters)
├── App.xaml            # Application entry point
└── MainWindow.xaml     # Main application window
```

## Technologies Used

- **Framework**: .NET 8.0 (Windows)
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Pattern**: MVVM (Model-View-ViewModel)
- **Database**: SQLite with Entity Framework Core
- **ORM**: Entity Framework Core 8.0

## Prerequisites

- **.NET 8.0 SDK** or later
- **Visual Studio 2022** (recommended) or any .NET-compatible IDE
- **Windows 10/11**

## Setup Instructions

### 1. Install .NET SDK

Download and install the .NET 8.0 SDK from:
https://dotnet.microsoft.com/download/dotnet/8.0

### 2. Clone or Download the Project

```bash
cd C:\Projects\expence-tracker
```

### 3. Restore NuGet Packages

```bash
cd ExpenseTracker
dotnet restore
```

### 4. Build the Project

```bash
dotnet build
```

### 5. Run the Application

```bash
dotnet run
```

Or open `ExpenseTracker.csproj` in Visual Studio and press F5 to run.

## Usage

### Adding an Expense

1. Click the **"➕ Add Expense"** button
2. Fill in the required fields:
   - **Amount**: Enter the expense amount
   - **Category**: Select from predefined categories
   - **Description**: Optional description
   - **Date**: Select the expense date
   - **Payment Method**: Choose payment method (Cash, Credit Card, etc.)
3. Click **"💾 Save"** to save the expense

### Editing an Expense

1. Select an expense from the list
2. Click the **"✏️ Edit"** button
3. Modify the fields as needed
4. Click **"💾 Save"** to update

### Deleting an Expense

1. Select an expense from the list
2. Click the **"🗑️ Delete"** button

### Filtering and Searching

- Use the **search box** to find expenses by description or category
- Use the **date pickers** to filter expenses by date range
- Click **"🔄 Refresh"** to reload all data

## Database

The application uses SQLite for data storage. The database file is automatically created at:

```
C:\Users\[YourUsername]\AppData\Local\ExpenseTracker\expenses.db
```

### Default Categories

- 🍔 Food & Dining
- 🚗 Transportation
- 🛍️ Shopping
- 🎮 Entertainment
- 💡 Bills & Utilities
- ⚕️ Healthcare
- 📚 Education
- 💰 Others

## Architecture

### MVVM Pattern

The application follows the MVVM (Model-View-ViewModel) pattern:

- **Models**: Data entities (Expense, Category, Budget)
- **Views**: XAML UI files (MainWindow, AddExpenseView)
- **ViewModels**: Business logic and data binding (MainViewModel, AddExpenseViewModel)

### Data Layer

- **DatabaseContext**: Entity Framework DbContext for database operations
- **IDataService/DataService**: Repository pattern for data access
- Async/await for all database operations

## Future Enhancements

- 📊 Charts and statistics visualization
- 📅 Recurring expenses
- 💰 Multiple accounts/wallets
- 🌍 Multi-currency support
- 💾 Backup and restore functionality
- 🌙 Dark mode theme
- 📤 Export to Excel/CSV
- 📱 Budget alerts and notifications

## Contributing

Feel free to fork this project and submit pull requests for any improvements!

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

For questions or support, please open an issue in the repository.
