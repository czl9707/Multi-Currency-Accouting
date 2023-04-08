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
    
    [HttpGet("ExpenseRecord/{id}")]
    public async Task<IActionResult> GetExpenseRecordByID(int id)
    {
        if (id < 0) return BadRequest("ID should be greater than zero!");
        return new JsonResult(
            await _context.GetCashFlowRecordByIDAsync<Expense>(id)
        );
    }

    [HttpGet("ExpenseRecord")]
    public async Task<IActionResult> GetExpenseRecordByRange([FromBody] dynamic timeRange)
    {
        DateTime start = DateTime.MinValue, end = DateTime.MinValue;
        try{
            start = timeRange.StartTime;
            end = timeRange.EndTime;
        } 
        catch (Exception)
        {
            return BadRequest("Invalid time range!");
        }

        if (start == DateTime.MinValue || end == DateTime.MinValue)
            return BadRequest("Invalid time range!");

        return new JsonResult(
            await _context.GetCashFlowRecordsByTimeSpanAsync<Expense>(start, end)
        );
    }

    [HttpPost("ExpenseRecord")]
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
            await _context.AddNewCashFlowRecordAsync<Expense>(record);
        }
        catch
        {
            return Forbid("DB reject!");
        }

        return Ok("Inserted new ExpenseRecord");
    }

    [HttpDelete("ExpenseRecord/{id}")]
    public async Task<IActionResult> DeleteExpenseRecordByID([FromBody] long id)
    {
        if (id < 0) return BadRequest("ID should be greater than zero!");
        await _context.DeleteCashFlowRecordAsync<Expense>(id);
        return Ok($"Deleted ExpenseRecord with ID {id}");
    }

    [HttpPut("ExpenseRecord")]
    public async Task<IActionResult> UpdateExpenseRecord([FromBody] ExpenseRecord record)
    {
        if (record.CashFlowId < 0) return BadRequest("ID should be greater than zero!");
        try 
        {
            await _context.UpdateCashFlowRecordAsync<Expense>(record);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated ExpenseRecord with ID {record.CashFlowId}");
    }

    [HttpGet("ExpenseRecord/{id}")]
    public async Task<IActionResult> GetIncomeRecordByID(int id)
    {
        if (id < 0) return BadRequest("ID should be greater than zero!");
        return new JsonResult(
            await _context.GetCashFlowRecordByIDAsync<Income>(id)
        );
    }

    [HttpGet("IncomeRecord")]
    public async Task<IActionResult> GetIncomeRecordByRange([FromBody] dynamic timeRange)
    {
        DateTime start = DateTime.MinValue, end = DateTime.MinValue;
        try{
            start = timeRange.StartTime;
            end = timeRange.EndTime;
        } 
        catch (Exception)
        {
            return BadRequest("Invalid time range!");
        }

        if (start == DateTime.MinValue || end == DateTime.MinValue)
            return BadRequest("Invalid time range!");

        return new JsonResult(
            await _context.GetCashFlowRecordsByTimeSpanAsync<Income>(start, end)
        );
    }

    [HttpPost("IncomeRecord")]
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
            await _context.AddNewCashFlowRecordAsync<Income>(record);
        }
        catch
        {
            return Forbid("DB reject!");
        }

        return Ok("Inserted new IncomeRecord");
    }

    [HttpDelete("IncomeRecord/{id}")]
    public async Task<IActionResult> DeleteIncomeRecordByID([FromBody] long id)
    {
        if (id < 0) return BadRequest("ID should be greater than zero!");
        await _context.DeleteCashFlowRecordAsync<Income>(id);
        return Ok($"Deleted IncomeRecord with ID {id}");
    }

    [HttpPut("IncomeRecord")]
    public async Task<IActionResult> UpdateIncomeRecord([FromBody] IncomeRecord record)
    {
        if (record.CashFlowId < 0) return BadRequest("ID should be greater than zero!");
        try 
        {
            await _context.UpdateCashFlowRecordAsync<Income>(record);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated IncomeRecord with ID {record.CashFlowId}");
    }

    [HttpGet("ExpenseType")]
    public async Task<IActionResult> GetAllExpenseTypes()
        => new JsonResult(
            await _context.GetAllCashFlowTypesAsync<Expense>().ConfigureAwait(false)
        );

    [HttpPost("ExpenseType")]
    public async Task<IActionResult> AddNewExpenseType([FromBody] ExpenseType type)
    {
        try
        {
            await _context.AddNewCashFlowTypeAsync<Expense>(type);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok("Inserted New ExpenseType!");
    }

    [HttpPut("ExpenseType")]
    public async Task<IActionResult> UpdateExpenseType([FromBody] ExpenseType type)
    {
        if (type.TypeId < 0) return BadRequest("ID should be greater or equal than zero!");

        try
        {
            await _context.UpdateCashFlowTypeAsync<Expense>(type);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated ExpenseType {type.TypeId}!");
    }

    [HttpDelete("ExpenseType")]
    public async Task<IActionResult> DeleteExpenseType([FromBody] long typeId)
    {
        if (typeId < 0) return BadRequest("ID should be greater or equal than zero!");
        
        try
        {
            await _context.DeleteCashFlowTypeAsync<Expense>(typeId);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated ExpenseType {typeId}!");
    }
    
    [HttpGet("IncomeType")]
    public async Task<IActionResult> GetAllIncomeTypes()
        => new JsonResult(
            await _context.GetAllCashFlowTypesAsync<Income>().ConfigureAwait(false)
        );

    [HttpPost("IncomeType")]
    public async Task<IActionResult> AddNewIncomeType([FromBody] IncomeType type)
    {
        try
        {
            await _context.AddNewCashFlowTypeAsync<Income>(type);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok("Inserted New IncomeType!");
    }

    [HttpPut("IncomeType")]
    public async Task<IActionResult> UpdateIncomeType([FromBody] IncomeType type)
    {
        if (type.TypeId < 0) return BadRequest("ID should be greater or equal than zero!");

        try
        {
            await _context.UpdateCashFlowTypeAsync<Income>(type);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated IncomeType {type.TypeId}!");
    }

    [HttpDelete("IncomeType")]
    public async Task<IActionResult> DeleteIncomeType([FromBody] long typeId)
    {
        if (typeId < 0) return BadRequest("ID should be greater or equal than zero!");
        
        try
        {
            await _context.DeleteCashFlowTypeAsync<Income>(typeId);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated IncomeType {typeId}!");
    }

    [HttpGet("PaymentMethod")]
    public async Task<IActionResult> GetAllPaymentMethods()
        => new JsonResult(
            await _context.GetAllMethodsAsync().ConfigureAwait(false)
        );

    [HttpPost("PaymentMethod")]
    public async Task<IActionResult> AddNewPaymentMethod([FromBody] PaymentMethod method)
    {
        try
        {
            await _context.AddNewMethodTypeAsync(method);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok("Inserted New PaymentMethod!");
    }

    [HttpPut("PaymentMethod")]
    public async Task<IActionResult> UpdatePaymentMethod([FromBody] PaymentMethod method)
    {
        if (method.MethodId < 0) return BadRequest("ID should be greater or equal than zero!");

        try
        {
            await _context.UpdateMethodTypeAsync(method);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated PaymentMethod {method.MethodId}!");
    }

    [HttpDelete("PaymentMethod")]
    public async Task<IActionResult> DeletePaymentMethod([FromBody] long methodId)
    {
        if (methodId < 0) return BadRequest("ID should be greater or equal than zero!");
        
        try
        {
            await _context.DeleteMethodTypeAsync(methodId);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated PaymentMethod {methodId}!");
    }
}