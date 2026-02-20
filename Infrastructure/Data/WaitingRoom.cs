using System;
using System.Collections.Generic;

namespace Telemedicine.API.Infrastructure.Data;

public partial class WaitingRoom
{
    public int Id { get; set; }

    public int AppointmentId { get; set; }

    public bool? IsPatientJoined { get; set; }

    public DateTime? JoinTime { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;
}
