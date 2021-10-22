using HidroWebAPI.Aplicacao.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HidroWebAPI.Models.DadoHidrologico
{
    public class InserirDadoHidroInstrumentoInput
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public EnvioDadoInstrumentoDto[] ArrDadoInstrumento { get; set; }
    }
}
