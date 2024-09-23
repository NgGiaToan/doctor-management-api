namespace CityInfo.API.DbContexts
{
    public class Doctor
    {
        public Guid DoctorId { get; set; }
        public string DoctorName {  get; set; }
        public string DoctorDepartment { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
