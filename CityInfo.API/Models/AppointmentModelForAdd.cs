namespace CityInfo.API.Models
{
    public class AppointmentModelForAdd
    {
        public DateTime AppointmentTime { get; set; }
        public string AppointmentStatus { get; set; }
        public string AppointmentComment { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
    }

    
}
