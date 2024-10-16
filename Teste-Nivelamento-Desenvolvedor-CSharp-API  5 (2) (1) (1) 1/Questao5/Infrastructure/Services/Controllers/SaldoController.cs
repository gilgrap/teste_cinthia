using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Exceptions;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/saldo")]
    public class SaldoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaldoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{idContaCorrente}")]
        public async Task<IActionResult> GetSaldo(string idContaCorrente)
        {
            try
            {
                var saldo = await _mediator.Send(new SaldoQuery { IdContaCorrente = idContaCorrente });
                return Ok(saldo);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { Error = ex.Message, Type = ex.Tipo });
            }
        }
    }

}
