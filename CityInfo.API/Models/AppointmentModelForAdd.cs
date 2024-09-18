namespace CityInfo.API.Models
{
    public class AppointmentModelForAdd
    {
        public DateTime AppoitmentTime { get; set; }
        public string AppoitmentStatus { get; set; }
        public Guid PatientId { get; set; }
    }
}
