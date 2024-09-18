using CityInfo.API.DbContexts;

namespace CityInfo.API.Models
{
    public class AppointmentModel
    {
        public DateTime AppoitmentTime { get; set; }
        public string AppoitmentStatus { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public DateTime Date { get; set; }
    }

    
    
}
