using CityInfo.API.DbContexts;
using CityInfo.API.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CityInfo.API.Services
{
    public class PatientRespository : IPatientRespository
    {
        private readonly DoctorManagementContext _context;

        public PatientRespository(DoctorManagementContext context) 
        {
            _context = context;
        }
        public static int PAGE_SIZE { get; set; } = 5;
        public List<PatientVM> GetAll(string search, string sortBy, int page =1, int page_size = 5)
        {
            var allPatient = _context.Patients.Include(pt => pt.Payment).AsQueryable();

            #region Filtering
            if (!string.IsNullOrWhiteSpace(search))
            {
                allPatient = allPatient.Where(pt => pt.FullName.Contains(search) || pt.PatientId.ToString().Contains(search));
            }
            #endregion

            #region Sorting
            allPatient = allPatient.OrderBy(pt => pt.FullName);

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc": allPatient = allPatient.OrderByDescending(pt => pt.FullName); break;
                    case "date_asc": allPatient = allPatient.OrderBy(pt => pt.Date); break;
                    case "date_desc": allPatient = allPatient.OrderByDescending(pt => pt.Date); break;
                }
            }
            #endregion

            #region Paging
            if (!int.IsNegative(page_size))
            {
                PAGE_SIZE = page_size;
            }
            if (PAGE_SIZE != 0)
            {
                allPatient = allPatient.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            }
            #endregion

            var result = allPatient.Select(pt => new PatientVM
            {
                PatientId = pt.PatientId,
                Status = pt.Status,
                Gender = pt.Gender,
                Avatar = pt.Avatar,
                FullName = pt.FullName,
                Date = pt.Date,
                Time = pt.Time,
                Location = pt.Location,
                Email = pt.Email,
                PhoneNumber = pt.PhoneNumber,
                Diseases = pt.Diseases,
                PaymentId = pt.PaymentId,
                PaymentName = pt.Payment.PaymentName,
                Age = DateTime.Now.Year - pt.Date.Year -
                    (DateTime.Now < pt.Date.AddYears(DateTime.Now.Year - pt.Date.Year) ? 1 : 0)
            });

            return result.ToList();
            //#region Paging
            //var result = PaginatedList<Patient>.Create(allPatient, page, PAGE_SIZE);
            //#endregion

            //return result.Select(pt=> new PatientModel
            //{
            //    Status = pt.Status,
            //    Gender = pt.Gender,
            //    Avatar = pt.Avatar,
            //    FullName = pt.FullName,
            //    Date = pt.Date,
            //    Location = pt.Location,
            //    Email = pt.Email,
            //    PhoneNumber = pt.PhoneNumber,
            //    Diseases = pt.Diseases,
            //    PaymentId = pt.PaymentId,
            //    PaymentName = pt.Payment?.PaymentName

            //}).ToList();

        }

        public PatientModel GetById(Guid id)
        {
            var patient = _context.Patients.SingleOrDefault(pt => pt.PatientId == id);
            if (patient != null)
            {
                return new PatientModel
                {
                    Status = patient.Status,
                    Gender = patient.Gender,
                    Avatar =patient.Avatar,
                    FullName = patient.FullName,
                    Date = patient.Date,
                    Time = patient.Time,
                    Location = patient.Location,
                    Email = patient.Email,
                    PhoneNumber = patient.PhoneNumber,
                    Diseases = patient.Diseases,
                    PaymentId = patient.PaymentId,
                    Age = DateTime.Now.Year - patient.Date.Year -
                        (DateTime.Now < patient.Date.AddYears(DateTime.Now.Year - patient.Date.Year) ? 1 : 0)
                };
            }
            return null;
        }

        public void Update(PatientVM patient)
        {

            var _patient = _context.Patients.SingleOrDefault(pt => pt.PatientId == patient.PatientId);
            if (_patient != null)
            {
                _patient.Status = patient.Status;
                _patient.Gender = patient.Gender;
                _patient.Avatar = patient.Avatar;
                _patient.FullName = patient.FullName;
                _patient.Date = patient.Date;
                _patient.Time = patient.Time;
                _patient.Location = patient.Location;
                _patient.Email = patient.Email;
                _patient.PhoneNumber = patient.PhoneNumber;
                _patient.Diseases = patient.Diseases;
                _patient.PaymentId = patient.PaymentId;

                _context.SaveChanges();
            }
            

        }

        public PatientModel Add(PatientModel patient)
        {
            var _patient = new  Patient
            {
                Status = patient.Status ?? "",
                Gender = patient.Gender ?? "",
                Avatar = patient.Avatar ?? "",
                FullName = patient.FullName ?? "",
                Date = patient.Date,
                Time = patient.Time,
                Location = patient.Location ?? "",
                Email = patient.Email ?? "",
                PhoneNumber = patient.PhoneNumber ?? "",
                Diseases = patient.Diseases ?? "",
                PaymentId = patient.PaymentId
            };
            _context.Add(_patient);
            _context.SaveChanges();

            return new PatientModel
            {
                Status = patient.Status,
                Gender = patient.Gender,
                Avatar = patient.Avatar,
                FullName = patient.FullName,
                Date = patient.Date,
                Time = patient.Time,
                Location = patient.Location,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                Diseases = patient.Diseases,
                PaymentId = patient.PaymentId
            };
        }

        public void Delete(Guid id)
        {
            var patient = _context.Patients.SingleOrDefault(pt => pt.PatientId == id);
            if (patient != null)
            {
                _context.Remove(patient);
                _context.SaveChanges();
            }
        }

        public Dictionary<string, int> PatientChart(int n)
        {
            var allPatient = _context.Patients.AsQueryable();
            var patientBefore = allPatient.Where(p => p.Time.Year < n).ToList();
            var patientInYear = allPatient.Where(p => p.Time.Year == n).ToList();
            var maleCount = patientInYear.Where(p => p.Gender == "Male").ToList();
            var femaleCount = patientInYear.Where(p => p.Gender == "Female").ToList();
            var childCount = patientInYear.Where(p => p.Gender == "Child").ToList();

            return new Dictionary<string, int>
            {
                { "total", allPatient.Count() },
                { "beforeYear", patientBefore.Count() },
                { "inYear", patientInYear.Count() },
                { "maleCount", maleCount.Count() },
                { "femaleCount", femaleCount.Count() },
                { "childCount", childCount.Count() }
            };
        } 
    }
}
