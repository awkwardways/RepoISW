using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PROYECTOISW.Models;

public partial class ProyectoiswContext : DbContext
{
    public ProyectoiswContext()
    {
    }

    public ProyectoiswContext(DbContextOptions<ProyectoiswContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<Duda> Dudas { get; set; }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    public virtual DbSet<Imagene> Imagenes { get; set; }

    public virtual DbSet<Propiedade> Propiedades { get; set; }

    public virtual DbSet<Rentada> Rentadas { get; set; }

    public virtual DbSet<Reseña> Reseñas { get; set; }

    public virtual DbSet<Respuesta> Respuestas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=PROYECTOISW;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.IdCitas).HasName("PK__Citas__84CC8C339210C5F0");

            entity.Property(e => e.IdCitas).HasColumnName("Id_Citas");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Creacion");
            entity.Property(e => e.IdPropiedad).HasColumnName("Id_Propiedad");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdPropiedadNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdPropiedad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Citas__Id_Propie__4B7734FF");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Citas__Id_Usuari__4C6B5938");
        });

        modelBuilder.Entity<Duda>(entity =>
        {
            entity.HasKey(e => e.IdDuda).HasName("PK__Dudas__130325564B47EA1B");

            entity.Property(e => e.IdDuda).HasColumnName("Id_Duda");
            entity.Property(e => e.Duda1)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("Duda");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.IdPropiedad).HasColumnName("Id_Propiedad");

            entity.HasOne(d => d.IdPropiedadNavigation).WithMany(p => p.Duda)
                .HasForeignKey(d => d.IdPropiedad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dudas__Id_Propie__681373AD");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.IdFavorito).HasName("PK__Favorito__6DACC00D2594EED4");

            entity.Property(e => e.IdFavorito).HasColumnName("Id_Favorito");
            entity.Property(e => e.IdPropiedad).HasColumnName("Id_Propiedad");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdPropiedadNavigation).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.IdPropiedad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favoritos__Id_Pr__44CA3770");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favoritos__Id_Pr__43D61337");
        });

        modelBuilder.Entity<Imagene>(entity =>
        {
            entity.HasKey(e => e.IdFoto).HasName("PK__Imagenes__E107B44E300908DC");

            entity.Property(e => e.IdFoto).HasColumnName("Id_Foto");
            entity.Property(e => e.IdPropiedad).HasColumnName("Id_Propiedad");

            entity.HasOne(d => d.IdPropiedadNavigation).WithMany(p => p.Imagenes)
                .HasForeignKey(d => d.IdPropiedad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Imagenes__Imagen__40F9A68C");
        });

        modelBuilder.Entity<Propiedade>(entity =>
        {
            entity.HasKey(e => e.IdPropiedad).HasName("PK__Propieda__5D2875B319ABC1C9");

            entity.Property(e => e.IdPropiedad).HasColumnName("Id_Propiedad");
            entity.Property(e => e.CondicionesEspeciales)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Condiciones_Especiales");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(600)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Publicacion");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.NumeroBaños)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Numero_Baños");
            entity.Property(e => e.NumeroHabitaciones)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Numero_Habitaciones");
            entity.Property(e => e.PrecioRenta).HasColumnName("Precio_Renta");
            entity.Property(e => e.Servicios)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Superficie)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.TipoPropiedad)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Tipo_Propiedad");
            entity.Property(e => e.Titulo)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Propiedades)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Propiedad__Fecha__3E1D39E1");
        });

        modelBuilder.Entity<Rentada>(entity =>
        {
            entity.HasKey(e => e.IdRentada).HasName("PK__Rentadas__8FFC224F59D05A30");

            entity.Property(e => e.IdRentada).HasColumnName("Id_Rentada");
            entity.Property(e => e.IdPropiedad).HasColumnName("Id_Propiedad");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdPropiedadNavigation).WithMany(p => p.Rentada)
                .HasForeignKey(d => d.IdPropiedad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rentadas__Id_Pro__503BEA1C");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Rentada)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rentadas__Fecha__4F47C5E3");
        });

        modelBuilder.Entity<Reseña>(entity =>
        {
            entity.HasKey(e => e.IdReseña).HasName("PK__Reseñas__D2EBE05C51B53586");

            entity.Property(e => e.IdReseña).HasColumnName("Id_Reseña");
            entity.Property(e => e.Comentario)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnName("Fecha_Creacion");
            entity.Property(e => e.IdPropiedad).HasColumnName("Id_Propiedad");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdPropiedadNavigation).WithMany(p => p.Reseñas)
                .HasForeignKey(d => d.IdPropiedad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reseñas__Id_Prop__489AC854");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Reseñas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reseñas__Id_Usua__47A6A41B");
        });

        modelBuilder.Entity<Respuesta>(entity =>
        {
            entity.HasKey(e => e.IdRespuesta).HasName("PK__Respuest__4F54537D776B4D22");

            entity.Property(e => e.IdRespuesta).HasColumnName("Id_Respuesta");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.IdDuda).HasColumnName("Id_Duda");
            entity.Property(e => e.Respuesta1)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("Respuesta");

            entity.HasOne(d => d.IdDudaNavigation).WithMany(p => p.Respuesta)
                .HasForeignKey(d => d.IdDuda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Respuesta__Id_Du__6AEFE058");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__63C76BE253D340F0");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("Correo_Electronico");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("Nombre_Completo");
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}