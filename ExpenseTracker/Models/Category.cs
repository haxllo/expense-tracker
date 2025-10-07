using System.Collections.Generic;

namespace ExpenseTracker.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = "💰";
        public string Color { get; set; } = "#4CAF50";
        public bool IsDefault { get; set; }
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
