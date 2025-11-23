using CineReview.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CineReview.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

      
        public DbSet<Midia> Midias { get; set; } 
        public DbSet<Temporada> Temporadas { get; set; }
        public DbSet<Episodio> Episodios { get; set; }

        public DbSet<Equipe> Equipes { get; set; } 

        public DbSet<Avaliacao> Avaliacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Usuario>()
                .HasDiscriminator<string>("TipoUsuario")
                .HasValue<Usuario>("Comum")
                .HasValue<Administrador>("Admin");

            modelBuilder.Entity<Midia>()
                .HasDiscriminator<string>("TipoMidia")
                .HasValue<Filme>("Filme")
                .HasValue<Serie>("Serie");

            modelBuilder.Entity<Equipe>()
                .HasDiscriminator<string>("TipoMembro")
                .HasValue<Ator>("Ator")
                .HasValue<EquipeTecnica>("Tecnico");

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Favoritos)
                .WithMany();
            
            modelBuilder.Entity<Serie>()
                .HasMany(s => s.Temporadas)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Temporada>()
                .HasMany(t => t.Episodios)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Avaliacao>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.Avaliacoes)
                .HasForeignKey(a => a.UsuarioId);
        }
    }
}
