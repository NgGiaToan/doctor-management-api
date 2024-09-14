using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.DbContexts
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public string PaymentName { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
    }
}
