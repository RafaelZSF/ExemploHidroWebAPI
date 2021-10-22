using HidroWebAPI.Aplicacao.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Aplicacao.Resultados.DadoHidrologico
{
    public class InserirDadoHidrologicoResultado
    {
        public DadoHidrologicoDto[] ArrDadoInserido { get; set; }
        public DadoHidrologicoDto[] ArrDadoNãoInserido { get; set; }
    }
}
