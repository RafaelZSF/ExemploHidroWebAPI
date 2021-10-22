using System;
using System.Collections.Generic;
using System.Text;
using DadoHidrologicoEntidade = HidroWebAPI.Dominio.Entidades.DadoHidrologico;

namespace HidroWebAPI.Aplicacao.Dtos
{
    public class EnvioDadoHidrologicoDto
    {
        public int IdReservatorio { get; set; }
        public string NomePosto { get; set; }
        public int IdTipoDadoHidrologico { get; set; }
        public DateTime DataRegistro { get; set; }
        public decimal ValorLeitura { get; set; }

        public EnvioDadoHidrologicoDto() { }

        public EnvioDadoHidrologicoDto(DadoHidrologicoEntidade dadoHidrologicoEntidade)
        {
            IdReservatorio = dadoHidrologicoEntidade.IdReservatorio;
            NomePosto = dadoHidrologicoEntidade.NomePosto;
            IdTipoDadoHidrologico = dadoHidrologicoEntidade.IdTipoDadoHidrologico;
            DataRegistro = dadoHidrologicoEntidade.DataRegistro;
            ValorLeitura = dadoHidrologicoEntidade.ValorLeitura;
        }
    }
}
