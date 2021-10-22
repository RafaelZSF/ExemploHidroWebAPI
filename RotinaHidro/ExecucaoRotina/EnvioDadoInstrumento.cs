using HidroWebAPI.Aplicacao.Dtos;
using HidroWebAPI.Infraestrutura.BancoDados.Repositorios;
using HidroWebAPI.Infraestrutura.Contexto;
using HidroWebAPI.Models.DadoHidrologico;
using HidroWebAPI.Util.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using DadoMetereologicoEntidade = HidroWebAPI.Dominio.Entidades.VS_GESTAO_BARRAGENS;

namespace RotinaHidro.ExecucaoRotina
{
    public class EnvioDadoInstrumento
    {
        private readonly ViewGestaoBarragensRepositorio viewGestaoBarragensRepositorio;

        private const int ID_EMPREENDEDOR_CHESF = 40;

        #region Constantes de Direcao Leitura Instrumento 

        public const int ID_DIRECAO_CHUVA = 985;
        public const int ID_DIRECAO_COTA_JUSANTE = 983;
        public const int ID_DIRECAO_COTA_MONTANTE = 984;
        public const int ID_DIRECAO_AFLUENCIA = 986;
        public const int ID_DIRECAO_DEFLUENCIA = 987;
        public const int ID_DIRECAO_VOLUME_HM = 990;
        public const int ID_DIRECAO_VOLUME_UTIL = 988;

        #endregion

        #region Constantes de Pluviometria

        public const int CHUVA_P1 = 18775;
        public const int CHUVA_P2 = 18778;
        public const int CHUVA_P3 = 18781;
        public const int CHUVA_P4 = 18752;
        public const int CHUVA_P5 = 18757;
        public const int CHUVA_P6 = 18769;
        public const int CHUVA_P7 = 18764;
        public const int CHUVA_P8 = 18770;
        public const int CHUVA_P9 = 18784;
        public const int CHUVA_P10 = 18788;

        #endregion

        #region Constantes de COTA_JUSANTE

        public const int COTA_JUSANTE_CJ_1 = 18776;
        public const int COTA_JUSANTE_CJ_2 = 18779;
        public const int COTA_JUSANTE_CJ_3 = 18782;
        public const int COTA_JUSANTE_CJ_4 = 18753;
        public const int COTA_JUSANTE_CJ_5 = 18765;
        public const int COTA_JUSANTE_CJ_6 = 18771;
        public const int COTA_JUSANTE_CJ_7 = 18785;
        public const int COTA_JUSANTE_CJ_8 = 18773;
        public const int COTA_JUSANTE_CJ_9 = 18759;
        public const int COTA_JUSANTE_CJ_10 = 18789;
        public const int COTA_JUSANTE_CJ_11 = 18767;

        #endregion

        #region Constantes de COTA_MONTANTE

        public const int COTA_MONTANTE_CM_1 = 18754;
        public const int COTA_MONTANTE_CM_2 = 18766;
        public const int COTA_MONTANTE_CM_3 = 18772;
        public const int COTA_MONTANTE_CM_4 = 18786;
        public const int COTA_MONTANTE_CM_5 = 18790;
        public const int COTA_MONTANTE_CM_6 = 18774;
        public const int COTA_MONTANTE_CM_7 = 18760;
        public const int COTA_MONTANTE_CM_8 = 18768;
        public const int COTA_MONTANTE_CM_9 = 18777;
        public const int COTA_MONTANTE_CM_10 = 18780;
        public const int COTA_MONTANTE_CM_11 = 18783;

        #endregion

        #region Constantes de AFLUENCIA

        public const int AFLUENCIA_A_1 = 18791;
        public const int AFLUENCIA_A_2 = 18794;
        public const int AFLUENCIA_A_3 = 18798;
        public const int AFLUENCIA_A_4 = 18801;
        public const int AFLUENCIA_A_5 = 18805;
        public const int AFLUENCIA_A_6 = 18809;
        public const int AFLUENCIA_A_7 = 18813;
        public const int AFLUENCIA_A_8 = 18816;
        public const int AFLUENCIA_A_9 = 18819;
        public const int AFLUENCIA_A_10 = 18822;
        public const int AFLUENCIA_A_11 = 18825;

        #endregion

        #region Constantes de DEFLUENCIA

        public const int DEFLUENCIA_D_1 = 18792;
        public const int DEFLUENCIA_D_2 = 18795;
        public const int DEFLUENCIA_D_3 = 18799;
        public const int DEFLUENCIA_D_4 = 18802;
        public const int DEFLUENCIA_D_5 = 18810;
        public const int DEFLUENCIA_D_6 = 18814;
        public const int DEFLUENCIA_D_7 = 18806;
        public const int DEFLUENCIA_D_8 = 18817;
        public const int DEFLUENCIA_D_9 = 18820;
        public const int DEFLUENCIA_D_10 = 18823;
        public const int DEFLUENCIA_D_11 = 18826;

        #endregion

        #region Constantes de VOLUME_HM

        public const int VOLUME_HM_V1 = 18796;
        public const int VOLUME_HM_V2 = 18800;
        public const int VOLUME_HM_V3 = 18803;
        public const int VOLUME_HM_V4 = 18807;
        public const int VOLUME_HM_V5 = 18811;
        public const int VOLUME_HM_V6 = 18815;
        public const int VOLUME_HM_V7 = 18818;
        public const int VOLUME_HM_V8 = 18821;
        public const int VOLUME_HM_V9 = 18824;
        public const int VOLUME_HM_V10 = 18827;
        public const int VOLUME_HM_V11 = 18793;

        #endregion

        #region Constantes de VOLUME_UTIL

        public const int VOLUME_UTIL_VU1 = 18797;
        public const int VOLUME_UTIL_VU2 = 18804;
        public const int VOLUME_UTIL_VU3 = 18828;
        public const int VOLUME_UTIL_VU4 = 18812;

        #endregion

        public EnvioDadoInstrumento(HidroContexto contextChesf)
        {
            viewGestaoBarragensRepositorio = new ViewGestaoBarragensRepositorio(contextChesf);
        }

        public void ExecutarRotina()
        {
            Console.WriteLine("INÍCIO - Execução da Rotina");
            EnviarDadosInstrumentosChesf();
            Console.WriteLine("FIM - Execução da Rotina");
        }


        public void EnviarDadosInstrumentosChesf()
        {
            DateTime DataInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            IEnumerable<DadoMetereologicoEntidade> enumDadosMetereologico = viewGestaoBarragensRepositorio.ListarPorData(DataInicio.AddDays(-1), DataFim.AddDays(-1));

            if (!enumDadosMetereologico.Any())
                return;

            List<EnvioDadoInstrumentoDto> ListaEnvioDadoInstrumento = new List<EnvioDadoInstrumentoDto>();

            foreach (DadoMetereologicoEntidade item_DadoMetereologico in enumDadosMetereologico)
            {
                if (item_DadoMetereologico.NR_LEITURA == null)
                    continue;

                EnvioDadoInstrumentoDto[] ArrEnvioDadoInstrumentoDto = ListarPorTipoDado(item_DadoMetereologico);

                if (!ArrEnvioDadoInstrumentoDto.Any())
                    continue;

                ListaEnvioDadoInstrumento.AddRange(ArrEnvioDadoInstrumentoDto);
            }

            EnviarDadosParaAPI(ListaEnvioDadoInstrumento);
        }

        private static void EnviarDadosParaAPI(List<EnvioDadoInstrumentoDto> ListaEnvioDadoInstrumento)
        {
            Console.WriteLine("Enviando para API...");

            InserirDadoHidroInstrumentoInput inserirDadoHidrologicoInput = new InserirDadoHidroInstrumentoInput()
            {
                Login = "LOGIN",
                Senha = "SENHA",
                ArrDadoInstrumento = ListaEnvioDadoInstrumento.ToArray()
            };

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = Timeout.InfiniteTimeSpan;

                string Endpoint = "https://localhost:44308/api/DadoHidrologico/InserirDadoHidroInstrumento";

                string jsonString = JsonConvert.SerializeObject(inserirDadoHidrologicoInput);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage HttpResponseMessage = httpClient.PostAsync(Endpoint, content).Result;

                if (!HttpResponseMessage.IsSuccessStatusCode)
                    throw new RegraDeNegocioException("Ocorreu um erro na chamada da API externa..");
            }
            Console.WriteLine("Enviado com sucesso.");
        }

        public EnvioDadoInstrumentoDto[] ListarPorTipoDado(DadoMetereologicoEntidade dadoMetereologicoEntidade)
        {
            switch (dadoMetereologicoEntidade.TIPO_DADO)
            {
                case "COTA_MONTANTE":
                    return ListarDadosCotaMontante(dadoMetereologicoEntidade);
                case "COTA_JUSANTE":
                    return ListarDadosCotaJusante(dadoMetereologicoEntidade);
                case "AFLUENCIA":
                    return ListarDadosAfluencia(dadoMetereologicoEntidade);
                case "DEFLUENCIA":
                    return ListarDadosDefluencia(dadoMetereologicoEntidade);
                case "CHUVA":
                    return ListarDadosChuva(dadoMetereologicoEntidade);
                case "VOLUME_ÚTIL_PERCENTUAL":
                    return ListarDadosVolumeUtil(dadoMetereologicoEntidade);
                case "VOLUME_HM_3":
                    return ListarDadosVolumeHM(dadoMetereologicoEntidade);
                default:
                    return new EnvioDadoInstrumentoDto[0];
            }
        }

        public EnvioDadoInstrumentoDto[] ListarDadosChuva(DadoMetereologicoEntidade dadoMetereologicoEntidade)
        {
            switch (dadoMetereologicoEntidade.NM_POSTO)
            {

                case "PAULO AFONSO IV JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = CHUVA_P1,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_CHUVA,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = CHUVA_P2,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_CHUVA,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = CHUVA_P3,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_CHUVA,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = CHUVA_P4,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_CHUVA,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = CHUVA_P5,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_CHUVA,
                        }
                    };
                case "RESERVATÓRIO DE SOBRADINHO JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = CHUVA_P10,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_CHUVA,
                        }
                    };
                case "RESERVATÓRIO DE BOA ESPERANÇA JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = CHUVA_P7,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_CHUVA,
                        }
                    };
                case "RESERVATÓRIO DE PEDRA JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = CHUVA_P9,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_CHUVA,
                        }
                    };
                case "RESERVATÓRIO DE FUNIL JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = CHUVA_P8,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_CHUVA,
                        }
                    };
                case "PIRANHAS":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = CHUVA_P6,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_CHUVA,
                        }
                    };
                default:
                    return new EnvioDadoInstrumentoDto[0];
            }
        }

        public EnvioDadoInstrumentoDto[] ListarDadosAfluencia(DadoMetereologicoEntidade dadoMetereologicoEntidade)
        {
            switch (dadoMetereologicoEntidade.NM_POSTO)
            {
                case "RESERVATÓRIO DELMIRO GOUVEIA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_9,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_10,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_11,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        }
                    };
                case "PAULO AFONSO IV":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_1,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE MOXOTÓ":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_7,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE ITAPARICA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_6,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE SOBRADINHO":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_5,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE BOA ESPERANÇA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_2,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE PEDRA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_4,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE FUNIL":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_3,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE XINGÓ":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = AFLUENCIA_A_8,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_AFLUENCIA,
                        }
                    };

                default:
                    return new EnvioDadoInstrumentoDto[0];
            }
        }

        public EnvioDadoInstrumentoDto[] ListarDadosCotaJusante(DadoMetereologicoEntidade dadoMetereologicoEntidade)
        {
            switch (dadoMetereologicoEntidade.NM_POSTO)
            {
                case "PAULO AFONSO IV JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_1,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_2,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_3,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_4,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        }
                    };
                case "RESERVATÓRIO DE MOXÓTO JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_9,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        }
                    };
                case "RESERVATÓRIO DE ITAPARICA JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_8,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        }
                    };
                case "RESERVATÓRIO DE SOBRADINHO JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_10,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        }
                    };
                case "RESERVATÓRIO DE BOA ESPERANÇA JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_5,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        }
                    };
                case "RESERVATÓRIO DE PEDRA JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_7,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        }
                    };
                case "RESERVATÓRIO DE FUNIL JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_6,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        }
                    };
                case "RESERVATÓRIO DE XINGO JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_JUSANTE_CJ_11,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_JUSANTE,
                        }
                    };

                default:
                    return new EnvioDadoInstrumentoDto[0];
            }
        }

        public EnvioDadoInstrumentoDto[] ListarDadosCotaMontante(DadoMetereologicoEntidade dadoMetereologicoEntidade)
        {
            switch (dadoMetereologicoEntidade.NM_POSTO)
            {
                case "RESERVATÓRIO DELMIRO GOUVEIA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_9,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_10,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_11,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        }
                    };
                case "PAULO AFONSO IV":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_1,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        }
                    };
                case "PAULO AFONSO IV JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_1,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        }
                    };
                case "RESERVATÓRIO DE MOXOTÓ":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_7,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        }
                    };
                case "RESERVATÓRIO DE ITAPARICA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_6,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        }
                    };
                case "RESERVATÓRIO DE SOBRADINHO":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_5,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        }
                    };
                case "RESERVATÓRIO DE BOA ESPERANÇA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_2,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        }
                    };
                case "RESERVATÓRIO DE PEDRA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_4,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        }
                    };
                case "RESERVATÓRIO DE FUNIL":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_3,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        }
                    };
                case "RESERVATÓRIO DE XINGÓ":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = COTA_MONTANTE_CM_8,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_COTA_MONTANTE,
                        }
                    };

                default:
                    return new EnvioDadoInstrumentoDto[0];
            }
        }

        public EnvioDadoInstrumentoDto[] ListarDadosDefluencia(DadoMetereologicoEntidade dadoMetereologicoEntidade)
        {
            switch (dadoMetereologicoEntidade.NM_POSTO)
            {
                case "DELMIRO GOUVEIA - JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_9,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_10,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_11,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        }
                    };
                case "PAULO AFONSO IV JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_1,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE MOXÓTO JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_6,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE ITAPARICA JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_5,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE SOBRADINHO JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_7,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE BOA ESPERANÇA JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_2,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE PEDRA JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_4,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE FUNIL JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_3,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        }
                    };
                case "RESERVATÓRIO DE XINGO JUSANTE":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = DEFLUENCIA_D_8,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_DEFLUENCIA,
                        }
                    };

                default:
                    return new EnvioDadoInstrumentoDto[0];
            }
        }

        public EnvioDadoInstrumentoDto[] ListarDadosVolumeHM(DadoMetereologicoEntidade dadoMetereologicoEntidade)
        {
            switch (dadoMetereologicoEntidade.NM_POSTO)
            {
                case "RESERVATÓRIO DELMIRO GOUVEIA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V8,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V9,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        },
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V10,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        }
                        ,
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V11,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        }
                    };
                case "RESERVATÓRIO DE MOXOTÓ":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V6,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        }
                    };
                case "RESERVATÓRIO DE ITAPARICA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V5,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        }
                    };
                case "RESERVATÓRIO DE SOBRADINHO":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V4,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        }
                    };
                case "RESERVATÓRIO DE BOA ESPERANÇA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V1,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        }
                    };
                case "RESERVATÓRIO DE PEDRA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V3,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        }
                    };
                case "RESERVATÓRIO DE FUNIL":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V2,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        }
                    };
                case "RESERVATÓRIO DE XINGÓ":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_HM_V7,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_HM,
                        }
                    };

                default:
                    return new EnvioDadoInstrumentoDto[0];
            }
        }

        public EnvioDadoInstrumentoDto[] ListarDadosVolumeUtil(DadoMetereologicoEntidade dadoMetereologicoEntidade)
        {
            switch (dadoMetereologicoEntidade.NM_POSTO)
            {
                
                case "RESERVATÓRIO DE ITAPARICA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_UTIL_VU4,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_UTIL,
                        }
                    };
                case "RESERVATÓRIO DE SOBRADINHO":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_UTIL_VU3,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_UTIL,
                        }
                    };
                case "RESERVATÓRIO DE BOA ESPERANÇA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_UTIL_VU1,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_UTIL,
                        }
                    };
                case "RESERVATÓRIO DE PEDRA":
                    return new EnvioDadoInstrumentoDto[]
                    {
                        new EnvioDadoInstrumentoDto()
                        {
                            IdInstrumento = VOLUME_UTIL_VU2,
                            DataRegistro = dadoMetereologicoEntidade.DT_REGISTRO,
                            ValorLeitura= (decimal)dadoMetereologicoEntidade.NR_LEITURA,
                            IdDirecaoLeituraInstrumento = ID_DIRECAO_VOLUME_UTIL,
                        }
                    }; 

                default:
                    return new EnvioDadoInstrumentoDto[0];
            }
        }

    }
}
