using HidroWebAPI.Aplicacao.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HidroWebAPI.Models.DadoHidrologico
{
    public class InserirDadoHidroInstrumentoOutput
    {
        public DadoLeituraDto[] ArrDadoLeituraInserido { get; set; }
        public DadoLeituraDto[] ArrDadoLeituraNaoInserido { get; set; }
    }
}
