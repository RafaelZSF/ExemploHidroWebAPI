using HidroWebAPI.Aplicacao.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HidroWebAPI.Models.DadoHidrologico
{
    public class InserirDadoHidrologicoInput
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public EnvioDadoHidrologicoDto[] ArrDadoHidrologico { get; set; }
    }
}
