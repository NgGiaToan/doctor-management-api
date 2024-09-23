using CityInfo.API.DbContexts;

namespace CityInfo.API.Models
{
    public class AppointmentVM
    {
        public Guid AppointmentId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string AppointmentStatus { get; set; }
        public string AppointmentComment { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        
    }
}
