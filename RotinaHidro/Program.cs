using HidroWebAPI.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RotinaHidro.ExecucaoRotina;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RotinaHidro
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12))
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }

            ServiceProvider serviceProviderChesf = new ServiceCollection()
            .AddDbContext<HidroContexto>(opt => opt.UseSqlServer("#{ConnectionString}#"))
            .BuildServiceProvider();

            HidroContexto contextChesf = serviceProviderChesf.GetRequiredService<HidroContexto>();

            new EnvioDadoInstrumento(contextChesf).ExecutarRotina();

            Console.WriteLine("FIM - Execução da Rotina");
            System.Threading.Thread.Sleep(5000);
        }
    }
}
