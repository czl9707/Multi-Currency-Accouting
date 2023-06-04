namespace Accountant.Controllers;

using Accountant.Models;
using Accountant.Services.Middleware;
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
    public async Task<IActionResult> GetCashflowRecordByID(string cashflowType, [FromQuery(Name = "ID")] long id)
    {
        if (! IsCashFlowTypeValid(cashflowType)) return BadRequest("type should be expense or income!");
        if (id < 0) return BadRequest("ID should be greater than zero!");

        return new JsonResult(
            cashflowType == "expense" ?
                await _context.GetCashFlowRecordByIDAsync<Expense>(id).ConfigureAwait(false) :
                await _context.GetCashFlowRecordByIDAsync<Income>(id).ConfigureAwait(false)
        );
    }

    [HttpGet("record/{cashflowType}/byTime")]
    public async Task<IActionResult> GetCashflowRecordByRange(
        string cashflowType, 
        [FromQuery(Name = "startDate")] DateTime startDate,
        [FromQuery(Name = "endDate")] DateTime endDate,
        [FromQuery(Name = "type")] int? typeFilter,
        [FromQuery(Name = "method")] int? methodFilter,
        [FromQuery(Name = "currency")] string? currencyFilter
    ){
        if (! IsCashFlowTypeValid(cashflowType)) return BadRequest("type should be expense or income!");

        if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            return BadRequest("Invalid time range!");

        try 
        {
            return new JsonResult(
                cashflowType == "expense" ?
                    await _context.GetCashFlowRecordsByTimeSpanAsync<Expense>(startDate, endDate, typeFilter, methodFilter, currencyFilter).ConfigureAwait(false):
                    await _context.GetCashFlowRecordsByTimeSpanAsync<Income>(startDate, endDate, typeFilter, methodFilter, currencyFilter).ConfigureAwait(false)
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("record/{cashflowType}")]
    public async Task<IActionResult> DeleteCashflowRecordByID(string cashflowType)
    {
        if (! IsCashFlowTypeValid(cashflowType)) return BadRequest("type should be expense or income!");

        long id = cashflowType == "expense" ? (await this.ParseRequestBody<ExpenseRecord>(Request).ConfigureAwait(false)).CashFlowId :
            (await this.ParseRequestBody<IncomeRecord>(Request).ConfigureAwait(false)).CashFlowId;

        if (id < 0) return BadRequest("ID should be greater than zero!");
        
        
        if (cashflowType == "expense") await _context.DeleteCashFlowRecordAsync<Expense>(id).ConfigureAwait(false);
        else await _context.DeleteCashFlowRecordAsync<Income>(id).ConfigureAwait(false);
        return Ok($"Deleted {cashflowType} record with ID {id}");
    }

    [HttpPost("record/{cashflowType}")]
    public async Task<IActionResult> AddNewCashflowRecord(string cashflowType)
    {
        if (! IsCashFlowTypeValid(cashflowType)) return BadRequest("type should be expense or income!");

        if (cashflowType == "expense")
        {
            var record = await this.ParseRequestBody<ExpenseRecord>(Request).ConfigureAwait(false);
            if (
                record.HappenUtc == default(DateTime) ||
                record.Amount == default(long) ||
                record.MethodId < 0 ||
                record.TypeId < 0 ||
                String.IsNullOrEmpty(record.CurrIso)
            ) return BadRequest("Missing info to create ExpenseRecord!");

            await _context.AddNewCashFlowRecordAsync<Expense>(record).ConfigureAwait(false);
        }
        else
        {
            var record = await this.ParseRequestBody<IncomeRecord>(Request).ConfigureAwait(false);
            if (
                record.HappenUtc == default(DateTime) ||
                record.Amount == default(long) ||
                record.MethodId < 0 ||
                record.TypeId < 0 ||
                String.IsNullOrEmpty(record.CurrIso)
            ) return BadRequest("Missing info to create IncomeRecord!");

            await _context.AddNewCashFlowRecordAsync<Income>(record).ConfigureAwait(false);
        }

        return Ok($"Inserted new {cashflowType} record");
    }

    [HttpPut("record/{cashflowType}")]
    public async Task<IActionResult> UpdateCashflowRecord(string cashflowType)
    {
        if (! IsCashFlowTypeValid(cashflowType)) return BadRequest("type should be expense or income!");

        long cashflowId = -1;

        if (cashflowType == "expense")
        {
            ExpenseRecord record = await this.ParseRequestBody<ExpenseRecord>(Request);
            cashflowId = record.CashFlowId;
            if (record.CashFlowId < 0) return BadRequest("ID should be greater than zero!");
            try
            {
                await _context.UpdateCashFlowRecordAsync<Expense>(record).ConfigureAwait(false);
            }
            catch
            {
                return Forbid("DB reject!"); 
            }
        }
        else
        {
            IncomeRecord record = await this.ParseRequestBody<IncomeRecord>(Request);
            cashflowId = record.CashFlowId;
            if (record.CashFlowId < 0) return BadRequest("ID should be greater than zero!");
            try
            {
                await _context.UpdateCashFlowRecordAsync<Income>(record).ConfigureAwait(false);
            }
            catch
            {
                return Forbid("DB reject!"); 
            }
        }

        return Ok($"Updated {cashflowType} record with ID {cashflowId}");
    }

    [HttpGet("type/{cashflowType}")]
    public async Task<IActionResult> GetAllCashflowTypes(string cashflowType)
    {
        if (! IsCashFlowTypeValid(cashflowType)) return BadRequest("type should be expense or income!");
        return new JsonResult(
            cashflowType == "expense" ?
            await _context.GetAllCashFlowTypesAsync<Expense>().ConfigureAwait(false) :
            await _context.GetAllCashFlowTypesAsync<Income>().ConfigureAwait(false)
        );
    }

        

    [HttpPost("type/{cashflowType}")]
    public async Task<IActionResult> AddNewCashflowType(string cashflowType)
    {
        if (! IsCashFlowTypeValid(cashflowType)) return BadRequest("type should be expense or income!");

        try
        {
            if (cashflowType == "expense")
            {
                await _context.AddNewCashFlowTypeAsync<Expense>(
                    await this.ParseRequestBody<ExpenseType>(Request).ConfigureAwait(false)
                ).ConfigureAwait(false);
            }
            else
            {
                await _context.AddNewCashFlowTypeAsync<Income>(
                    await this.ParseRequestBody<IncomeType>(Request).ConfigureAwait(false)
                ).ConfigureAwait(false);
            }
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Inserted New {cashflowType} type!");
    }

    [HttpPut("type/{cashflowType}")]
    public async Task<IActionResult> UpdateCashflowType(string cashflowType)
    {
        if (! IsCashFlowTypeValid(cashflowType)) return BadRequest("type should be expense or income!");

        long typeId = -1;

        if (cashflowType == "expense")
        {
            ExpenseType type = await this.ParseRequestBody<ExpenseType>(Request).ConfigureAwait(false);
            typeId = type.TypeId;
            if (type.TypeId < 0) return BadRequest("ID should be greater or equal than zero!");
            try
            {
                await _context.UpdateCashFlowTypeAsync<Expense>(type).ConfigureAwait(false);
            }
            catch
            {
                return Forbid("DB reject!"); 
            }
        }
        else
        {
            IncomeType type = await this.ParseRequestBody<IncomeType>(Request).ConfigureAwait(false);
            typeId = type.TypeId;
            if (type.TypeId < 0) return BadRequest("ID should be greater or equal than zero!");
            try
            {
                await _context.UpdateCashFlowTypeAsync<Income>(type).ConfigureAwait(false);
            }
            catch
            {
                return Forbid("DB reject!"); 
            }
        }


        return Ok($"Updated {cashflowType} Type {typeId}!");
    }

    [HttpDelete("type/{cashflowType}")]
    public async Task<IActionResult> DeleteCashflowType(string cashflowType)
    {
        if (! IsCashFlowTypeValid(cashflowType)) return BadRequest("type should be expense or income!");

        long typeId = cashflowType == "expense" ? (await this.ParseRequestBody<ExpenseType>(Request).ConfigureAwait(false)).TypeId :
            (await this.ParseRequestBody<IncomeType>(Request).ConfigureAwait(false)).TypeId;

        if (typeId < 0) return BadRequest("ID should be greater or equal than zero!");
        
        try
        {
            if (cashflowType == "expense") await _context.DeleteCashFlowTypeAsync<Expense>(typeId).ConfigureAwait(false);
            else await _context.DeleteCashFlowTypeAsync<Income>(typeId).ConfigureAwait(false);
        }
        catch
        {
            return Forbid("DB reject!"); 
        }

        return Ok($"Updated {cashflowType} type {typeId}!");
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

    private bool IsCashFlowTypeValid(string cashflowType)
        => ! (cashflowType != "expense" && cashflowType != "income");
    
    private async Task<T> ParseRequestBody<T>(HttpRequest request)
    {
        string requestString = await RequestBodyBuffering.GetRawBodyAsync(request);
        
        var body = JsonSerializer.Deserialize<T>(requestString);
        return body ?? throw new ArgumentException("Cannot parse request body");
    }
}