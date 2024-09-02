using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.DbContexts
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public Guid PaymentId { get; set; }
        [Required]
        [MaxLength(50)]
        public string PaymentName { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
    }
}
