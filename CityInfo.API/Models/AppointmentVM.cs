using CityInfo.API.DbContexts;

namespace CityInfo.API.Models
{
    public class AppointmentVM
    {
        public Guid AppoitmentId { get; set; }
        public DateTime AppoitmentTime { get; set; }
        public string AppoitmentStatus { get; set; }
        
    }
}
