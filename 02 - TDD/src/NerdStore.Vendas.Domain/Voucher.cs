using FluentValidation.Results;
using NerdStore.Vendas.Domain.Validations;
using System;

namespace NerdStore.Vendas.Domain
{
    public class Voucher
    {
        public string Codigo { get; private set; }
        public decimal? PercentualDesconto { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }

        public Voucher(string codigo, 
                       decimal? percentualDesconto, 
                       decimal? valorDesconto, 
                       int quantidade, 
                       DateTime dataValidade, 
                       bool ativo, bool utilizado, 
                       TipoDescontoVoucher tipoDesconto)
        {
            Codigo = codigo;
            PercentualDesconto = percentualDesconto;
            ValorDesconto = valorDesconto;
            Quantidade = quantidade;
            DataValidade = dataValidade;
            Ativo = ativo;
            Utilizado = utilizado;
            TipoDescontoVoucher = tipoDesconto;
        }


        public ValidationResult ValidarSeAplicavel()
        {
            return new VoucherAplicavelValidation().Validate(this);
        }
    }
}
