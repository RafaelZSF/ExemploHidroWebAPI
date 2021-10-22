using HidroWebAPI.Aplicacao.Repositorios;
using HidroWebAPI.Infraestrutura.BancoDados.Repositorios;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HidroWebAPI
{
    public static class DependencyInjection
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region services
            #endregion

            #region repository
            services.AddScoped<IDadoHidrologicoRepositorio, DadoHidrologicoRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<ITipoDadoHidrologicoRepositorio, TipoDadoHidrologicoRepositorio>();
            services.AddScoped<IReservatorioHidroRepositorio, ReservatorioHidroRepositorio>();
            services.AddScoped<IViewGestaoBarragensRepositorio, ViewGestaoBarragensRepositorio>();

            services.AddScoped<IBarragemRepositorio, BarragemRepositorio>();
            services.AddScoped<ILeituraRepositorio, LeituraRepositorio>();
            services.AddScoped<IInstrumentoRepositorio, InstrumentoRepositorio>();
            #endregion
        }
    }
}
