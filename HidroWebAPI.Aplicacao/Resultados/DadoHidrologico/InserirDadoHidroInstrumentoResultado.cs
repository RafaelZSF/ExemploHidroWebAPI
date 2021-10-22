using HidroWebAPI.Aplicacao.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Aplicacao.Resultados.DadoHidrologico
{
    public class InserirDadoHidroInstrumentoResultado
    {
        public DadoLeituraDto[] ArrDadoLeituraInserido { get; set; }
        public DadoLeituraDto[] ArrDadoLeituraNaoInserido { get; set; }
    }
}
