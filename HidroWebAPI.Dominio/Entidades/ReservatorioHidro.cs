using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Dominio.Entidades
{
    public class ReservatorioHidro
    {
        public int IdReservatorioHidro { get; set; }
        public int IdEmpreendedor { get; set; }
        public string SiglaPosto { get; set; }
        public string NomeReservatorio { get; set; }
    }
}
