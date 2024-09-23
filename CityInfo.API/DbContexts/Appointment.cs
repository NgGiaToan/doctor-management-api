using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.DbContexts
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string AppointmentStatus { get; set; }
        public string AppointmentComment { get; set; }
        public string AppointmentConfirmed { get; set; }
        public string PatientStatus { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
