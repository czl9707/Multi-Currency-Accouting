namespace Accountant.Controllers;

using Accountant.Models;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class AccountantController : ControllerBase
{
    private readonly ILogger<AccountantController> _logger;
    private readonly IContext _context;

    public AccountantController(
        ILogger<AccountantController> logger,
        IContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost("ExpenseRecord")]
    public async Task<IActionResult> AddNewExpenseRecord()
    {
        await _context

        return new StatusCodeResult(404);
    }
    
    [HttpGet("ExpenseRecord")]
    public async Task<iActionResult> GetExpenseRecordByID([FromBody] long cashFlowID){

    }
}