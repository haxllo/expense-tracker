using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public class DataService : IDataService
    {
        private readonly DatabaseContext _context;

        public DataService()
        {
            _context = new DatabaseContext();
        }

        public async Task InitializeDatabaseAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task<List<Expense>> GetExpensesAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Expenses.AsNoTracking().Include(e => e.Category).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(e => e.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.Date <= endDate.Value);

            return await query.OrderByDescending(e => e.Date).ToListAsync();
        }

        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses.Include(e => e.Category)
                                         .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            expense.CreatedAt = DateTime.Now;
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<Expense> UpdateExpenseAsync(Expense expense)
        {
            expense.UpdatedAt = DateTime.Now;
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<Budget>> GetBudgetsAsync(int? month = null, int? year = null)
        {
            var query = _context.Budgets.Include(b => b.Category).AsQueryable();

            if (month.HasValue)
                query = query.Where(b => b.Month == month.Value);

            if (year.HasValue)
                query = query.Where(b => b.Year == year.Value);

            return await query.ToListAsync();
        }

        public async Task<Budget> AddOrUpdateBudgetAsync(Budget budget)
        {
            var existing = await _context.Budgets
                .FirstOrDefaultAsync(b => b.CategoryId == budget.CategoryId 
                                       && b.Month == budget.Month 
                                       && b.Year == budget.Year);

            if (existing != null)
            {
                existing.MonthlyLimit = budget.MonthlyLimit;
                _context.Budgets.Update(existing);
            }
            else
            {
                budget.CreatedAt = DateTime.Now;
                _context.Budgets.Add(budget);
            }

            await _context.SaveChangesAsync();
            return existing ?? budget;
        }

        public async Task<decimal> GetTotalExpensesAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Expenses.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(e => e.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.Date <= endDate.Value);

            var expenses = await query.ToListAsync();
            return expenses.Sum(e => e.Amount);
        }

        public async Task<Dictionary<string, decimal>> GetExpensesByCategoryAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Expenses.Include(e => e.Category).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(e => e.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.Date <= endDate.Value);

            var expenses = await query.ToListAsync();
            return expenses
                .GroupBy(e => e.Category!.Name)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
