namespace Questao5.Application.Commands.Responses
{
    public class SaldoResponse
    {
        public int NumeroConta { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataConsulta { get; set; }
        public decimal Saldo { get; set; }
    }
}
