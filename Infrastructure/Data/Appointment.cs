using System;
using System.Collections.Generic;

namespace Telemedicine.API.Infrastructure.Data;

public partial class Appointment
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<WaitingRoom> WaitingRooms { get; set; } = new List<WaitingRoom>();
}
