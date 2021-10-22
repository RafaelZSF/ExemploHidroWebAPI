using HidroWebAPI.Aplicacao.Repositorios;
using HidroWebAPI.Infraestrutura.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReservatorioHidroEntidade = HidroWebAPI.Dominio.Entidades.ReservatorioHidro;
using UsuarioEmpreendedorEntidade = HidroWebAPI.Dominio.Entidades.UsuarioEmpreendedor;

namespace HidroWebAPI.Infraestrutura.BancoDados.Repositorios
{
    public class ReservatorioHidroRepositorio : IReservatorioHidroRepositorio
    {
        private readonly EntityRepositorioBase<ReservatorioHidroEntidade> reservatorioHidroRepositorio;

        public ReservatorioHidroRepositorio(HidroContexto contextdb)
        {
            reservatorioHidroRepositorio = new EntityRepositorioBase<ReservatorioHidroEntidade>(contextdb);
        }

        public IEnumerable<ReservatorioHidroEntidade> ListarPorIdEmpreendedor(int IdEmpreendedor) 
        {
            HidroContexto reservatorioHidroContexto = reservatorioHidroRepositorio.context;

            IEnumerable<ReservatorioHidroEntidade> enumReservatorioHidro =
                            (from reservatorioHidro in reservatorioHidroContexto.ReservatorioHidro
                             where reservatorioHidro.IdEmpreendedor == IdEmpreendedor
                             select reservatorioHidro);

            return enumReservatorioHidro;
        }

        public IEnumerable<ReservatorioHidroEntidade> ListarPorIdUsuario(int IdUsuario) 
        {
            HidroContexto reservatorioHidroContexto = reservatorioHidroRepositorio.context;

            IEnumerable<ReservatorioHidroEntidade> enumReservatorioHidro =
                            (from reservatorioHidro in reservatorioHidroContexto.ReservatorioHidro
                             join usuarioEmpreendedor in reservatorioHidroContexto.UsuarioEmpreendedor on reservatorioHidro.IdEmpreendedor equals usuarioEmpreendedor.IdEmpreendedor
                             where usuarioEmpreendedor.IdUsuario == IdUsuario
                             select reservatorioHidro);

            return enumReservatorioHidro;
        }

    }
}
