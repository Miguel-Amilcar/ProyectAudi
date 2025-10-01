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

        public virtual DbSet<BITACORA_DETALLE> BITACORA_DETALLE { get; set; }
        public virtual DbSet<BITACORA_ENCABEZADO> BITACORA_ENCABEZADO { get; set; }
        public virtual DbSet<CREDENCIAL> CREDENCIAL { get; set; }
        public virtual DbSet<OPERACION> OPERACION { get; set; }
        public virtual DbSet<PERMISO> PERMISO { get; set; }
        public virtual DbSet<ROL> ROL { get; set; }
        public virtual DbSet<TABLAS_AUDITABLE> TABLAS_AUDITABLE { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<VW_Auditoria_Global> VW_Auditoria_Global { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Name=DefaultConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BITACORA_DETALLE>(entity =>
            {
                entity.HasKey(e => e.DETALLE_ID).HasName("PK__BITACORA__83681071ED184B52");

                entity.HasOne(d => d.BITACORA)
                    .WithMany(p => p.BITACORA_DETALLE)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BITACORA___BITAC__7E37BEF6");
            });

            modelBuilder.Entity<BITACORA_ENCABEZADO>(entity =>
            {
                entity.HasKey(e => e.BITACORA_ID).HasName("PK__BITACORA__EF8B7ABEC01C1BBC");

                entity.Property(e => e.FECHA_OPERACION).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.OPERACION)
                    .WithMany(p => p.BITACORA_ENCABEZADO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BITACORA___OPERA__7A672E12");

                entity.HasOne(d => d.TABLA)
                    .WithMany(p => p.BITACORA_ENCABEZADO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BITACORA___TABLA__797309D9");

                entity.HasOne(d => d.USUARIO_AFECTADO)
                    .WithMany(p => p.BITACORA_ENCABEZADOUSUARIO_AFECTADO)
                    .HasConstraintName("FK__BITACORA___USUAR__787EE5A0");

                entity.HasOne(d => d.USUARIO)
                    .WithMany(p => p.BITACORA_ENCABEZADOUSUARIO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BITACORA___USUAR__778AC167");
            });

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

                entity.HasOne(d => d.ROL)
                    .WithMany(p => p.PERMISO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PERMISO_ROL");
            });

            modelBuilder.Entity<ROL>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("TRG_ROL_AUDITORIA"));

                entity.Property(e => e.ESTADO).HasDefaultValue(true);
                entity.Property(e => e.FECHA_CREACION).HasDefaultValueSql("(getdate())");

                modelBuilder.Entity<USUARIO>()
                    .HasOne(u => u.ROL)
                    .WithMany(r => r.USUARIOS)
                    .HasForeignKey(u => u.ROL_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USUARIO_ROL");

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
