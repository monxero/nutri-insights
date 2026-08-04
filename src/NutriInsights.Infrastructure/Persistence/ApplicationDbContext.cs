using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using CategoriaAlimentoEntity = NutriInsights.Domain.CategoriaAlimento.CategoriaAlimento;
using UnidadMedidaEntity = NutriInsights.Domain.UnidadMedida.UnidadMedida;
using CalificadorCantidadEntity = NutriInsights.Domain.CalificadorCantidad.CalificadorCantidad;
using AlimentoEntity = NutriInsights.Domain.Alimento.Alimento;
using AlimentoUnidadEquivalenciaEntity = NutriInsights.Domain.AlimentoUnidadEquivalencia.AlimentoUnidadEquivalencia;
using RegistroEntity = NutriInsights.Domain.Registro.Registro;
using ItemDeRegistroEntity = NutriInsights.Domain.ItemDeRegistro.ItemDeRegistro;
using ObjetivoEntity = NutriInsights.Domain.Objetivo.Objetivo;
using NutriInsights.Domain.CategoriaAlimento;

namespace NutriInsights.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<CategoriaAlimentoEntity> CategoriasAlimento { get; set; }
    public DbSet<UnidadMedidaEntity> UnidadesMedida { get; set; }
    public DbSet<CalificadorCantidadEntity> CalificadoresCantidad { get; set; }
    public DbSet<AlimentoEntity> Alimentos { get; set; }
    public DbSet<AlimentoUnidadEquivalenciaEntity> AlimentosUnidadEquivalencia { get; set; }
    public DbSet<RegistroEntity> Registros { get; set; }
    public DbSet<ItemDeRegistroEntity> ItemsDeRegistro { get; set; }
    public DbSet<ObjetivoEntity> Objetivos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RegistroEntity>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ObjetivoEntity>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(o => o.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AlimentoEntity>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.UsuarioPropietarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CategoriaAlimentoEntity>().HasData(
            new CategoriaAlimentoEntity { Id = CategoriasAlimentoSemilla.ProteinaAnimalId, Nombre = "Proteína animal", PorcionReferenciaGramos = 120 },
            new CategoriaAlimentoEntity { Id = CategoriasAlimentoSemilla.ProteinaVegetalId, Nombre = "Proteína vegetal / legumbres", PorcionReferenciaGramos = 80 },
            new CategoriaAlimentoEntity { Id = CategoriasAlimentoSemilla.VerdurasId, Nombre = "Verduras", PorcionReferenciaGramos = 100 },
            new CategoriaAlimentoEntity { Id = CategoriasAlimentoSemilla.FrutasId, Nombre = "Frutas", PorcionReferenciaGramos = 150 },
            new CategoriaAlimentoEntity { Id = CategoriasAlimentoSemilla.CarbohidratosId, Nombre = "Carbohidratos / cereales", PorcionReferenciaGramos = 150 },
            new CategoriaAlimentoEntity { Id = CategoriasAlimentoSemilla.LacteosId, Nombre = "Lácteos", PorcionReferenciaGramos = 200 },
            new CategoriaAlimentoEntity { Id = CategoriasAlimentoSemilla.GrasasYFrutosSecosId, Nombre = "Grasas y frutos secos", PorcionReferenciaGramos = 30 },
            new CategoriaAlimentoEntity { Id = CategoriasAlimentoSemilla.OtrosProcesadosId, Nombre = "Otros / procesados", PorcionReferenciaGramos = 50 }
        );
    }
}