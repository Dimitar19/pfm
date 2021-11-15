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
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionKind? transactionKind, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string sortBy, [FromQuery] SortOrder sortOrder){
            page ??= 1;
            pageSize ??= 10;
            // TransactionKind? transactionKindEnum = null;
            // DateTime? startDateTime = null;
            // DateTime? endDateTime = null;
            // if (!string.IsNullOrEmpty(transactionKind))
            //     transactionKindEnum = (TransactionKind)Enum.Parse(typeof(TransactionKind), transactionKind, true);
            // if (!string.IsNullOrEmpty(startDate))
            //     startDateTime = DateTime.Parse(startDate);
            // if(!string.IsNullOrEmpty(endDate))
            //     endDateTime = DateTime.Parse(endDate);
            var pagedSortedList = await _pfmService.GetTransactions(transactionKind, startDate, endDate, page.Value, pageSize.Value, sortBy, sortOrder);
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
        public async Task<IActionResult> CategorizeTransaction([FromRoute] string id, [FromBody] TransactionCategorizeCommand command){
            var res = await _pfmService.CategorizeTransaction(id, command);
            return Ok(res);
        }

        [HttpPost("auto-categorize")]
        public IActionResult CategorizeTransactions(){
            return Ok();
        }
    }
}