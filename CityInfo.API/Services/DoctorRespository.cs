using CityInfo.API.DbContexts;
using CityInfo.API.Models;

namespace CityInfo.API.Services
{
    public class DoctorRespository : IDoctorRespository
    {
        private readonly DoctorManagementContext _context;

        public static int PAGE_SIZE { get; set; } = 5;

        public DoctorRespository(DoctorManagementContext context)
        {
            _context = context;
        }

        public List<DoctorModel> GetAll(string search, int page = 1, int page_size = 5)
        {
            var allDoctor = _context.Doctors.AsQueryable();
            #region Filtering
            if (!string.IsNullOrWhiteSpace(search))
            {
                allDoctor = allDoctor.Where(pt => pt.DoctorName.Contains(search));
            }
            #endregion

            #region Paging
            if (!int.IsNegative(page_size))
            {
                PAGE_SIZE = page_size;
            }
            allDoctor = allDoctor.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion

            var result = allDoctor.Select(dt => new DoctorModel
            {
                DoctorName = dt.DoctorName,
                DoctorDepartment = dt.DoctorDepartment,
            });

            return result.ToList();
        }


        public List<DoctorModelName> GetNameByDeparterment(string departerment)
        {
            var allDoctor = _context.Doctors.AsQueryable();

            allDoctor = allDoctor.Where(dt => dt.DoctorDepartment.Contains(departerment));

            var result = allDoctor.Select(dt => new DoctorModelName
            {
                DoctorName = dt.DoctorName
            });

            return result.ToList();
        }

        public DoctorModel Add(DoctorModel model)
        {
            try
            {
                var _doctor = new Doctor
                {
                    DoctorName = model.DoctorName,
                    DoctorDepartment = model.DoctorDepartment,
                };

                _context.Add(_doctor);
                _context.SaveChangesAsync();

                return new DoctorModel
                {
                    DoctorName = model.DoctorName,
                    DoctorDepartment = model.DoctorDepartment,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding appointment: {ex.Message}");
                throw;
            }
        }

        public void Update(DoctorVM doctor)
        {
            var _doctor = _context.Doctors.SingleOrDefault(dt => dt.DoctorId == doctor.DoctorId);
            if (_doctor != null )
            {
                _doctor.DoctorName = doctor.DoctorName;
                _doctor.DoctorDepartment = doctor.DoctorDepartment;
                
                _context.SaveChanges();
            }
        }
        

        public void Delete(Guid id)
        {
            var doctor = _context.Doctors.SingleOrDefault(dt => dt.DoctorId == id);
            if (doctor != null)
            {
                _context.Remove(doctor);
                _context.SaveChanges();
            }
        }
    }
}
