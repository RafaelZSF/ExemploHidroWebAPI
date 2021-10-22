using HidroWebAPI.Aplicacao.Dtos;
using HidroWebAPI.Aplicacao.Resultados.DadoHidrologico;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Aplicacao.Requisicoes.DadoHidrologico
{
    public class InserirDadoHidrologicoRequisicao : IRequest<InserirDadoHidrologicoResultado>
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public EnvioDadoHidrologicoDto[] ArrDadoHidrologico { get; set; }
    }
}
