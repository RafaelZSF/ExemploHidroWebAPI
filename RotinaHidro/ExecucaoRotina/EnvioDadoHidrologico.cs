using HidroWebAPI.Aplicacao.Dtos;
using HidroWebAPI.Controllers;
using HidroWebAPI.Infraestrutura.BancoDados.Repositorios;
using HidroWebAPI.Infraestrutura.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DadoMetereologicoEntidade = HidroWebAPI.Dominio.Entidades.VS_GESTAO_BARRAGENS;
using TipoDadoHidrologicoEntidade = HidroWebAPI.Dominio.Entidades.TipoDadoHidrologico;
using ReservatorioHidroEntidade = HidroWebAPI.Dominio.Entidades.ReservatorioHidro;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Net.Http.Headers;
using HidroWebAPI.Util.Exceptions;
using HidroWebAPI.Models.DadoHidrologico;

namespace RotinaHidro.ExecucaoRotina
{
    public class EnvioDadoHidrologico
    {
        private readonly ViewGestaoBarragensRepositorio viewGestaoBarragensRepositorio;
        private readonly TipoDadoHidrologicoRepositorio tipoDadoHidrologicoRepositorio;
        private readonly ReservatorioHidroRepositorio reservatorioHidroRepositorio;

        private const int ID_EMPREENDEDOR_CHESF = 40;
        public EnvioDadoHidrologico(HidroContexto contextChesf, HidroContexto contextPimenta)
        {
            viewGestaoBarragensRepositorio = new ViewGestaoBarragensRepositorio(contextChesf);
            tipoDadoHidrologicoRepositorio = new TipoDadoHidrologicoRepositorio(contextPimenta);
            reservatorioHidroRepositorio = new ReservatorioHidroRepositorio(contextPimenta);
        }

        public void ExecutarRotina()
        {
            Console.WriteLine("INÍCIO - Execução da Rotina");
            EnviarDadoHidrologicoChesf();
            Console.WriteLine("FIM - Execução da Rotina");
        }

        public void EnviarDadoHidrologicoChesf()
        {
            DateTime DataInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            IEnumerable<DadoMetereologicoEntidade> enumDadosMetereologico = viewGestaoBarragensRepositorio.ListarPorData(DataInicio.AddDays(-1), DataFim.AddDays(-1));

            if (!enumDadosMetereologico.Any())
                return;

            List<EnvioDadoHidrologicoDto> ListaEnvioDadoHidrologico = new List<EnvioDadoHidrologicoDto>();

            IEnumerable<TipoDadoHidrologicoEntidade> enumTipoDadoHidrologico = tipoDadoHidrologicoRepositorio.ListarPorIdEmpreendedor(ID_EMPREENDEDOR_CHESF);
            IEnumerable<ReservatorioHidroEntidade> enumReservatorioHidro = reservatorioHidroRepositorio.ListarPorIdEmpreendedor(ID_EMPREENDEDOR_CHESF);

            int contador = 1;
            foreach (DadoMetereologicoEntidade item_DadoMetereologico in enumDadosMetereologico)
            {
                if (item_DadoMetereologico.NR_LEITURA == null)
                    continue;

                EnvioDadoHidrologicoDto dadoHidrologico = new EnvioDadoHidrologicoDto()
                {
                    IdReservatorio = ObterReservatorioChest(item_DadoMetereologico, enumReservatorioHidro),
                    NomePosto = item_DadoMetereologico.NM_POSTO,
                    IdTipoDadoHidrologico = ObterTipoDadoMetereologicoChesf(item_DadoMetereologico, enumTipoDadoHidrologico),
                    DataRegistro = item_DadoMetereologico.DT_REGISTRO,
                    ValorLeitura = (int)item_DadoMetereologico.NR_LEITURA,
                };

                if (dadoHidrologico.IdReservatorio == -1 || dadoHidrologico.IdTipoDadoHidrologico == -1)
                    continue;

                Console.WriteLine(String.Format("Inserindo {0} leitura na Lista",contador));
                ListaEnvioDadoHidrologico.Add(dadoHidrologico);
                contador++;
            }

            EnviarDadosParaAPI(ListaEnvioDadoHidrologico);
        }

        private static void EnviarDadosParaAPI(List<EnvioDadoHidrologicoDto> ListaEnvioDadoHidrologico)
        {
            Console.WriteLine("Enviando para API...");

            InserirDadoHidrologicoInput inserirDadoHidrologicoInput = new InserirDadoHidrologicoInput()
            {
                Login = "LOGIN",
                Senha = "SENHA",
                ArrDadoHidrologico = ListaEnvioDadoHidrologico.ToArray()
            };

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = Timeout.InfiniteTimeSpan;

                string Endpoint = "https://localhost:44308/api/DadoHidrologico/InserirDadoHidrologico";

                string jsonString = JsonConvert.SerializeObject(inserirDadoHidrologicoInput);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage HttpResponseMessage = httpClient.PostAsync(Endpoint, content).Result;

                if (!HttpResponseMessage.IsSuccessStatusCode)
                    throw new RegraDeNegocioException("Ocorreu um erro na chamada da API externa..");
            }
            Console.WriteLine("Enviado com sucesso.");
        }

        public int ObterReservatorioChest(DadoMetereologicoEntidade p_DadoMetereologicoEntidade, IEnumerable<ReservatorioHidroEntidade> p_EnumReservatorioHidro) 
        {
            ReservatorioHidroEntidade reservatorioHidroEntidade = p_EnumReservatorioHidro.FirstOrDefault(p => p.NomeReservatorio.ToUpper().Trim().Equals(p_DadoMetereologicoEntidade.NM_POSTO.ToUpper().Trim()));

            return reservatorioHidroEntidade != null ? reservatorioHidroEntidade.IdReservatorioHidro : ObterReservatorioChestPorCodigoPosto(p_DadoMetereologicoEntidade.CODIGO_POSTO);
        }

        public int ObterReservatorioChestPorCodigoPosto(int p_CodigoPosto)
        {
            switch (p_CodigoPosto)
            {
                case 49208080:  //UAS RESERVATÓRIO DE MOXOTÓ
                    return 1;
                case 49200000:  //APJ RESERVATÓRIO DE MOXÓTO JUSANTE
                    return 1;

                case 34218080:  //UBE RESERVATÓRIO DE BOA ESPERANÇA
                    return 2;   
                case 744013:    //BEJ	RESERVATÓRIO DE BOA ESPERANÇA JUSANTE
                    return 2;   
                case 34219080:  //BEJ	RESERVATÓRIO DE BOA ESPERANÇA JUSANTE
                    return 2;

                case 49042580:  //ULG RESERVATÓRIO DE ITAPARICA
                    return 3;
                case 49046000:  //ITJ RESERVATÓRIO DE ITAPARICA JUSANTE
                    return 3;

                case 49210080:  //UPA RESERVATÓRIO DELMIRO GOUVEI
                    return 4;
                case 49210085:  //UPAJ DELMIRO GOUVEIA - JUSANTE
                    return 4;

                case 52568080:  //UPE RESERVATÓRIO DE PEDRA
                    return 5;
                case 52569000:  //PEJ RESERVATÓRIO DE PEDRA JUSANTE
                    return 5;
                case 1340022:  //PEJ RESERVATÓRIO DE PEDRA JUSANTE
                    return 5;

                case 49340080:  //UXG RESERVATÓRIO DE XINGÓ
                    return 6;
                case 49341000:  //XGJ RESERVATÓRIO DE XINGO JUSANTE
                    return 6;

                case 47750080:  //USB RESERVATÓRIO DE SOBRADINHO
                    return 7;
                case 47770000:  //SBJ RESERVATÓRIO DE SOBRADINHO JUSANTE
                    return 7;
                case 940029:    //SBJ RESERVATÓRIO DE SOBRADINHO JUSANTE
                    return 7;


                case 52697080: //UFL RESERVATÓRIO DE FUNIL
                    return 8;
                case 1439107:  //UFJ RESERVATÓRIO DE FUNIL JUSANTE
                    return 8;
                case 52698000: //UFJ RESERVATÓRIO DE FUNIL JUSANTE
                    return 8;

                case 49210070: //USQ PAULO AFONSO IV
                    return 9;
                case 49211050: //UQJ PAULO AFONSO IV JUSANTE
                    return 9;
                case 52404002: //UQJ PAULO AFONSO IV JUSANTE
                    return 9;

                default:
                    return -1;
            }

        }

        public int ObterTipoDadoMetereologicoChesf(DadoMetereologicoEntidade p_DadoMetereologicoEntidade, IEnumerable<TipoDadoHidrologicoEntidade> p_EnumTipoDadoHidrologico)
        {
            TipoDadoHidrologicoEntidade tipoDadoHidrologicoEntidade = p_EnumTipoDadoHidrologico.FirstOrDefault(p => p.NomeTipoDado.ToUpper().Trim().Equals(p_DadoMetereologicoEntidade.TIPO_DADO.ToUpper().Trim()));

            return tipoDadoHidrologicoEntidade != null ? tipoDadoHidrologicoEntidade.IdTipoDadoHidrologico : ObterTipoDadoHidrologicoChesfPorTípoDado(p_DadoMetereologicoEntidade.TIPO_DADO);
        }

        public int ObterTipoDadoHidrologicoChesfPorTípoDado(string strTipoDado) 
        {
            switch (strTipoDado)
            {
                case "COTA_MONTANTE":  
                    return 1;
                case "COTA_JUSANTE":  
                    return 2;
                case "AFLUENCIA":  
                    return 3;
                case "DEFLUENCIA":  
                    return 4;
                case "CHUVA":
                    return 5;
                default:
                    return -1;
            }
        }

    }
}
