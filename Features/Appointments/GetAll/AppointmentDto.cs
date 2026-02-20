namespace Telemedicine.API.Features.Appointments.GetAll
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }
}
