using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.DbContexts
{
    public class Appointment
    {
        public Guid AppoitmentId { get; set; }
        public DateTime AppoitmentTime { get; set; }
        public string AppoitmentStatus { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
