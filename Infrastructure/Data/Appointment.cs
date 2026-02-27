using System;
using System.Collections.Generic;

namespace Telemedicine.API.Infrastructure.Data;
public enum AppointmentStatus
{
    Scheduled = 1,
    Completed = 2,
    Canceled = 3
}
public partial class Appointment
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public AppointmentStatus Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<WaitingRoom> WaitingRooms { get; set; } = new List<WaitingRoom>();
}
