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

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WaitingRoom> WaitingRooms { get; set; }



}
