namespace BankAPI.Model
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public BankAccount Account { get; set; } = null!;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? Note { get; set; }

        public Guid? RelatedAccountId { get; set; }
    }
}
