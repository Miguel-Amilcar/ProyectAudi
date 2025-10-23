using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProyectAudi.Models;

namespace ProyectAudi.Data
{
    public partial class ProyectDbContext : DbContext
    {
        public ProyectDbContext() { }

        public ProyectDbContext(DbContextOptions<ProyectDbContext> options)
            : base(options) { }

        public virtual DbSet<CREDENCIAL> CREDENCIAL { get; set; }
        public virtual DbSet<OPERACION> OPERACION { get; set; }
        public virtual DbSet<PERMISO> PERMISO { get; set; }
        public virtual DbSet<ROL> ROL { get; set; }
        public virtual DbSet<ROL_PERMISO> ROL_PERMISO { get; set; }
        public virtual DbSet<TABLAS_AUDITABLE> TABLAS_AUDITABLE { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<VW_Auditoria_Global> VW_Auditoria_Global { get; set; }
        public virtual DbSet<TOKEN_RECUPERACION> TOKEN_RECUPERACION { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Name=DefaultConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CREDENCIAL>(entity =>
            {
                entity.HasKey(e => e.CREDENCIAL_ID).HasName("PK_CREDENCIALES");

                entity.ToTable(tb => tb.HasTrigger("TRG_CREDENCIAL_AUDITORIA"));

                entity.HasOne(d => d.USUARIO)
                    .WithOne(p => p.CREDENCIAL)
                    .HasForeignKey<CREDENCIAL>(d => d.USUARIO_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CREDENCIAL_USUARIO");
            });

            modelBuilder.Entity<OPERACION>(entity =>
            {
                entity.HasKey(e => e.OPERACION_ID).HasName("PK__OPERACIO__BA24BAFC7C24E0A6");
            });

            modelBuilder.Entity<PERMISO>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("TRG_PERMISO_AUDITORIA"));

                entity.Property(e => e.FECHA_CREACION).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ROL>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("TRG_ROL_AUDITORIA"));

                entity.Property(e => e.FECHA_CREACION).HasDefaultValueSql("(getdate())");

                modelBuilder.Entity<USUARIO>()
                    .HasOne(u => u.ROL)
                    .WithMany(r => r.USUARIOS)
                    .HasForeignKey(u => u.ROL_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ROL_PERMISO>(entity =>
            {
                entity.HasKey(e => new { e.ROL_ID, e.PERMISO_ID });

                entity.HasOne(e => e.ROL)
                    .WithMany(r => r.ROL_PERMISOS)
                    .HasForeignKey(e => e.ROL_ID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.PERMISO)
                    .WithMany(p => p.ROL_PERMISOS)
                    .HasForeignKey(e => e.PERMISO_ID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TABLAS_AUDITABLE>(entity =>
            {
                entity.HasKey(e => e.TABLA_ID).HasName("PK__TABLAS_A__24101E37A544B8B4");

                entity.Property(e => e.ACTIVO).HasDefaultValue(true);
            });

            modelBuilder.Entity<USUARIO>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("TRG_USUARIO_AUDITORIA_COMPLETA"));

                entity.Property(e => e.ESTADO_TINY).HasDefaultValue((byte)1);
                entity.Property(e => e.FECHA_CREACION).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<VW_Auditoria_Global>(entity =>
            {
                entity.ToView("VW_Auditoria_Global");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
