using System;
using System.Collections.Generic;

namespace Telemedicine.API.Infrastructure.Data;

public partial class Patient
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
