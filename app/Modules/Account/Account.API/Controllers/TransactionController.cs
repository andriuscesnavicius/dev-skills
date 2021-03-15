namespace Account.API.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Account.Models;
    using Account.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpPost("amount")]
        [SwaggerOperation(Summary = "Creates a new transaction which updates the current account balance.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Transaction created.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Mandatory body parameters missing or have incorrect type.")]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed, Description = "Specified HTTP method not allowed.")]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, Description = "Specified content type not allowed.")]
        public async Task<ActionResult> CreateTransaction(
            [FromBody, Required] Transaction transaction,
            [FromHeader(Name = "Transaction-Id"), Required] Guid transactionId)
        {
            await transactionService.CreateTransaction(transaction, transactionId);
            return Ok();
        }

        [HttpGet("transaction/{transaction_Id:Guid}")]
        [SwaggerOperation(Summary = "Returns the transaction.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Transaction details.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Transaction not found.")]
        public async Task<ActionResult<Transaction>> GetTransaction(
            [FromRoute(Name = "transaction_Id"), Required] Guid transactionId)
        {
            var transaction = await transactionService.GetTransaction(transactionId);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpGet("balance/{account_id:Guid}")]
        [SwaggerOperation(Summary = "Returns the current account balance.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Account balance.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Account not found.")]
        public async Task<ActionResult<AccountBalance>> GetBalance(
            [FromRoute(Name = "account_id"), Required] Guid accountId)
        {
            var account = await transactionService.GetBalance(accountId);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpGet("max_transaction_volume")]
        [SwaggerOperation(Summary = "Returns accounts with the max number of transactions.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Accounts with the max number of transactions.")]
        public async Task<ActionResult<MaxTransactionVolume>> GetMaxTransactionVolume()
        {
            return Ok(await transactionService.GetMaxTransactionVolume());
        }
    }
}
