using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Dominio.Entidades
{
    public class DadoHidrologico
    {
        public int IdDadoHidrologico { get; set; }
        public int IdReservatorio { get; set; }
        public string NomePosto { get; set; }
        public int IdTipoDadoHidrologico { get; set; }
        public DateTime DataRegistro { get; set; }
        public decimal ValorLeitura { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DadoHidrologico dadoHidrologico &&
                   IdDadoHidrologico == dadoHidrologico.IdDadoHidrologico &&
                   IdReservatorio == dadoHidrologico.IdReservatorio &&
                   NomePosto == dadoHidrologico.NomePosto &&
                   IdTipoDadoHidrologico == dadoHidrologico.IdTipoDadoHidrologico &&
                   DataRegistro == dadoHidrologico.DataRegistro &&
                   ValorLeitura == dadoHidrologico.ValorLeitura;
        }
    }
}
