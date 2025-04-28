using MedicalArchive.API.Application.DTOs.VaccinationDTOs;
using MedicalArchive.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalArchive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VaccinationsController : ControllerBase
    {
        private readonly IVaccinationService _vaccinationService;

        public VaccinationsController(IVaccinationService vaccinationService)
        {
            _vaccinationService = vaccinationService;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<VaccinationDto>>> GetAllVaccinations()
        {
            var vaccinations = await _vaccinationService.GetAllVaccinationsAsync();
            return Ok(vaccinations);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<VaccinationDto>>> GetVaccinationsByUserId(int userId)
        {
            var vaccinations = await _vaccinationService.GetVaccinationsByUserIdAsync(userId);
            return Ok(vaccinations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VaccinationDto>> GetVaccinationById(int id)
        {
            var vaccination = await _vaccinationService.GetVaccinationByIdAsync(id);
            if (vaccination == null)
            {
                return NotFound($"Вакцинацію з ID {id} не знайдено");
            }

            return Ok(vaccination);
        }

        [HttpPost]
        public async Task<ActionResult<VaccinationDto>> CreateVaccination([FromBody] VaccinationCreateDto vaccinationCreateDto)
        {
            try
            {
                var vaccination = await _vaccinationService.CreateVaccinationAsync(vaccinationCreateDto);
                return CreatedAtAction(nameof(GetVaccinationById), new { id = vaccination.Id }, vaccination);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVaccination(int id, [FromBody] VaccinationCreateDto vaccinationUpdateDto)
        {
            try
            {
                await _vaccinationService.UpdateVaccinationAsync(id, vaccinationUpdateDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaccination(int id)
        {
            try
            {
                await _vaccinationService.DeleteVaccinationAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
