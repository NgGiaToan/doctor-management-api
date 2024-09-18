using AutoMapper.Internal;
using CityInfo.API.DbContexts;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRespository _appointmentRespository;

        public AppointmentController(IAppointmentRespository appointmentRespository, IEmailSender service)
        {
            _appointmentRespository = appointmentRespository;
            this.emailService = service;
        }

        [HttpGet]

        public IActionResult GetAllAppointment(string search = "", string sortBy = "", int page = 1, int page_size = 5)
        {
            try
            {
                var result = _appointmentRespository.GetAll(search, sortBy, page, page_size);
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
                var data = _appointmentRespository.GetById(id);
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
        public IActionResult Update(Guid id, AppointmentVM appointment)
        {
            if (id != appointment.AppoitmentId)
            {
                return BadRequest();
            }
            try
            {
                _appointmentRespository.Update(appointment);
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
                _appointmentRespository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private readonly IEmailSender emailService;

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail(AppointmentModelForAdd appointment)
        {
            try
            {
                await _appointmentRespository.Add(appointment);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AppointmentModelForAdd appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Appointment information is required.");
            }

            try
            {
                var result = await _appointmentRespository.Add(appointment);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding appointment: {ex.Message}");
            }
        }
    }
}
