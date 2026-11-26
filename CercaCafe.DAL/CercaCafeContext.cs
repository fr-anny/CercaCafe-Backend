using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CercaCafe.DAL.Models;

public partial class CercaCafeContext : DbContext
{
    public CercaCafeContext()
    {
    }

    public CercaCafeContext(DbContextOptions<CercaCafeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cafeteria> Cafeterias { get; set; }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Resena> Resenas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=CercaCafe;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cafeteria>(entity =>
        {
            entity.HasKey(e => e.IdCafeteria).HasName("PK__Cafeteri__6DD71762C1E8212F");

            entity.Property(e => e.IdCafeteria).HasColumnName("id_cafeteria");
            entity.Property(e => e.Calificacion)
                .HasColumnType("decimal(2, 1)")
                .HasColumnName("calificacion");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .HasColumnName("direccion");
            entity.Property(e => e.Latitud)
                .HasColumnType("decimal(9, 6)")
                .HasColumnName("latitud");
            entity.Property(e => e.Longitud)
                .HasColumnType("decimal(9, 6)")
                .HasColumnName("longitud");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.TipoCafeteria)
                .HasMaxLength(50)
                .HasColumnName("tipo_cafeteria");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.IdFavorito).HasName("PK__Favorito__6DD71762C1E8212F");

            entity.ToTable("Favoritos");

            entity.Property(e => e.IdFavorito).HasColumnName("id_favorito");
            entity.Property(e => e.FechaGuardado)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_guardado");
            entity.Property(e => e.IdCafeteria).HasColumnName("id_cafeteria");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdCafeteriaNavigation).WithMany()
                .HasForeignKey(d => d.IdCafeteria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favoritos_Cafeterias");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany()
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favoritos_Usuarios");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horarios__C5836D69B9A575C8");

            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.Dia)
                .HasMaxLength(20)
                .HasColumnName("dia");
            entity.Property(e => e.HoraApertura).HasColumnName("hora_apertura");
            entity.Property(e => e.HoraCierre).HasColumnName("hora_cierre");
            entity.Property(e => e.IdCafeteria).HasColumnName("id_cafeteria");

            entity.HasOne(d => d.IdCafeteriaNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.IdCafeteria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Horarios_Cafeterias");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__FF341C0DCDD2C627");

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Categoria)
                .HasMaxLength(50)
                .HasColumnName("categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCafeteria).HasColumnName("id_cafeteria");
            entity.Property(e => e.Imagen)
                .HasMaxLength(250)
                .HasColumnName("imagen");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdCafeteriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCafeteria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Cafeterias");
        });

        modelBuilder.Entity<Resena>(entity =>
        {
            entity.HasKey(e => e.IdResena).HasName("PK__Resenas__06CD9363EB4DCAD5");

            entity.Property(e => e.IdResena).HasColumnName("id_resena");
            entity.Property(e => e.Calificacion).HasColumnName("calificacion");
            entity.Property(e => e.Comentario)
                .HasMaxLength(500)
                .HasColumnName("comentario");
            entity.Property(e => e.FechaResena).HasColumnName("fecha_resena");
            entity.Property(e => e.IdCafeteria).HasColumnName("id_cafeteria");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdCafeteriaNavigation).WithMany(p => p.Resenas)
                .HasForeignKey(d => d.IdCafeteria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resenas_Cafeterias");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Resenas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resenas_Usuarios");

            entity.Property(e => e.FotoUrl)
                .HasMaxLength(500)
                .HasColumnName("foto_url");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__4E3E04AD23D77896");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__2A586E0B2A300A38").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .HasColumnName("correo");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}