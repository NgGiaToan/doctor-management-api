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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");
                entity.HasKey(p => p.PatientId);

                entity.Property(p => p.Status)
                      .IsRequired();

                entity.Property(p => p.Gender)
                      .IsRequired();

                entity.Property(p => p.Avatar)
                      .HasMaxLength(255); 

                entity.Property(p => p.FullName)
                      .IsRequired()
                      .HasMaxLength(100); 

                entity.Property(p => p.Date)
                      .IsRequired();

                entity.Property(p => p.Location)
                      .HasMaxLength(255);

                entity.Property(p => p.Email)
                      .HasMaxLength(100); 

                entity.Property(p => p.PhoneNumber)
                      .HasMaxLength(15); 

                entity.Property(p => p.Diseases)
                      .HasMaxLength(500);

                entity.HasOne(p => p.Payment)
                      .WithMany()
                      .HasForeignKey(p => p.PaymentId)
                      .HasConstraintName("FK_Patient_Payment");
                //.OnDelete(DeleteBehavior.SetNull); 

                entity.HasMany(p => p.Appointments)
                      .WithOne(a => a.Patient)
                      .HasForeignKey(a => a.PatientId)
                      .HasConstraintName("FK_Patient_Appoitment");
                      //.OnDelete(DeleteBehavior.Cascade);  
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.HasKey(p => p.PaymentId);

                entity.Property(p => p.PaymentName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasMany(p => p.Patients)
                      .WithOne(pat => pat.Payment)
                      .HasForeignKey(pat => pat.PaymentId)
                      .HasConstraintName("FK_Patient_Payment");
                      //.OnDelete(DeleteBehavior.SetNull);  
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                entity.HasKey(a => a.AppoitmentId);

                entity.Property(a => a.AppoitmentTime)
                      .IsRequired();

                entity.Property(a => a.AppoitmentStatus)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasOne(a => a.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(a => a.PatientId)
                      .HasConstraintName("FK_Patient_Appoitment");
                      //.OnDelete(DeleteBehavior.Cascade);  
            });
        }
    }
}
