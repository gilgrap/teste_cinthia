using Dapper;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Exceptions;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Application.Handlers
{
    public class SaldoQueryHandler : IRequestHandler<SaldoQuery, SaldoResponse>
    {
        private readonly IDbConnection _dbConnection;

        public SaldoQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<SaldoResponse> Handle(SaldoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
                new { request.IdContaCorrente });

            if (conta == null)
                throw new BusinessException("Conta não cadastrada.", "INVALID_ACCOUNT");

            if (conta.Ativo == 0)
                throw new BusinessException("Conta inativa.", "INACTIVE_ACCOUNT");

            // Calcular saldo
            var creditos = await _dbConnection.QueryAsync<decimal>(
                "SELECT SUM(valor) FROM movimento WHERE idcontacorrente = @IdContaCorrente AND tipomovimento = 'C'",
                new { request.IdContaCorrente });

            var debitos = await _dbConnection.QueryAsync<decimal>(
                "SELECT SUM(valor) FROM movimento WHERE idcontacorrente = @IdContaCorrente AND tipomovimento = 'D'",
                new { request.IdContaCorrente });

            decimal saldo = creditos.FirstOrDefault() - debitos.FirstOrDefault();

            return new SaldoResponse
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                DataConsulta = DateTime.Now,
                Saldo = saldo
            };
        }
    }

}
