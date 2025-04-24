namespace BankAPI.DTO
{
    public record CreateAccountDto(string OwnerName);
    public record AccountDetailDto(string AccountNumber, string OwnerName, decimal Balance);
    public record TransactionDto(DateTime Timestamp, decimal Amount, string Type, string? Note);

    public record DepositDto(string AccountNumber, decimal Amount);
    public record WithdrawDto(string AccountNumber, decimal Amount);
    public record TransferDto(string FromAccountNumber, string ToAccountNumber, decimal Amount);
}
