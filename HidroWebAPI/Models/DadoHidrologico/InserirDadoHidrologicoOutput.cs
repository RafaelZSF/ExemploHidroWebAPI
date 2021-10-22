using HidroWebAPI.Aplicacao.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HidroWebAPI.Models.DadoHidrologico
{
    public class InserirDadoHidrologicoOutput
    {
        public DadoHidrologicoDto[] ArrDadoInserido { get; set; }
        public DadoHidrologicoDto[] ArrDadoNãoInserido { get; set; }
    }
}
