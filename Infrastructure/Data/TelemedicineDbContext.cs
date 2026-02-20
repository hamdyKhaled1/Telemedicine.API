using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Telemedicine.API.Infrastructure.Data;

public partial class TelemedicineDbContext : DbContext
{
    public TelemedicineDbContext()
    {
    }

    public TelemedicineDbContext(DbContextOptions<TelemedicineDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<WaitingRoom> WaitingRooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=TelemedicineDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appointm__3214EC07683B45B3");

            entity.HasIndex(e => new { e.DoctorId, e.StartTime, e.EndTime }, "Appointments_Doctor_Time");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Scheduled");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Patient");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Doctors__3214EC0750EC2B12");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Specialty).HasMaxLength(100);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Patients__3214EC07A66E7D2C");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<WaitingRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WaitingR__3214EC0709AFE3AD");

            entity.Property(e => e.IsPatientJoined).HasDefaultValue(false);

            entity.HasOne(d => d.Appointment).WithMany(p => p.WaitingRooms)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WaitingRooms_Appointments");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
