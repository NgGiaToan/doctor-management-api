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

        public static int PAGE_SIZE { get; set; } = 5;
        public PatientRespository(DoctorManagementContext context) 
        {
            _context = context;
        }
        public List<PatientModel> GetAll(string search, string sortBy, int page =1, int page_size = 5)
        {
            var allPatient = _context.Patients.AsQueryable();

            #region Filtering
            if (!string.IsNullOrWhiteSpace(search))
            {
                allPatient = allPatient.Where(pt => pt.FullName.Contains(search));
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
            allPatient = allPatient.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = allPatient.Select(pt => new PatientModel
            {
                Status = pt.Status,
                Gender = pt.Gender,
                Avatar = pt.Avatar,
                FullName = pt.FullName,
                Date = pt.Date,
                Location = pt.Location,
                Email = pt.Email,
                PhoneNumber = pt.PhoneNumber,
                Diseases = pt.Diseases,
                PaymentId = pt.PaymentId

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
                    Location = patient.Location,
                    Email = patient.Email,
                    PhoneNumber = patient.PhoneNumber,
                    Diseases = patient.Diseases,
                    PaymentId = patient.PaymentId
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
                Status = patient.Status,
                Gender = patient.Gender,
                Avatar = patient.Avatar,
                FullName = patient.FullName,
                Date = patient.Date,
                Location = patient.Location,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                Diseases = patient.Diseases,
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
    }
}
