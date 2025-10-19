using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Transactions.TopUp.Commands.TopUpWallet;
using Wallet.Application.Transactions.TopUp.Queries.GetTransactionById;

namespace Wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Processes a wallet top-up request.
        /// </summary>
        /// <param name="command">The command containing the details of the wallet top-up operation to perform. Must not be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the top-up operation.</param>
        /// <returns></returns>
        [HttpPost("topup")]
        public async Task<IActionResult> TopUp([FromBody] TopUpWalletCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves detailed information for a specific transaction by its unique id 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("transaction-info/{transactionId:guid}")]
        public async Task<IActionResult> GetTransactionById(Guid transactionId, CancellationToken cancellationToken)
        {
            var query = new GetTransactionByIdQuery(transactionId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
