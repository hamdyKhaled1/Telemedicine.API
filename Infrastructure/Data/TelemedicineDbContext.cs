using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Telemedicine.API.Features.Account;

namespace Telemedicine.API.Infrastructure.Data;

public partial class TelemedicineDbContext :IdentityDbContext<ApplicationUser>
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // ← مهم جداً لـ Identity

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Status)
                  .HasConversion<int>()
                   .HasDefaultValue(AppointmentStatus.Scheduled);

            entity.Property(e => e.IsDeleted)
                  .HasDefaultValue(false);

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken()
                  .ValueGeneratedOnAddOrUpdate();

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("(getutcdate())");

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(d => d.Doctor)
                  .WithMany(p => p.Appointments)
                  .HasForeignKey(d => d.DoctorId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Appointments_Doctor");

            entity.HasOne(d => d.Patient)
                  .WithMany(p => p.Appointments)
                  .HasForeignKey(d => d.PatientId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Appointments_Patient");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FullName)
                  .HasMaxLength(150);

            entity.Property(e => e.Specialty)
                  .HasMaxLength(100);

            entity.Property(e => e.Email)
                  .HasMaxLength(150);

            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FullName)
                  .HasMaxLength(150);

            entity.Property(e => e.Email)
                  .HasMaxLength(150);

            entity.Property(e => e.Phone)
                  .HasMaxLength(20);
        });

        modelBuilder.Entity<WaitingRoom>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IsPatientJoined)
                  .HasDefaultValue(false);

            entity.HasOne(d => d.Appointment)
                  .WithMany(p => p.WaitingRooms)
                  .HasForeignKey(d => d.AppointmentId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_WaitingRooms_Appointments");
        });

        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


