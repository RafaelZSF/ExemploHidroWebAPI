using HidroWebAPI.Aplicacao.Repositorios;
using HidroWebAPI.Aplicacao.Requisicoes.DadoHidrologico;
using HidroWebAPI.Aplicacao.Resultados.DadoHidrologico;
using HidroWebAPI.Util.Criptografia;
using HidroWebAPI.Util.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UsuarioEntidade = HidroWebAPI.Dominio.Entidades.Usuario;
using InstrumentoEntidade = HidroWebAPI.Dominio.Entidades.Instrumento;
using LeituraEntidade = HidroWebAPI.Dominio.Entidades.Leitura;
using BarragemEntidade = HidroWebAPI.Dominio.Entidades.Barragem;
using System.Linq;
using HidroWebAPI.Aplicacao.Dtos;

namespace HidroWebAPI.Aplicacao.Executores.DadoHidrologico
{
    public class InserirDadoHidroInstrumentoExecutor : IRequestHandler<InserirDadoHidroInstrumentoRequisicao, InserirDadoHidroInstrumentoResultado>
    {
        private readonly IUsuarioRepositorio usuarioRepositorio;
        private readonly IBarragemRepositorio barragemRepositorio;
        private readonly IInstrumentoRepositorio instrumentoRepositorio;
        private readonly ILeituraRepositorio leituraRepositorio;

        public const int EMPREENDEDOR_CHESF = 40;

        public InserirDadoHidroInstrumentoExecutor(IUsuarioRepositorio usuarioRepositorio, IBarragemRepositorio barragemRepositorio,
            IInstrumentoRepositorio instrumentoRepositorio, ILeituraRepositorio leituraRepositorio)
        {
            this.usuarioRepositorio = usuarioRepositorio;
            this.barragemRepositorio = barragemRepositorio;
            this.instrumentoRepositorio = instrumentoRepositorio;
            this.leituraRepositorio = leituraRepositorio;
        }

        public Task<InserirDadoHidroInstrumentoResultado> Handle(InserirDadoHidroInstrumentoRequisicao request, CancellationToken cancellationToken)
        {
            UsuarioEntidade usuarioEntidade = ValidaRequisicao(request);

            List<DadoLeituraDto> ListaInseridos = new List<DadoLeituraDto>();
            List<DadoLeituraDto> ListaNaoInseridos = new List<DadoLeituraDto>();

            foreach (EnvioDadoInstrumentoDto item_EnvioDadoInstrumento in request.ArrDadoInstrumento)
            {
                InsereDadaLeituraNaLista(usuarioEntidade, ListaInseridos, ListaNaoInseridos, item_EnvioDadoInstrumento);
            }

            return Task.FromResult(new InserirDadoHidroInstrumentoResultado()
            {
                ArrDadoLeituraInserido = ListaInseridos.ToArray(),
                ArrDadoLeituraNaoInserido = ListaNaoInseridos.ToArray()
            });
        }

        private UsuarioEntidade ValidaRequisicao(InserirDadoHidroInstrumentoRequisicao request)
        {
            if (request.ArrDadoInstrumento == null)
                throw new RegraDeNegocioException("Não foram enviados nenhum registro de Dados");

            UsuarioEntidade usuarioEntidade = ValidaUsuario(request);

            if (usuarioEntidade.IdEmpreendedor != EMPREENDEDOR_CHESF)
                throw new RegraDeNegocioException("Usuário não possui permissão.");

            IEnumerable<BarragemEntidade> enumBarragemEntidade = barragemRepositorio.ListarPorIdEmpreendedor(EMPREENDEDOR_CHESF);
            IEnumerable<InstrumentoEntidade> enumInstrumentoEntidade = instrumentoRepositorio.ListarPorArrIdInstrumento(request.ArrDadoInstrumento.Select(p => p.IdInstrumento).Distinct().ToArray());

            int[] ArrIdBarragem = enumBarragemEntidade.Select(p => p.IdBarragem).Distinct().ToArray();
            if (enumInstrumentoEntidade.Any(p => !ArrIdBarragem.Contains(p.IdBarragem)))
                throw new RegraDeNegocioException("O Instrumento não pertence ao empreendimento da barragem.");

            return usuarioEntidade;
        }

        private void InsereDadaLeituraNaLista(UsuarioEntidade usuarioEntidade, List<DadoLeituraDto> ListaInseridos, List<DadoLeituraDto> ListaNaoInseridos, EnvioDadoInstrumentoDto item_EnvioDadoInstrumento)
        {
            LeituraEntidade leituraEntidade;
            int idLeitura;
            InserirLeitura(usuarioEntidade, item_EnvioDadoInstrumento, out leituraEntidade, out idLeitura);

            if (idLeitura > 0)
            {
                leituraEntidade.IdLeitura = idLeitura;
                ListaInseridos.Add(new DadoLeituraDto(leituraEntidade));
            }
            else
                ListaNaoInseridos.Add(new DadoLeituraDto(leituraEntidade));
        }

        private void InserirLeitura(UsuarioEntidade usuarioEntidade, EnvioDadoInstrumentoDto item_EnvioDadoInstrumento, out LeituraEntidade leituraEntidade, out int idLeitura)
        {
            leituraEntidade = new LeituraEntidade()
            {
                idInstrumento = item_EnvioDadoInstrumento.IdInstrumento,
                DataLeitura = item_EnvioDadoInstrumento.DataRegistro,
                ValorLeitura = item_EnvioDadoInstrumento.ValorLeitura,
                Relevante = false,
                ResponsavelAlteracao = usuarioEntidade.IdUsuario,
                DataAlteracao = DateTime.Now,
                IdDirecaoLeituraInstrumento = item_EnvioDadoInstrumento.IdDirecaoLeituraInstrumento,
                IdNivelControle = null,
                DataExclusao = null,
                ReponsavelExclusao = null,
                Observacao = null,
                DataControlada = null,
                ResponsavelControle = null,
                FoiEditado = false,
                IdTipoInsercao = 5,
            };
            idLeitura = leituraRepositorio.InserirLeitura(leituraEntidade);
        }

        private UsuarioEntidade ValidaUsuario(InserirDadoHidroInstrumentoRequisicao request)
        {
            string senhaCodificada = Criptografia.Encrypt(request.Senha);

            return usuarioRepositorio.VerificaLoginSenha(request.Login, senhaCodificada);
        }
    }
}
