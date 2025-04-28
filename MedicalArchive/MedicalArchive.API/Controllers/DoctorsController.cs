using MedicalArchive.API.Application.DTOs.DoctorDTOs;
using MedicalArchive.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalArchive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound($"Лікаря з ID {id} не знайдено");
            }

            return Ok(doctor);
        }

        [HttpPost]
        public async Task<ActionResult<DoctorDto>> CreateDoctor([FromBody] DoctorCreateDto doctorCreateDto)
        {
            try
            {
                var doctor = await _doctorService.CreateDoctorAsync(doctorCreateDto);
                return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, doctor);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorDto doctorDto)
        {
            if (id != doctorDto.Id)
            {
                return BadRequest("ID лікаря не співпадає з ID в URL");
            }

            try
            {
                await _doctorService.UpdateDoctorAsync(id, doctorDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                await _doctorService.DeleteDoctorAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
