using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        public int Numero { get; private set; } 
        public string Titular { get; set; }    
        public double Saldo { get; private set; } 

        private const double TaxaSaque = 3.50; 

        // Construtor com ou sem depósito inicial
        public ContaBancaria(int numero, string titular, double depositoInicial = 0.0)
        {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial; 
        }

        // R realizar depósitos
        public void Deposito(double valor)
        {
            Saldo += valor;
        }

        // Realizar saques, aplicando a taxa de saque
        public void Saque(double valor)
        {
            Saldo -= (valor + TaxaSaque);
        }

        // Exibir os dados da conta
        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo:F2}";
        }
    }
}
