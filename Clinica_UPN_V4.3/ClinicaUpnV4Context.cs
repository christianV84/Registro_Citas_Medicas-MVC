using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Clinica_UPN_V4._3;

public partial class ClinicaUpnV4Context : DbContext
{
    public ClinicaUpnV4Context()
    {
    }

    public ClinicaUpnV4Context(DbContextOptions<ClinicaUpnV4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradors { get; set; }

    public virtual DbSet<Citum> Cita { get; set; }

    public virtual DbSet<Consultorio> Consultorios { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

 //   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
 //       => optionsBuilder.UseSqlServer("server=CHRISTIAN\\SQLEXPRESS;database=Clinica_UPN_v4;Trusted_Connection=True; \nTrustServerCertificate=True; Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.UsuarioAdmin).HasName("PK__Administ__5322089BD9FD7F23");

            entity.ToTable("Administrador");

            entity.HasIndex(e => e.CodigoSeguridad, "unique_CodigoSeguridad").IsUnique();

            entity.HasIndex(e => e.UsuarioAdmin, "unique_UsuarioAdmin").IsUnique();

            entity.HasIndex(e => e.Dni, "unique_adm_dni").IsUnique();

            entity.Property(e => e.UsuarioAdmin)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(9)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.NumCita).HasName("PK__Cita__F11A0ACDF45F5AAF");

            entity.HasIndex(e => e.Fecha, "unique_fecha_evento").IsUnique();

            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.UsuarioMedico)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioPaciente)
                .HasMaxLength(10)
                .IsUnicode(false);

            // Configurar exclusión de propiedades FechaSoloFecha y FechaSoloHora
            entity.Ignore(c => c.FechaSoloFecha);
            entity.Ignore(c => c.FechaSoloHora);

            entity.HasOne(d => d.NumConsultorioNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.NumConsultorio)
                .HasConstraintName("FK__Cita__NumConsult__42E1EEFE");

            entity.HasOne(d => d.UsuarioMedicoNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.UsuarioMedico)
                .HasConstraintName("FK__Cita__UsuarioMed__40F9A68C");

            entity.HasOne(d => d.UsuarioPacienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.UsuarioPaciente)
                .HasConstraintName("FK__Cita__UsuarioPac__41EDCAC5");
        });

        modelBuilder.Entity<Consultorio>(entity =>
        {
            entity.HasKey(e => e.NumConsultorio).HasName("PK__Consulto__076B7593EDACCA5F");

            entity.Property(e => e.NumConsultorio).ValueGeneratedNever();
            entity.Property(e => e.EspConsultorio)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.UsuarioMed).HasName("PK__Medico__7F455BA0C12668AA");

            entity.ToTable("Medico");

            entity.HasIndex(e => e.NumColegiatura, "unique_NumColegiatura").IsUnique();

            entity.HasIndex(e => e.UsuarioMed, "unique_UsuarioMed").IsUnique();

            entity.HasIndex(e => e.Dni, "unique_dniMed").IsUnique();

            entity.Property(e => e.UsuarioMed)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Especializacion)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(9)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.UsuarioPac).HasName("PK__Paciente__4B504FE66A7900C0");

            entity.ToTable("Paciente");

            entity.HasIndex(e => e.UsuarioPac, "unique_UsuarioPac").IsUnique();

            entity.HasIndex(e => e.Dni, "unique_dni").IsUnique();

            entity.Property(e => e.UsuarioPac)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(9)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
