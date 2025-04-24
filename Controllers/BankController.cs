using BankAPI.Data;
using BankAPI.DTO;
using BankAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankController : ControllerBase
{
    private readonly BankDbContext _context;

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        TransferIn,
        TransferOut
    }

    public BankController(BankDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAccount(CreateAccountDto dto)
    {
        var account = new BankAccount
        {
            Id = Guid.NewGuid(),
            OwnerName = dto.OwnerName,
            AccountNumber = Guid.NewGuid().ToString(),
            Balance = 0
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return Ok(account.AccountNumber);
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(DepositDto dto)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == dto.AccountNumber);
        if (account == null)
        {
            return NotFound();
        }

        account.Balance += dto.Amount;
        _context.Transactions.Add(new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Amount = dto.Amount,
            Type = TransactionType.Deposit.ToString(),
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(WithdrawDto dto)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == dto.AccountNumber);
        if (account == null)
        {
            return NotFound();
        }

        if (account.Balance < dto.Amount)
        {
            return BadRequest("Nedostatek prostředků.");
        }

        account.Balance -= dto.Amount;
        _context.Transactions.Add(new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Amount = -dto.Amount,
            Type = "Withdrawal",
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(TransferDto dto)
    {
        var from = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == dto.FromAccountNumber);
        var to = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == dto.ToAccountNumber);
        if (from == null || to == null)
        {
            return NotFound();
        }

        if (from.Balance < dto.Amount)
        {
            return BadRequest("Nedostatek prostředků.");
        }

        from.Balance -= dto.Amount;
        to.Balance += dto.Amount;

        var transferOut = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = from.Id,
            Amount = -dto.Amount,
            Type = "TransferOut",
            RelatedAccountId = to.Id
        };

        var transferIn = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = to.Id,
            Amount = dto.Amount,
            Type = "TransferIn",
            RelatedAccountId = from.Id
        };

        _context.Transactions.AddRange(transferOut, transferIn);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{accountNumber}")]
    public async Task<ActionResult<AccountDetailDto>> GetAccount(string accountNumber)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
        if (account == null)
        {
            return NotFound();
        }

        return new AccountDetailDto(account.AccountNumber, account.OwnerName, account.Balance);
    }

    [HttpGet("{accountNumber}/transactions")]
    public async Task<ActionResult<List<TransactionDto>>> GetTransactions(string accountNumber)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
        if (account == null)
        {
            return NotFound();
        }

        var trs = await _context.Transactions
            .Where(x => x.AccountId == account.Id)
            .OrderByDescending(x => x.Timestamp)
            .Select(t => new TransactionDto(t.Timestamp, t.Amount, t.Type, t.Note))
            .ToListAsync();

        return trs;
    }
}
