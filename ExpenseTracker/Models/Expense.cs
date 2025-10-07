using System;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
