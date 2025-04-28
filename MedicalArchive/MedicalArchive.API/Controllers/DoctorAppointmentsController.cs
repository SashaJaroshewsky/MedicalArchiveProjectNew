using MedicalArchive.API.Application.DTOs.DoctorAppointmentDTOs;
using MedicalArchive.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalArchive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DoctorAppointmentsController : ControllerBase
    {
        private readonly IDoctorAppointmentService _appointmentService;

        public DoctorAppointmentsController(IDoctorAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<DoctorAppointmentDto>>> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllDoctorAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<DoctorAppointmentDto>>> GetAppointmentsByUserId(int userId)
        {
            var appointments = await _appointmentService.GetDoctorAppointmentsByUserIdAsync(userId);
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorAppointmentDto>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.GetDoctorAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound($"Прийом з ID {id} не знайдено");
            }

            return Ok(appointment);
        }

        [HttpPost]
        public async Task<ActionResult<DoctorAppointmentDto>> CreateAppointment([FromBody] DoctorAppointmentCreateDto appointmentCreateDto)
        {
            try
            {
                var appointment = await _appointmentService.CreateDoctorAppointmentAsync(appointmentCreateDto);
                return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] DoctorAppointmentCreateDto appointmentUpdateDto)
        {
            try
            {
                await _appointmentService.UpdateDoctorAppointmentAsync(id, appointmentUpdateDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                await _appointmentService.DeleteDoctorAppointmentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
