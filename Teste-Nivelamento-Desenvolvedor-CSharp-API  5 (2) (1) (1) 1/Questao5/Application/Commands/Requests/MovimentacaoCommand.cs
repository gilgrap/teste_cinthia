using MediatR;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoCommand : IRequest<string>
    {
        public string ChaveIdempotencia { get; set; }
        public string IdContaCorrente { get; set; }
        public string TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }

}
