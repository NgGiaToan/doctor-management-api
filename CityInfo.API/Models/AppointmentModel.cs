using CityInfo.API.DbContexts;
using System.Globalization;

namespace CityInfo.API.Models
{
    public class AppointmentModel
    {
        public Guid AppointmentId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string AppointmentStatus { get; set; }
        public string AppointmentComment { get; set; }
        public string AppointmentConfirmed { get; set; }
        public string PatientStatus { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public DateTime Date { get; set; }
        public string Diseases { get; set; }
        public int Age { get; set; }
    }

    
    
}
