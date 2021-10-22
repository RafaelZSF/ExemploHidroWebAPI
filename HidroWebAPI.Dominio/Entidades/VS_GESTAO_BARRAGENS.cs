using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Dominio.Entidades
{
    public class VS_GESTAO_BARRAGENS
    {
        public int CODIGO_POSTO { get; set; }
        public string SG_POSTO { get; set; }
        public string NM_POSTO { get; set; }
        public DateTime DT_REGISTRO { get; set; }
        public string TIPO_DADO { get; set; }
        public decimal? NR_LEITURA { get; set; }
    }
}
