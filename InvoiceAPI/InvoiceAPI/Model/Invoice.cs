using System;
using System.Text.Json.Serialization;

namespace InvoiceAPI.Models
{
    public class Invoice
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; } = 0;
        [JsonPropertyName("due_date")]
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "pending"; // pending, paid, void
    }
}
