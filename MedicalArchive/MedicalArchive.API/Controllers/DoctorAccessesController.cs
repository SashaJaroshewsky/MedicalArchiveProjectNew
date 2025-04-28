using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalArchive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DoctorAccessesController : ControllerBase
    {
        private readonly IDoctorAccessService _doctorAccessService;

        public DoctorAccessesController(IDoctorAccessService doctorAccessService)
        {
            _doctorAccessService = doctorAccessService;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<DoctorAccessDto>>> GetAllDoctorAccesses()
        {
            var accesses = await _doctorAccessService.GetAllDoctorAccessesAsync();
            return Ok(accesses);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<DoctorAccessDto>>> GetAccessesByUserId(int userId)
        {
            var accesses = await _doctorAccessService.GetDoctorAccessesByUserIdAsync(userId);
            return Ok(accesses);
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<DoctorAccessDto>>> GetAccessesByDoctorId(int doctorId)
        {
            var accesses = await _doctorAccessService.GetDoctorAccessesByDoctorIdAsync(doctorId);
            return Ok(accesses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorAccessDto>> GetAccessById(int id)
        {
            var access = await _doctorAccessService.GetDoctorAccessByIdAsync(id);
            if (access == null)
            {
                return NotFound($"Доступ з ID {id} не знайдено");
            }

            return Ok(access);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<DoctorAccessDto>> CreateAccess([FromBody] DoctorAccessCreateDto accessCreateDto)
        {
            try
            {
                var access = await _doctorAccessService.CreateDoctorAccessAsync(accessCreateDto);
                return CreatedAtAction(nameof(GetAccessById), new { id = access.Id }, access);
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
        public async Task<IActionResult> DeleteAccess(int id)
        {
            try
            {
                await _doctorAccessService.DeleteDoctorAccessAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
