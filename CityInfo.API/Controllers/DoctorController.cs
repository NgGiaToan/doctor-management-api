using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRespository _doctorRespository;

        public DoctorController(IDoctorRespository doctorRespository)
        {
            _doctorRespository = doctorRespository;
        }

        [HttpGet]
        public IActionResult GetAllDoctor(string search = "", int page = 1, int page_size = 5)
        {
            try
            {
                var result = _doctorRespository.GetAll(search, page, page_size);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the patient.");
            }
        }

        [HttpGet("{index}")]
        public IActionResult GetByIndex(string index)
        {
            try
            {
                var result = _doctorRespository.GetNameByDeparterment(index);
                return Ok(result);
            }
            catch 
            {
                return BadRequest("We can't get the patient.");
            }

        }

        [HttpPost]
        public IActionResult Add(DoctorModel model)
        {
            try
            {
                return Ok(_doctorRespository.Add(model));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, DoctorVM model)
        {
            if (id != model.DoctorId)
            {
                return BadRequest();
            }

            try
            {
                _doctorRespository.Update(model);
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
                _doctorRespository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
