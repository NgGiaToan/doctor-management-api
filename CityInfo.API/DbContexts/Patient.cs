using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace CityInfo.API.DbContexts
{
    public class Patient
    {
        public Guid PatientId { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Diseases { get; set; }

        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }

    }
}
