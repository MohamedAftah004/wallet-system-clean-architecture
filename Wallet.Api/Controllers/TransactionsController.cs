using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Transactions.Payments.Commands.MakePayment;
using Wallet.Application.Transactions.Payments.Queries.GetWalletBalance;
using Wallet.Application.Transactions.Refunds.Commands.RefundTransaction;
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


        /// <summary>
        /// Processes a wallet payment reqoest.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("payment")]
        public async Task<IActionResult> MakePayment([FromBody] MakePaymentCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new MakePaymentCommand(command.WalletId, command.Amount, command.Description), cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves the current balance of a specific wallet byy it,s id
        /// </summary>
        /// <param name="walletId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{walletId}/balance")]
        public async Task<IActionResult> GetBalance(Guid walletId, CancellationToken cancellationToken)
        {
            var query = new GetWalletBalanceQuery(walletId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Processes a refund for the specified transaction.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("refund/{transactionId:guid}")]
        public async Task<IActionResult> RefundTransaction(Guid transactionId , CancellationToken cancellationToken)
        {
            await _mediator.Send(new RefundTransactionCommand(transactionId), cancellationToken);
            return Ok(new { Message = "Refund processed successfully." });
        }


    }
}
