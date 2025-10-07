using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public interface IDataService : IDisposable
    {
        Task<List<Expense>> GetExpensesAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Expense?> GetExpenseByIdAsync(int id);
        Task<Expense> AddExpenseAsync(Expense expense);
        Task<Expense> UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int id);

        Task<List<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> AddCategoryAsync(Category category);

        Task<List<Budget>> GetBudgetsAsync(int? month = null, int? year = null);
        Task<Budget> AddOrUpdateBudgetAsync(Budget budget);

        Task<decimal> GetTotalExpensesAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Dictionary<string, decimal>> GetExpensesByCategoryAsync(DateTime? startDate = null, DateTime? endDate = null);
        
        Task InitializeDatabaseAsync();
    }
}
