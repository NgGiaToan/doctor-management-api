using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.DbContexts
{
    [Table("Appointment")]
    public class Appointment
    {
        [Key]
        public Guid AppoitmentId { get; set; }
        public string AppoitmentTime { get; set; }
        public string AppoitmentStatus { get; set; }
        public Guid? PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
