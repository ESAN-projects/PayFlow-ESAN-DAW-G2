using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Infrastructure.Data;

public partial class PayflowContext : DbContext
{
    public PayflowContext()
    {
    }

    public PayflowContext(DbContextOptions<PayflowContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administradores> Administradores { get; set; }

    public virtual DbSet<Cuentas> Cuentas { get; set; }

    public virtual DbSet<HistorialValidaciones> HistorialValidaciones { get; set; }

    public virtual DbSet<Notificacion> Notificaciones { get; set; }

    public virtual DbSet<Transacciones> Transacciones { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administradores>(entity =>
        {
            entity.HasKey(e => e.AdministradorId).HasName("PK__Administ__2C780D56C2D3D81F");

            entity.HasIndex(e => e.CorreoElectronico, "UQ__Administ__531402F33C975609").IsUnique();

            entity.Property(e => e.AdministradorId).HasColumnName("AdministradorID");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContraseñaHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstadoAdministrador)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Activo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql( "(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cuentas>(entity =>
        {
            entity.HasKey(e => e.CuentaId).HasName("PK__Cuentas__40072EA14095E801");

            entity.HasIndex(e => e.UsuarioId, "UQ__Cuentas__2B3DE799A0C5FEB6").IsUnique();

            entity.HasIndex(e => e.NumeroCuenta, "UQ__Cuentas__E039507BB23849BD").IsUnique();

            entity.Property(e => e.CuentaId).HasColumnName("CuentaID");
            entity.Property(e => e.EstadoCuenta)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Activo");
            entity.Property(e => e.NumeroCuenta)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Saldo).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithOne(p => p.Cuentas)
                .HasForeignKey<Cuentas>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cuentas_Usuarios");
        });

        modelBuilder.Entity<HistorialValidaciones>(entity =>
        {
            entity.HasKey(e => e.ValidacionId).HasName("PK__Historia__88272E765FE1CAD6");

            entity.Property(e => e.ValidacionId).HasColumnName("ValidacionID");
            entity.Property(e => e.AdministradorId).HasColumnName("AdministradorID");
            entity.Property(e => e.Comentarios).HasColumnType("text");
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Resultado)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TipoValidacion)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");

            entity.HasOne(d => d.Administrador).WithMany(p => p.HistorialValidaciones)
                .HasForeignKey(d => d.AdministradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Validaciones_Admin");

            entity.HasOne(d => d.Transaccion).WithMany(p => p.HistorialValidaciones)
                .HasForeignKey(d => d.TransaccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Validaciones_Transacciones");
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasKey(e => e.NotificacionId).HasName("PK__Notifica__BCC120C45278CBAD");

            entity.Property(e => e.NotificacionId).HasColumnName("NotificacionID");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("No Leido");
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mensaje).HasColumnType("text");
            entity.Property(e => e.TipoNotificacion)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Transaccion).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.TransaccionId)
                .HasConstraintName("FK_Notificaciones_Transacciones");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificaciones_Usuarios");
        });

        modelBuilder.Entity<Transacciones>(entity =>
        {
            entity.HasKey(e => e.TransaccionId).HasName("PK__Transacc__86A849DE80BF3261");

            entity.HasIndex(e => new { e.FechaHora, e.Estado }, "IX_Transacciones_FechaEstado");

            entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");
            entity.Property(e => e.Banco)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Comentario)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ComentariosAdmin)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CuentaDestinoId).HasColumnName("CuentaDestinoID");
            entity.Property(e => e.CuentaId).HasColumnName("CuentaID");
            entity.Property(e => e.Estado)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Iporigen)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("IPOrigen");
            entity.Property(e => e.Monto).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.NumeroOperacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RutaVoucher)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TipoTransaccion)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CuentaDestino).WithMany(p => p.TransaccionesCuentaDestino)
                .HasForeignKey(d => d.CuentaDestinoId)
                .HasConstraintName("FK_Transacciones_CuentaDestino");

            entity.HasOne(d => d.Cuenta).WithMany(p => p.TransaccionesCuenta)
                .HasForeignKey(d => d.CuentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transacciones_Cuentas");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798B10EEEA7");

            entity.HasIndex(e => e.CorreoElectronico, "UQ__Usuarios__531402F36DFEEA3A").IsUnique();

            entity.HasIndex(e => e.Dni, "UQ__Usuarios__C035B8DD729A7553").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContraseñaHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DNI");
            entity.Property(e => e.EstadoUsuario)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Activo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
