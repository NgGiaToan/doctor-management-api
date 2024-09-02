using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class DoctorManagementContext: DbContext
    {
        public DoctorManagementContext(DbContextOptions options) : base(options) { }

        #region DbSet
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        #endregion
    }
}
