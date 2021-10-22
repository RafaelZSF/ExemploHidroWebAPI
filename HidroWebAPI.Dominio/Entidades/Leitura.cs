using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Dominio.Entidades
{
    public class Leitura
    {
        public int IdLeitura { get; set; }
        public int idInstrumento { get; set; }
        public DateTime DataLeitura { get; set; }
        public decimal ValorLeitura { get; set; }
        public bool Relevante { get; set; }
        public int ResponsavelAlteracao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public int? IdDirecaoLeituraInstrumento { get; set; }
        public int? IdNivelControle { get; set; }
        public DateTime? DataExclusao { get; set; }
        public int? ReponsavelExclusao { get; set; }
        public string Observacao { get; set; }
        public DateTime? DataControlada { get; set; }
        public int? ResponsavelControle { get; set; }
        public bool FoiEditado { get; set; }
        public int IdTipoInsercao { get; set; }
    }
}
