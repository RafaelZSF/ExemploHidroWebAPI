using System;
using System.Collections.Generic;
using System.Text;
using ReservatorioHidroEntidade = HidroWebAPI.Dominio.Entidades.ReservatorioHidro;

namespace HidroWebAPI.Aplicacao.Repositorios
{
    public interface IReservatorioHidroRepositorio
    {
        IEnumerable<ReservatorioHidroEntidade> ListarPorIdEmpreendedor(int IdEmpreendedor);
        IEnumerable<ReservatorioHidroEntidade> ListarPorIdUsuario(int IdUsuario);
    }
}
