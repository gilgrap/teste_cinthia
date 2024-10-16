using MediatR;
using Questao5.Application.Commands.Requests;
using System.Data;
using Dapper;
using Questao5.Domain.Entities;
using Questao5.Application.Exceptions;
using System.Text.Json;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoCommandHandler : IRequestHandler<MovimentacaoCommand, string>
    {
        private readonly IDbConnection _dbConnection;

        public MovimentacaoCommandHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<string> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
        {
            // Validações de negócio
            var conta = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
                new { request.IdContaCorrente });

            if (conta == null)
                throw new BusinessException("Conta não cadastrada.", "INVALID_ACCOUNT");

            if (conta.Ativo == 0)
                throw new BusinessException("Conta inativa.", "INACTIVE_ACCOUNT");

            if (request.Valor <= 0)
                throw new BusinessException("Valor inválido.", "INVALID_VALUE");

            if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                throw new BusinessException("Tipo de movimento inválido.", "INVALID_TYPE");

            // Verificar idempotência
            var idempotencia = await _dbConnection.QueryFirstOrDefaultAsync<Idempotencia>(
                "SELECT * FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia",
                new { request.ChaveIdempotencia });

            if (idempotencia != null)
                return idempotencia.Resultado;  // Retorna o resultado idempotente anterior.

            // Inserir movimento
            string idMovimento = Guid.NewGuid().ToString();
            await _dbConnection.ExecuteAsync(
                "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
                new { IdMovimento = idMovimento, request.IdContaCorrente, DataMovimento = DateTime.Now, request.TipoMovimento, request.Valor });

            // Gravar idempotência
            await _dbConnection.ExecuteAsync(
                "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) " +
                "VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)",
                new { request.ChaveIdempotencia, Requisicao = JsonSerializer.Serialize(request), Resultado = idMovimento });

            return idMovimento;
        }
    }

}
