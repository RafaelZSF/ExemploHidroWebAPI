using HidroWebAPI.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Infraestrutura.Contexto
{
    public partial class HidroContexto : DbContext
    {
        public HidroContexto() { }

        public HidroContexto(DbContextOptions<HidroContexto> options) : base(options)
        {
        }

        public virtual DbSet<DadoHidrologico> DadoHidrologico { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<TipoDadoHidrologico> TipoDadoHidrologico { get; set; }
        public virtual DbSet<ReservatorioHidro> ReservatorioHidro { get; set; }
        public virtual DbSet<VS_GESTAO_BARRAGENS> VS_GESTAO_BARRAGENS { get; set; }

        public virtual DbSet<Leitura> Leitura { get; set; }
        public virtual DbSet<Barragem> Barragem { get; set; }
        public virtual DbSet<Instrumento> Instrumento { get; set; }
        public virtual DbSet<UsuarioEmpreendedor> UsuarioEmpreendedor { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DadoHidrologico>(entity =>
            {
                entity.HasKey(e => e.IdDadoHidrologico);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);
            });

            modelBuilder.Entity<TipoDadoHidrologico>(entity =>
            {
                entity.HasKey(e => e.IdTipoDadoHidrologico);
            });

            modelBuilder.Entity<ReservatorioHidro>(entity =>
            {
                entity.HasKey(e => e.IdReservatorioHidro);
            });

            modelBuilder.Entity<VS_GESTAO_BARRAGENS>(entity =>
            {
                entity.HasNoKey();
            });


            modelBuilder.Entity<Leitura>(entity =>
            {
                entity.HasKey(e => e.IdLeitura);
            });

            modelBuilder.Entity<Barragem>(entity =>
            {
                entity.HasKey(e => e.IdBarragem);
            });

            modelBuilder.Entity<Instrumento>(entity =>
            {
                entity.HasKey(e => e.IdInstrumento);
            });

            modelBuilder.Entity<UsuarioEmpreendedor>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpreendedor, e.IdUsuario });
            });
        }
    }
}
