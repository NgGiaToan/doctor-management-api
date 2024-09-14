using CityInfo.API.DbContexts;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        //private readonly DoctorManagementContext _context;
        //public PatientController(DoctorManagementContext context)
        //{
        //    _context = context;
        //}

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var patients = _context.Patients.ToList();
        //    return Ok(patients);
        //}

        private readonly IPatientRespository _patientRespository;

        public PatientController(IPatientRespository patientRespository)
        {
            _patientRespository = patientRespository;
        }

        [HttpGet]

        public IActionResult GetAllPatient(string search="", string sortBy = "", int page=1, int page_size=5)
        {
            try
            {
                var result = _patientRespository.GetAll(search, sortBy, page, page_size);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the patient.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var data = _patientRespository.GetById(id);
                if (data != null)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, PatientVM patient)
        {
            if (id != patient.PatientId)
            {
                return BadRequest();
            }
            try
            {
                _patientRespository.Update(patient);
                return NoContent();

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _patientRespository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Add(PatientModel patient)
        {
            try
            {
                return Ok(_patientRespository.Add(patient));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //[HttpGet("{id}")]
        //public IActionResult GetById(Guid id)
        //{
        //    var patient = _context.Patients.SingleOrDefault(lo => lo.PatientId == id);
        //    if (patient != null)
        //    {
        //        return Ok(patient);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //[HttpPost]
        //public IActionResult CreateNew(PatientModel model)
        //{
        //    try
        //    {
        //        var patient = new Patient
        //        {
        //            Status = model.Status,
        //            Gender = model.Gender,
        //            Avatar = model.Avatar,
        //            FullName = model.FullName,
        //            Date = model.Date,
        //            Location = model.Location,
        //            Email = model.Email,
        //            PhoneNumber = model.PhoneNumber,
        //            Diseases = model.Diseases,
        //            PaymentId = model.PaymentId
        //        };
        //        _context.Patients.Add(patient);
        //        _context.SaveChanges();
        //        return Ok();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPut("{id}")]
        //public IActionResult UpdateLoaiById (Guid id, PatientModel model)
        //{
        //    var patient = _context.Patients.SingleOrDefault(loai => loai.PatientId == id);
        //    if (patient != null)
        //    {
        //        patient.Status = model.Status;
        //        patient.Gender = model.Gender;
        //        patient.Avatar = model.Avatar;
        //        patient.FullName = model.FullName;
        //        patient.Date = model.Date;
        //        patient.Location = model.Location;
        //        patient.Email = model.Email;
        //        patient.PhoneNumber = model.PhoneNumber;
        //        patient.Diseases = model.Diseases;
        //        patient.PaymentId = model.PaymentId;
        //        _context.SaveChanges();
        //        return NoContent();
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}
    }
}
