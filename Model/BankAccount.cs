namespace BankAPI.Model
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        public List<Transaction> Transactions { get; set; } = new();
    }
}
