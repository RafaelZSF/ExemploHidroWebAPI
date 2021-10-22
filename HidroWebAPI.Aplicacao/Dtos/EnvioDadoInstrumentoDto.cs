using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Aplicacao.Dtos
{
    public class EnvioDadoInstrumentoDto
    {
        public int IdInstrumento { get; set; }
        public DateTime DataRegistro { get; set; }
        public decimal ValorLeitura { get; set; }
        public int IdDirecaoLeituraInstrumento { get; set; }

    }
}
