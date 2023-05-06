namespace Accountant.Controllers;

using Accountant.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


[ApiController]
[Route("[controller]")]
public class AccountantController : ControllerBase
{
    private ILogger<AccountantController> _logger;
    private IContext _context;

    public AccountantController(
        ILogger<AccountantController> logger,
        IContext context
    )
    {
        this._logger = logger;
        this._context = context;
    }
    
    [HttpGet("record/{cashflowType}/byID")]
    public async Task<IActionResult> GetExpenseRecordByID(string cashflowType, [FromQuery(Name = "ID")] long id)
    {
        if (cashflowType != "expense" && cashflowType != "income") return BadRequest("type should be expense or income!");
        if (id < 0) return BadRequest("ID should be greater than zero!");

        return new JsonResult(
            cashflowType == "expense" ?
                await _context.GetCashFlowRecordByIDAsync<Expense>(id).ConfigureAwait(false) :
                await _context.GetCashFlowRecordByIDAsync<Income>(id).ConfigureAwait(false)
        );
    }

    [HttpGet("record/{cashflowType}/byTime")]
    public async Task<IActionResult> GetExpenseRecordByRange(
        string cashflowType, 
        [FromQuery(Name = "startDate")] DateTime startDate,
        [FromQuery(Name = "endDate")] DateTime endDate
    ){
        if (cashflowType != "expense" && cashflowType != "income") return BadRequest("type should be expense or income!");

        if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            return BadRequest("Invalid time range!");

        return new JsonResult(
            cashflowType == "expense" ?
                await _context.GetCashFlowRecordsByTimeSpanAsync<Expense>(startDate, endDate).ConfigureAwait(false):
                await _context.GetCashFlowRecordsByTimeSpanAsync<Income>(startDate, endDate).ConfigureAwait(false)
        );
    }

    [HttpDelete("record/{cashflowType}")]
    public async Task<IActionResult> DeleteExpenseRecordByID(string cashflowType, [FromBody] dynamic idObject)
    {
        long id = idObject.ID;
        if (cashflowType != "expense" && cashflowType != "income") return BadRequest("type should be expense or income!");
        if (id < 0) return BadRequest("ID should be greater than zero!");

        if (cashflowType == "expense") await _context.DeleteCashFlowRecordAsync<Expense>(id).ConfigureAwait(false);
        else await _context.DeleteCashFlowRecordAsync<Income>(id).ConfigureAwait(false);

        return Ok($"Deleted ExpenseRecord with ID {id}");
    }

    [HttpPost("record/expense")]
    public async Task<IActionResult> AddNewExpenseRecord([FromBody] ExpenseRecord record)
    {
        if (
            record.HappenUtc == default(DateTime) ||
            record.Amount == default(long) ||
            record.MethodId < 0 ||
            record.TypeId < 0 ||
            String.IsNullOrEmpty(record.CurrIso)
        ) 
            return BadRequest("Missing info to create ExpenseRecord!");

        try
        {
            await _context.AddNewCashFlowRecordAsync<Expense>(record).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!");
        }

        return Ok("Inserted new ExpenseRecord");
    }
    
    [HttpPost("record/income")]
    public async Task<IActionResult> AddNewIncomeRecord([FromBody] IncomeRecord record)
    {
        if (
            record.HappenUtc == default(DateTime) ||
            record.Amount == default(long) ||
            record.MethodId < 0 ||
            record.TypeId < 0 ||
            String.IsNullOrEmpty(record.CurrIso)
        ) 
            return BadRequest("Missing info to create IncomeRecord!");

        try
        {
            await _context.AddNewCashFlowRecordAsync<Income>(record).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!");
        }

        return Ok("Inserted new IncomeRecord");
    }

    [HttpPut("record/expense")]
    public async Task<IActionResult> UpdateExpenseRecord([FromBody] ExpenseRecord record)
    {
        if (record.CashFlowId < 0) return BadRequest("ID should be greater than zero!");
        try 
        {
            await _context.UpdateCashFlowRecordAsync<Expense>(record).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated ExpenseRecord with ID {record.CashFlowId}");
    }

    [HttpPut("record/income")]
    public async Task<IActionResult> UpdateIncomeRecord([FromBody] IncomeRecord record)
    {
        if (record.CashFlowId < 0) return BadRequest("ID should be greater than zero!");
        try 
        {
            await _context.UpdateCashFlowRecordAsync<Income>(record).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated IncomeRecord with ID {record.CashFlowId}");
    }

    [HttpGet("type/expense")]
    public async Task<IActionResult> GetAllExpenseTypes()
        => new JsonResult(
            await _context.GetAllCashFlowTypesAsync<Expense>().ConfigureAwait(false)
        );

    [HttpPost("type/expense")]
    public async Task<IActionResult> AddNewExpenseType([FromBody] ExpenseType type)
    {
        try
        {
            await _context.AddNewCashFlowTypeAsync<Expense>(type).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok("Inserted New ExpenseType!");
    }

    [HttpPut("type/expense")]
    public async Task<IActionResult> UpdateExpenseType([FromBody] ExpenseType type)
    {
        if (type.TypeId < 0) return BadRequest("ID should be greater or equal than zero!");

        try
        {
            await _context.UpdateCashFlowTypeAsync<Expense>(type).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated ExpenseType {type.TypeId}!");
    }

    [HttpDelete("type/expense")]
    public async Task<IActionResult> DeleteExpenseType([FromBody] dynamic idObject)
    {
        long typeId = idObject.ID;
        if (typeId < 0) return BadRequest("ID should be greater or equal than zero!");
        
        try
        {
            await _context.DeleteCashFlowTypeAsync<Expense>(typeId).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated ExpenseType {typeId}!");
    }
    
    [HttpGet("type/income")]
    public async Task<IActionResult> GetAllIncomeTypes()
        => new JsonResult(
            await _context.GetAllCashFlowTypesAsync<Income>().ConfigureAwait(false)
        );

    [HttpPost("type/income")]
    public async Task<IActionResult> AddNewIncomeType([FromBody] IncomeType type)
    {
        try
        {
            await _context.AddNewCashFlowTypeAsync<Income>(type).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok("Inserted New IncomeType!");
    }

    [HttpPut("type/income")]
    public async Task<IActionResult> UpdateIncomeType([FromBody] IncomeType type)
    {
        if (type.TypeId < 0) return BadRequest("ID should be greater or equal than zero!");

        try
        {
            await _context.UpdateCashFlowTypeAsync<Income>(type).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated IncomeType {type.TypeId}!");
    }

    [HttpDelete("type/income")]
    public async Task<IActionResult> DeleteIncomeType([FromBody] dynamic idObject)
    {
        long typeId = idObject.ID;
        if (typeId < 0) return BadRequest("ID should be greater or equal than zero!");
        
        try
        {
            await _context.DeleteCashFlowTypeAsync<Income>(typeId).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated IncomeType {typeId}!");
    }

    [HttpGet("paymentmethod")]
    public async Task<IActionResult> GetAllPaymentMethods()
        => new JsonResult(
            await _context.GetAllMethodsAsync().ConfigureAwait(false)
        );

    [HttpPost("paymentmethod")]
    public async Task<IActionResult> AddNewPaymentMethod([FromBody] PaymentMethod method)
    {
        try
        {
            await _context.AddNewMethodTypeAsync(method).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok("Inserted New PaymentMethod!");
    }

    [HttpPut("paymentmethod")]
    public async Task<IActionResult> UpdatePaymentMethod([FromBody] PaymentMethod method)
    {
        if (method.MethodId < 0) return BadRequest("ID should be greater or equal than zero!");

        try
        {
            await _context.UpdateMethodTypeAsync(method).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated PaymentMethod {method.MethodId}!");
    }

    [HttpDelete("paymentmethod")]
    public async Task<IActionResult> DeletePaymentMethod([FromBody] dynamic idObject)
    {
        long methodId = idObject.ID;
        if (methodId < 0) return BadRequest("ID should be greater or equal than zero!");
        
        try
        {
            await _context.DeleteMethodTypeAsync(methodId).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated PaymentMethod {methodId}!");
    }

    [HttpGet("currency/all")]
    public async Task<IActionResult> GetAllCurrencies()
        => new JsonResult(
            await _context.GetAllCurrenciesAsync().ConfigureAwait(false)
        );
}