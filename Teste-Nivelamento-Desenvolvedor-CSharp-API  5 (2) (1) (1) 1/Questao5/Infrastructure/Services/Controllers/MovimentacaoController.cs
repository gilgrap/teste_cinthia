using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Exceptions;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/movimentacao")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PostMovimentacao([FromBody] MovimentacaoCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { IdMovimento = result });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { Error = ex.Message, Type = ex.Tipo });
            }
        }
    }

}
