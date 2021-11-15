using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pfm.Models;
using pfm.Commands;
using System;
using Microsoft.AspNetCore.Http;
using pfm.Services;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace pfm.Controllers{
    [ApiController]
    [Route("/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly IPfmService _pfmService;
        public TransactionsController(ILogger<TransactionsController> logger, IPfmService pfmService){
            _logger = logger;
            _pfmService = pfmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] string transactionKind, [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string sortBy, [FromQuery] SortOrder sortOrder){
            page ??= 1;
            pageSize ??= 10;
            var pagedSortedList = await _pfmService.GetTransactions(page.Value, pageSize.Value, sortBy, sortOrder);
            return Ok(pagedSortedList);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactions([FromForm] IFormFile file)
        {
            var pom = await _pfmService.ImportTransactions(file);

            return Ok(pom);
        }

        [HttpPost("{id}/split")]
        public IActionResult SplitTransaction([FromRoute] string id, [FromBody] SplitTransactionCommand command){
            return Ok();
        }

        [HttpPost("{id}/categorize")]
        public IActionResult CategorizeTransaction([FromRoute] string id, [FromBody] TransactionCategorizeCommand command){
            var res = _pfmService.CategorizeTransaction(id, command);
            return Ok(res);
        }

        [HttpPost("auto-categorize")]
        public IActionResult CategorizeTransactions(){
            return Ok();
        }
    }
}