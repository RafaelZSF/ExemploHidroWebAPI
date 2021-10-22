using HidroWebAPI.Aplicacao.Dtos;
using HidroWebAPI.Aplicacao.Repositorios;
using HidroWebAPI.Aplicacao.Requisicoes.DadoHidrologico;
using HidroWebAPI.Aplicacao.Resultados.DadoHidrologico;
using HidroWebAPI.Util.Criptografia;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UsuarioEntidade = HidroWebAPI.Dominio.Entidades.Usuario;
using DadoHidrologicoEntidade = HidroWebAPI.Dominio.Entidades.DadoHidrologico;
using TipoDadoHidrologicoEntidade = HidroWebAPI.Dominio.Entidades.TipoDadoHidrologico;
using ReservatorioHidroEntidade = HidroWebAPI.Dominio.Entidades.ReservatorioHidro;
using LeituraEntidade = HidroWebAPI.Dominio.Entidades.Leitura;
using System.Linq;
using HidroWebAPI.Util.Exceptions;

namespace HidroWebAPI.Aplicacao.Executores.DadoHidrologico
{
    public class InserirDadoHidrologicoExecutor : IRequestHandler<InserirDadoHidrologicoRequisicao, InserirDadoHidrologicoResultado>
    {
        private readonly IDadoHidrologicoRepositorio dadoHidrologicoRepositorio;
        private readonly IUsuarioRepositorio usuarioRepositorio;
        private readonly ITipoDadoHidrologicoRepositorio tipoDadoHidrologicoRepositorio;
        private readonly IReservatorioHidroRepositorio reservatorioHidroRepositorio;
        private readonly ILeituraRepositorio leituraRepositorio;

        public InserirDadoHidrologicoExecutor(IDadoHidrologicoRepositorio dadoHidrologicoRepositorio, IUsuarioRepositorio usuarioRepositorio,
            ITipoDadoHidrologicoRepositorio tipoDadoHidrologicoRepositorio, IReservatorioHidroRepositorio reservatorioHidroRepositorio,
            ILeituraRepositorio leituraRepositorio)
        {
            this.dadoHidrologicoRepositorio = dadoHidrologicoRepositorio;
            this.usuarioRepositorio = usuarioRepositorio;
            this.tipoDadoHidrologicoRepositorio = tipoDadoHidrologicoRepositorio;
            this.reservatorioHidroRepositorio = reservatorioHidroRepositorio;
            this.leituraRepositorio = leituraRepositorio;
        }

        public Task<InserirDadoHidrologicoResultado> Handle(InserirDadoHidrologicoRequisicao request, CancellationToken cancellationToken)
        {
            (IEnumerable<ReservatorioHidroEntidade> enumReservatorioHidroEntidade, UsuarioEntidade usuarioEntidade) retornoValidacao = ValidaRequisicao(request);

            List<DadoHidrologicoDto> ListaInseridos = new List<DadoHidrologicoDto>();
            List<DadoHidrologicoDto> ListaNaoInseridos = new List<DadoHidrologicoDto>();

            foreach (EnvioDadoHidrologicoDto item_DadoHidrologicoDto in request.ArrDadoHidrologico)
            {
                DadoHidrologicoEntidade dadoHidrologicoEntidade = new DadoHidrologicoEntidade()
                {
                    IdReservatorio = item_DadoHidrologicoDto.IdReservatorio,
                    NomePosto = string.IsNullOrEmpty(item_DadoHidrologicoDto.NomePosto) ?
                        retornoValidacao.enumReservatorioHidroEntidade.First(p => p.IdReservatorioHidro == item_DadoHidrologicoDto.IdReservatorio).NomeReservatorio : item_DadoHidrologicoDto.NomePosto,
                    IdTipoDadoHidrologico = item_DadoHidrologicoDto.IdTipoDadoHidrologico,
                    DataRegistro = item_DadoHidrologicoDto.DataRegistro,
                    ValorLeitura = item_DadoHidrologicoDto.ValorLeitura,
                };

                int idDadoHidrologico = dadoHidrologicoRepositorio.InserirDadoHidrologico(dadoHidrologicoEntidade);

                if (idDadoHidrologico > 0)
                {
                    dadoHidrologicoEntidade.IdDadoHidrologico = idDadoHidrologico;
                    ListaInseridos.Add(new DadoHidrologicoDto(dadoHidrologicoEntidade));
                }
                else
                    ListaNaoInseridos.Add(new DadoHidrologicoDto(dadoHidrologicoEntidade));
            }

            Task.Factory.StartNew(() => InserirDadoHidroInstrumentos(request.ArrDadoHidrologico,retornoValidacao.usuarioEntidade));

            return Task.FromResult(new InserirDadoHidrologicoResultado()
            {
                ArrDadoInserido = ListaInseridos.ToArray(),
                ArrDadoNãoInserido = ListaNaoInseridos.ToArray()
            });
        }

        private (IEnumerable<ReservatorioHidroEntidade>, UsuarioEntidade) ValidaRequisicao(InserirDadoHidrologicoRequisicao request)
        {
            if (request.ArrDadoHidrologico == null)
                throw new RegraDeNegocioException("Não foram enviados nenhum registro de Dados Hidrológicos");

            UsuarioEntidade usuarioEntidade = ValidaUsuario(request);

            ValidaTipoDadoHidrologico(request, usuarioEntidade);

            IEnumerable<ReservatorioHidroEntidade> enumReservatorioHidroEntidade = ValidarRepositorio(request, usuarioEntidade);

            return (enumReservatorioHidroEntidade, usuarioEntidade);
        }

        private IEnumerable<ReservatorioHidroEntidade> ValidarRepositorio(InserirDadoHidrologicoRequisicao request, UsuarioEntidade usuarioEntidade)
        {
            IEnumerable<ReservatorioHidroEntidade> enumReservatorioHidroEntidade = reservatorioHidroRepositorio.ListarPorIdUsuario(usuarioEntidade.IdUsuario);

            int[] ArrIdReservatorioHidro = enumReservatorioHidroEntidade.Select(p => p.IdReservatorioHidro).ToArray();
            if (request.ArrDadoHidrologico.Any(p => !ArrIdReservatorioHidro.Contains(p.IdReservatorio)))
                throw new RegraDeNegocioException("O Empreendimento não possui nenhum Reservatorio cadastrado, com o IdReservatorio passado como parâmetro");
            return enumReservatorioHidroEntidade;
        }

        private void ValidaTipoDadoHidrologico(InserirDadoHidrologicoRequisicao request, UsuarioEntidade usuarioEntidade)
        {
            IEnumerable<TipoDadoHidrologicoEntidade> enumTipoDadoHidrologicoEntidade = tipoDadoHidrologicoRepositorio.ListarPorIdEmpreendedor(usuarioEntidade.IdEmpreendedor);

            int[] ArrIdTipoDadoHidrologico = enumTipoDadoHidrologicoEntidade.Select(p => p.IdTipoDadoHidrologico).ToArray();
            if (request.ArrDadoHidrologico.Any(p => !ArrIdTipoDadoHidrologico.Contains(p.IdTipoDadoHidrologico)))
                throw new RegraDeNegocioException("O Empreendimento não possui nenhum Tipo de Dado Hidrológico cadastrado, com o IdTipoDadoHidrologico passado como parâmetro");
        }

        private UsuarioEntidade ValidaUsuario(InserirDadoHidrologicoRequisicao request)
        {
            string senhaCodificada = Criptografia.Encrypt(request.Senha);

            return usuarioRepositorio.VerificaLoginSenha(request.Login, senhaCodificada);
        }

        public const int COTA_MONTANTE = 6;
        public const int COTA_JUSANTE = 7;
        public const int CHUVA = 8;

        #region Constantes de Pluviometria

        public const int CHAVANTES_DH1_PRC = 15644;
        public const int JURUMIRIM_DH1_PRC = 15651;
        public const int SALTO_GRANDE_DH1_PRC = 15657;
        public const int CANOAS_I_DH1_PRC = 15663;
        public const int CANOAS_II_DH1_PRC = 15668;
        public const int CAPIVARA_DH1_PRC = 15674;
        public const int TAQUARUCU_DH1_PRC = 15679;
        public const int ROSANA_DH1_PRC = 15709;

        public const int PALMEIRAS_DH1_PRC = 15698;
        public const int RETIRO_DH1_PRC = 15705;

        public const int ILHA_SOLTEIRA_DH1_PRC = 15616;
        public const int JUPIA_DH1_PRC = 15622 ;

        public const int SALTO_DH1_PRC = 15685;

        public const int GARIBALDI_DH1_PRC = 15691;

        #endregion

        #region Constantes de COTA_MONTANTE

        public const int CHAVANTES_NAM_0001 = 15605;
        public const int JURUMIRIM_NAM_0001 = 15607;
        public const int SALTO_GRANDE_NAM_0001 = 15609;
        public const int CANOAS_I_NAM_0001 = 15533;
        public const int CANOAS_II_NAM_0001 = 15602;
        public const int CAPIVARA_NAM_0001 = 15544;
        public const int TAQUARUCU_NAM_0001 = 14202;
        public const int ROSANA_NAM_0001 = 15083;

        public const int PALMEIRAS_NAM_0001 = 15630;
        public const int RETIRO_NAM_0001 = 15632;

        public const int ILHA_SOLTEIRA_NAM_0001 = 15627;
        public const int JUPIA_NAM_0001 = 15611;

        public const int SALTO_NAM_0001 = 15634;

        public const int GARIBALDI_NAM_0001 = 15636;

        #endregion

        #region Constantes de COTA_JUSANTE

        public const int CHAVANTES_NAJ_0001 = 15606;
        public const int JURUMIRIM_NAJ_0001 = 15608;
        public const int SALTO_GRANDE_NAJ_0001 = 15610;
        public const int CANOAS_I_NAJ_0001 = 15540;
        public const int CANOAS_II_NAJ_0001 = 15603;
        public const int CAPIVARA_NAJ_0001 = 15629;
        public const int TAQUARUCU_NAJ_0001 = 14203;
        public const int ROSANA_NAJ_0001 = 15084;

        public const int PALMEIRAS_NAJ_0001 = 15631;
        public const int RETIRO_NAJ_0001 = 15633;

        public const int ILHA_SOLTEIRA_NAJ_0001 = 15628;
        public const int JUPIA_NAJ_0001 = 15612;

        public const int SALTO_NAJ_0001 = 15635;

        public const int GARIBALDI_NAJ_0001 = 15637;

        #endregion

        #region Constantes de DIRECAO LEITURA INSTRUMENTO 

        public const int DIRECAO_PRECIPITACAO_PARAPANEMA = 623;
        public const int DIRECAO_COTA_MONTANTE_PARAPANEMA = 479;
        public const int DIRECAO_COTA_JUSANTE_PARAPANEMA = 480;

        public const int DIRECAO_PRECIPITACAO_SAPUCAI = 629;
        public const int DIRECAO_COTA_MONTANTE_SAPUCAI = 638;
        public const int DIRECAO_COTA_JUSANTE_SAPUCAI = 639;

        public const int DIRECAO_PRECIPITACAO_PARANA = 618;
        public const int DIRECAO_COTA_MONTANTE_PARANA = 487;
        public const int DIRECAO_COTA_JUSANTE_PARANA = 488;

        public const int DIRECAO_PRECIPITACAO_CANOAS = 646;
        public const int DIRECAO_COTA_MONTANTE_CANOAS = 640;
        public const int DIRECAO_COTA_JUSANTE_CANOAS = 641;

        public const int DIRECAO_PRECIPITACAO_VERDE = 634;
        public const int DIRECAO_COTA_MONTANTE_VERDE = 642;
        public const int DIRECAO_COTA_JUSANTE_VERDE = 643;

        #endregion

        private void InserirDadoHidroInstrumentos(EnvioDadoHidrologicoDto[] ArrDadoHidrologico, UsuarioEntidade usuarioEntidade)
        {
            foreach (EnvioDadoHidrologicoDto item_EnvioDadoHidrologico in ArrDadoHidrologico)
            {
                (int? IdInstrumento, int? IdDirecaoLeituraInstrumento) retorno = ListarPorTipoDado(item_EnvioDadoHidrologico);

                if (retorno.IdDirecaoLeituraInstrumento == null || retorno.IdInstrumento == null)
                    continue;

                LeituraEntidade leituraEntidade = new LeituraEntidade()
                {
                    idInstrumento = (int)retorno.IdInstrumento,
                    DataLeitura = item_EnvioDadoHidrologico.DataRegistro,
                    ValorLeitura = item_EnvioDadoHidrologico.ValorLeitura,
                    Relevante = false,
                    ResponsavelAlteracao = usuarioEntidade.IdUsuario,
                    DataAlteracao = DateTime.Now,
                    IdDirecaoLeituraInstrumento = retorno.IdDirecaoLeituraInstrumento,
                    IdNivelControle = null,
                    DataExclusao = null,
                    ReponsavelExclusao = null,
                    Observacao = null,
                    DataControlada = null,
                    ResponsavelControle = null,
                    FoiEditado = false,
                    IdTipoInsercao = 5,
                };

                leituraRepositorio.InserirLeitura(leituraEntidade);
            }
        }

        public (int?, int?) ListarPorTipoDado(EnvioDadoHidrologicoDto envioDadoHidrologico)
        {
            switch (envioDadoHidrologico.IdTipoDadoHidrologico)
            {
                case COTA_MONTANTE:
                    return ObterInstrumentoDirecaoCotaMontante(envioDadoHidrologico);
                case COTA_JUSANTE:
                    return ObterInstrumentoDirecaoCotaJusante(envioDadoHidrologico);
                case CHUVA:
                    return ObterInstrumentoDirecaoPrecipitacao(envioDadoHidrologico);

                default:
                    return (null, null);
            }
        }

        public (int?, int?) ObterInstrumentoDirecaoCotaMontante(EnvioDadoHidrologicoDto envioDadoHidrologico) 
        {
            switch (envioDadoHidrologico.NomePosto.ToUpper().Trim())
            {
                //EMPREENDEDOR 41
                case "RESERVATÓRIO DE CHAVANTES":
                    return (CHAVANTES_NAM_0001, DIRECAO_COTA_MONTANTE_PARAPANEMA);
                case "RESERVATÓRIO DE JURUMIRIM":
                    return (JURUMIRIM_NAM_0001, DIRECAO_COTA_MONTANTE_PARAPANEMA);
                case "RESERVATÓRIO DE SALTO GRANDE":
                    return (SALTO_GRANDE_NAM_0001, DIRECAO_COTA_MONTANTE_PARAPANEMA);
                case "RESERVATÓRIO DE CANOAS I":
                    return (CANOAS_I_NAM_0001, DIRECAO_COTA_MONTANTE_PARAPANEMA);
                case "RESERVATÓRIO DE CANOAS II":
                    return (CANOAS_II_NAM_0001, DIRECAO_COTA_MONTANTE_PARAPANEMA);
                case "RESERVATÓRIO DE CAPIVARA":
                    return (CAPIVARA_NAM_0001, DIRECAO_COTA_MONTANTE_PARAPANEMA);
                case "RESERVATÓRIO DE TAQUARUÇU":
                    return (TAQUARUCU_NAM_0001, DIRECAO_COTA_MONTANTE_PARAPANEMA);
                case "RESERVATÓRIO DE ROSANA":
                    return (ROSANA_NAM_0001, DIRECAO_COTA_MONTANTE_PARAPANEMA);
                //EMPREENDEDOR 42
                case "RESERVATÓRIO DE PALMEIRAS":
                    return (PALMEIRAS_NAM_0001, DIRECAO_COTA_MONTANTE_SAPUCAI);
                case "RESERVATÓRIO DE RETIRO":
                    return (RETIRO_NAM_0001, DIRECAO_COTA_MONTANTE_SAPUCAI);
                //EMPREENDEDOR 43
                case "RESERVATÓRIO DE ILHA SOLTEIRA":
                    return (ILHA_SOLTEIRA_NAM_0001, DIRECAO_COTA_MONTANTE_PARANA);
                case "RESERVATÓRIO DE JUPIÁ":
                    return (JUPIA_NAM_0001, DIRECAO_COTA_MONTANTE_PARANA);
                //EMPREENDEDOR 44
                case "RESERVATÓRIO DE SALTO":
                    return (SALTO_NAM_0001, DIRECAO_COTA_MONTANTE_CANOAS);
                //EMPREENDEDOR 45
                case "RESERVATÓRIO DE GARIBALDI":
                    return (GARIBALDI_NAM_0001, DIRECAO_COTA_MONTANTE_VERDE);

                default:
                    return (null,null);
            }
        }

        public (int?, int?) ObterInstrumentoDirecaoCotaJusante(EnvioDadoHidrologicoDto envioDadoHidrologico)
        {
            switch (envioDadoHidrologico.NomePosto)
            {
                //EMPREENDEDOR 41
                case "RESERVATÓRIO DE CHAVANTES":
                    return (CHAVANTES_NAJ_0001, DIRECAO_COTA_JUSANTE_PARAPANEMA);
                case "RESERVATÓRIO DE JURUMIRIM":
                    return (JURUMIRIM_NAJ_0001, DIRECAO_COTA_JUSANTE_PARAPANEMA);
                case "RESERVATÓRIO DE SALTO GRANDE":
                    return (SALTO_GRANDE_NAJ_0001, DIRECAO_COTA_JUSANTE_PARAPANEMA);
                case "RESERVATÓRIO DE CANOAS I":
                    return (CANOAS_I_NAJ_0001, DIRECAO_COTA_JUSANTE_PARAPANEMA);
                case "RESERVATÓRIO DE CANOAS II":
                    return (CANOAS_II_NAJ_0001, DIRECAO_COTA_JUSANTE_PARAPANEMA);
                case "RESERVATÓRIO DE CAPIVARA":
                    return (CAPIVARA_NAJ_0001, DIRECAO_COTA_JUSANTE_PARAPANEMA);
                case "RESERVATÓRIO DE TAQUARUÇU":
                    return (TAQUARUCU_NAJ_0001, DIRECAO_COTA_JUSANTE_PARAPANEMA);
                case "RESERVATÓRIO DE ROSANA":
                    return (ROSANA_NAJ_0001, DIRECAO_COTA_JUSANTE_PARAPANEMA);
                //EMPREENDEDOR 42
                case "RESERVATÓRIO DE PALMEIRAS":
                    return (PALMEIRAS_NAJ_0001, DIRECAO_COTA_JUSANTE_SAPUCAI);
                case "RESERVATÓRIO DE RETIRO":
                    return (RETIRO_NAJ_0001, DIRECAO_COTA_JUSANTE_SAPUCAI);
                //EMPREENDEDOR 43
                case "RESERVATÓRIO DE ILHA SOLTEIRA":
                    return (ILHA_SOLTEIRA_NAJ_0001, DIRECAO_COTA_JUSANTE_PARANA);
                case "RESERVATÓRIO DE JUPIÁ":
                    return (JUPIA_NAJ_0001, DIRECAO_COTA_JUSANTE_PARANA);
                //EMPREENDEDOR 44
                case "RESERVATÓRIO DE SALTO":
                    return (SALTO_NAJ_0001, DIRECAO_COTA_JUSANTE_CANOAS);
                //EMPREENDEDOR 45
                case "RESERVATÓRIO DE GARIBALDI":
                    return (GARIBALDI_NAJ_0001, DIRECAO_COTA_JUSANTE_VERDE);

                default:
                    return (null, null);
            }
        }

        public (int?, int?) ObterInstrumentoDirecaoPrecipitacao(EnvioDadoHidrologicoDto envioDadoHidrologico)
        {
            switch (envioDadoHidrologico.NomePosto)
            {
                //EMPREENDEDOR 41
                case "RESERVATÓRIO DE CHAVANTES":
                    return (CHAVANTES_DH1_PRC, DIRECAO_PRECIPITACAO_PARAPANEMA);
                case "RESERVATÓRIO DE JURUMIRIM":
                    return (JURUMIRIM_DH1_PRC, DIRECAO_PRECIPITACAO_PARAPANEMA);
                case "RESERVATÓRIO DE SALTO GRANDE":
                    return (SALTO_GRANDE_DH1_PRC, DIRECAO_PRECIPITACAO_PARAPANEMA);
                case "RESERVATÓRIO DE CANOAS I":
                    return (CANOAS_I_DH1_PRC, DIRECAO_PRECIPITACAO_PARAPANEMA);
                case "RESERVATÓRIO DE CANOAS II":
                    return (CANOAS_II_DH1_PRC, DIRECAO_PRECIPITACAO_PARAPANEMA);
                case "RESERVATÓRIO DE CAPIVARA":
                    return (CAPIVARA_DH1_PRC, DIRECAO_PRECIPITACAO_PARAPANEMA);
                case "RESERVATÓRIO DE TAQUARUÇU":
                    return (TAQUARUCU_DH1_PRC, DIRECAO_PRECIPITACAO_PARAPANEMA);
                case "RESERVATÓRIO DE ROSANA":
                    return (ROSANA_DH1_PRC, DIRECAO_PRECIPITACAO_PARAPANEMA);
                //EMPREENDEDOR 42
                case "RESERVATÓRIO DE PALMEIRAS":
                    return (PALMEIRAS_DH1_PRC, DIRECAO_PRECIPITACAO_SAPUCAI);
                case "RESERVATÓRIO DE RETIRO":
                    return (RETIRO_DH1_PRC, DIRECAO_PRECIPITACAO_SAPUCAI);
                //EMPREENDEDOR 43
                case "RESERVATÓRIO DE ILHA SOLTEIRA":
                    return (ILHA_SOLTEIRA_DH1_PRC, DIRECAO_PRECIPITACAO_PARANA);
                case "RESERVATÓRIO DE JUPIÁ":
                    return (JUPIA_DH1_PRC, DIRECAO_PRECIPITACAO_PARANA);
                //EMPREENDEDOR 44
                case "RESERVATÓRIO DE SALTO":
                    return (SALTO_DH1_PRC, DIRECAO_PRECIPITACAO_CANOAS);
                //EMPREENDEDOR 45
                case "RESERVATÓRIO DE GARIBALDI":
                    return (GARIBALDI_DH1_PRC, DIRECAO_PRECIPITACAO_VERDE);

                default:
                    return (null, null);
            }
        }
    }
}
