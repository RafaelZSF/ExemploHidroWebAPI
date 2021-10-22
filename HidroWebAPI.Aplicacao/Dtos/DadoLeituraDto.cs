using System;
using System.Collections.Generic;
using System.Text;
using LeituraEntidade = HidroWebAPI.Dominio.Entidades.Leitura;

namespace HidroWebAPI.Aplicacao.Dtos
{
    public class DadoLeituraDto
    {
        public int IdLeitura { get; set; }
        public int idInstrumento { get; set; }
        public DateTime DataLeitura { get; set; }
        public decimal ValorLeitura { get; set; }
        public bool EHRelevante { get; set; }
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

        public DadoLeituraDto() { }

        public DadoLeituraDto(LeituraEntidade leituraEntidade) 
        {
            IdLeitura = leituraEntidade.IdLeitura;
            idInstrumento = leituraEntidade.idInstrumento;
            DataLeitura = leituraEntidade.DataLeitura;
            ValorLeitura = leituraEntidade.ValorLeitura;
            EHRelevante = leituraEntidade.Relevante;
            ResponsavelAlteracao = leituraEntidade.ResponsavelAlteracao;
            DataAlteracao = leituraEntidade.DataAlteracao;
            IdDirecaoLeituraInstrumento = leituraEntidade.IdDirecaoLeituraInstrumento;
            IdNivelControle = leituraEntidade.IdNivelControle;
            DataExclusao = leituraEntidade.DataExclusao;
            ReponsavelExclusao = leituraEntidade.ReponsavelExclusao;
            Observacao = leituraEntidade.Observacao;
            DataControlada = leituraEntidade.DataControlada;
            ResponsavelControle = leituraEntidade.ResponsavelControle;
            FoiEditado = leituraEntidade.FoiEditado;
            IdTipoInsercao = leituraEntidade.IdTipoInsercao;
        }

    }
}
