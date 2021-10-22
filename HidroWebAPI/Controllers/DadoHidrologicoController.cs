using HidroWebAPI.Aplicacao.Requisicoes.DadoHidrologico;
using HidroWebAPI.Aplicacao.Resultados.DadoHidrologico;
using HidroWebAPI.Models.DadoHidrologico;
using HidroWebAPI.Util.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HidroWebAPI.Controllers
{
    //[Route("api/[controller]")]
    public class DadoHidrologicoController : ApiController
    {

        /// <summary>
        /// Inserir Dados Hidrológicos 
        /// </summary>
        /// <remarks> Insere os Dados Hidrológicos enviados na request.
        ///  Exemplo de requisição:
        ///
        ///     POST 
        ///           {
        ///             "Login": "Rafael",
        ///             "Senha": "Rafael123",
        ///                 "ArrDadoHidrologico": [
        ///                    {
        ///                         "IdReservatorio": 1, 
        ///                         "NomePosto": "Posto Pimenta",
        ///                         "IdTipoDadoHidrologico": 1,
        ///                         "DataRegistro": "2021-09-02T18:40:20.111Z"
        ///                         "ValorLeitura": 118.1
        ///                     },
        ///                     {
        ///                         "IdReservatorio": 2, 
        ///                         "NomePosto": "Posto UHE Ferreira",
        ///                         "IdTipoDadoHidrologico": 2, 
        ///                         "DataRegistro": "2021-09-01T15:42:16.566Z"
        ///                         "ValorLeitura": 94.2
        ///                     },
        ///                 ]
        ///             }
        ///             
        /// Os Campos IdReservatorio e IdTipoDadoHidrologico são cadastrados previamente e enviados para possível integração com os dados hidrológicos
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Retorna uma lista dos Dados Hidrológicos que foram inseridos e uma lista dos Dados que não foram inseridos.</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Os Dados foram inseridos", typeof(IEnumerable<int>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError,"Ocorreu um erro os dados hidrológicos não foram inseridos.", Type = typeof(NotFoundResult))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "O usuário não possui permissão para essa requisição.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Os parametros da requisição são inválidos")]
        [HttpPost("api/[controller]/InserirDadoHidrologico")]
        [ProducesResponseType(typeof(InserirDadoHidrologicoOutput), StatusCodes.Status200OK)]
        public async Task<ObjectResult> InserirDadoHidrologico(InserirDadoHidrologicoInput input, CancellationToken cancellationToken)
        {
            InserirDadoHidrologicoRequisicao requisicao = new InserirDadoHidrologicoRequisicao()
            {
                Login = input.Login,
                Senha = input.Senha,
                ArrDadoHidrologico = input.ArrDadoHidrologico,
            };

            InserirDadoHidrologicoResultado resultado = await Mediator.Send(requisicao, cancellationToken);

            InserirDadoHidrologicoOutput output = new InserirDadoHidrologicoOutput()
            {
                ArrDadoInserido = resultado.ArrDadoInserido,
                ArrDadoNãoInserido = resultado.ArrDadoNãoInserido
            };

            return new ObjectResult(output)
            {
                StatusCode = StatusCodes.Status200OK,
            };
        }




        /// <summary>
        /// Inserir Dados Hidrológicos do Instrumento
        /// </summary>
        /// <remarks> Insere os Dados Hidrológicos do Instrumento enviados na request.
        ///  Exemplo de requisição:
        ///
        ///     POST 
        ///           {
        ///             "Login": "Amanda",
        ///             "Senha": "Amanda987",
        ///                 "ArrDadoInstrumento": [
        ///                    {
        ///                         "IdInstrumento": 1, 
        ///                         "DataRegistro": "2021-09-02T18:40:20.111Z",
        ///                         "ValorLeitura": 20,
        ///                         "IdDirecaoLeituraInstrumento": "100"
        ///                     },
        ///                     {
        ///                         "IdInstrumento": 2, 
        ///                         "DataRegistro": "2021-08-02T18:40:20.111Z",
        ///                         "ValorLeitura": 30,
        ///                         "IdDirecaoLeituraInstrumento": "200"
        ///                     },
        ///                 ]
        ///             }
        ///             
        /// Os Campos IdInstrumento é cadastrado previamente e enviado para possível integração com os dados hidrológicos
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Retorna uma lista dos dados do instrumento que foram inseridos</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Os Dados foram inseridos", typeof(IEnumerable<int>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Ocorreu um erro os dados hidrológicos não foram inseridos.", Type = typeof(NotFoundResult))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "O usuário não possui permissão para essa requisição.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Os parametros da requisição são inválidos")]
        [HttpPost("api/[controller]/InserirDadoHidroInstrumento")]
        [ProducesResponseType(typeof(InserirDadoHidroInstrumentoOutput), StatusCodes.Status200OK)]
        public async Task<ObjectResult> InserirDadoHidroInstrumento(InserirDadoHidroInstrumentoInput input, CancellationToken cancellationToken)
        {
            InserirDadoHidroInstrumentoRequisicao requisicao = new InserirDadoHidroInstrumentoRequisicao()
            {
                Login = input.Login,
                Senha = input.Senha,
                ArrDadoInstrumento = input.ArrDadoInstrumento,
            };

            InserirDadoHidroInstrumentoResultado resultado = await Mediator.Send(requisicao, cancellationToken);

            InserirDadoHidroInstrumentoOutput output = new InserirDadoHidroInstrumentoOutput()
            {
                ArrDadoLeituraInserido = resultado.ArrDadoLeituraInserido,
                ArrDadoLeituraNaoInserido = resultado.ArrDadoLeituraNaoInserido
            };

            return new ObjectResult(output)
            {
                StatusCode = StatusCodes.Status200OK,
            };
        }


    }
}
