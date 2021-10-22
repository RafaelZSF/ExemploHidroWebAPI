using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Dominio.Entidades
{
    public class Barragem
    {
        public int IdBarragem { get; set; }
        public int? IdEmpreendedor { get; set; }
        public int? IdTipoEstrutura { get; set; }
        public int? IdEmpreendimento { get; set; }
        public string Nome { get; set; }

        //public long? Latitude { get; set; }
        //public long? Longitude { get; set; }
        //public string Fuso { get; set; }
        //public int? MapaZoom { get; set; }
        //public int DatUmHorizontal { get; set; }
        //public string CursoDAguaBarragado { get; set; }
        //public string Bacia { get; set; }
        //public string SubBacia { get; set; }
        //public int? Finalidade { get; set; }
        //public int? EtapaVidaUtil { get; set; }
        //public int? InicioOperacao { get; set; }
        //public int? FimOperacao { get; set; }
        //public int? AlteamentosPrevistos { get; set; }
        //public int? AlteamentosRealizados { get; set; }
        //public int? ResponsavelExclusao { get; set; }
        public DateTime? DataExclusao { get; set; }
        //public int? ResponsavelAlteracao { get; set; }
        //public DateTime? DataAlteracao { get; set; }
        //public bool EstruturaCompetencia { get; set; }
        //public int? IDSubTipoEstrutura { get; set; }
        //public string DescricaoTipoEstrutura { get; set; }
        //public string DescricaoSubTipoEstrutura { get; set; } // Tipo
        //public bool EhDeContreto { get; set; }
        //public string OrgaoFiscalizador { get; set; }
        //public string Observacoes { get; set; }
        //public bool EhBarragemNova { get; set; }
        //public int? IdBarragemMae { get; set; }
        //public string Drenagem { get; set; }
        //public string TipoSubEstrutura { get; set; }
        //public string ComprimentoTotalSubEstrutura { get; set; }
        //public string AlturaSubEstrutura { get; set; }
        //public string ElevacaoSubEstrutura { get; set; }
        //public int? IdAnalisePercolacao { get; set; }
        //public int? IdAnaliseEstabilidade { get; set; }
        //public string LarguraCristaSubEstrutura { get; set; }
        //public string LarguraBermasSubEstrutura { get; set; }
        //public string InclinacaoTaludeJusanteSubEstrutura { get; set; }
        //public string InclinacaoTaludeMontanteSubEstrutura { get; set; }
        //public string ObservacoesSubEstrutura { get; set; }
        //public string Empreendimento_nome { get; set; }
        //public string Cor { get; set; }
        //public string Iniciais { get; set; }
        //public string CaminhoCamada { get; set; }
        //public double? LatitudeEmpreendimento { get; set; }
        //public double? LongitudeEmpreendimento { get; set; }
        //public int? MapaZoomEmpreendimento { get; set; }
    }
}
