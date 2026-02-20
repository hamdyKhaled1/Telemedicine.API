using System;
using System.Collections.Generic;

namespace Telemedicine.API.Infrastructure.Data;

public partial class Doctor
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Specialty { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
