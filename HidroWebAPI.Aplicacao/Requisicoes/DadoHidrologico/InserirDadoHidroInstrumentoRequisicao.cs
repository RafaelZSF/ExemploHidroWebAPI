using HidroWebAPI.Aplicacao.Dtos;
using HidroWebAPI.Aplicacao.Resultados.DadoHidrologico;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Aplicacao.Requisicoes.DadoHidrologico
{
    public class InserirDadoHidroInstrumentoRequisicao : IRequest<InserirDadoHidroInstrumentoResultado>
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public EnvioDadoInstrumentoDto[] ArrDadoInstrumento { get; set; }
    }
}
