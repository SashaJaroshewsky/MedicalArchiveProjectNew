using MedicalArchive.API.Application.DTOs.PrescriptionDTOs;
using MedicalArchive.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalArchive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetAllPrescriptions()
        {
            var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();
            return Ok(prescriptions);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptionsByUserId(int userId)
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsByUserIdAsync(userId);
            return Ok(prescriptions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDto>> GetPrescriptionById(int id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
            if (prescription == null)
            {
                return NotFound($"Рецепт з ID {id} не знайдено");
            }

            return Ok(prescription);
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionDto>> CreatePrescription([FromBody] PrescriptionCreateDto prescriptionCreateDto)
        {
            try
            {
                var prescription = await _prescriptionService.CreatePrescriptionAsync(prescriptionCreateDto);
                return CreatedAtAction(nameof(GetPrescriptionById), new { id = prescription.Id }, prescription);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(int id, [FromBody] PrescriptionCreateDto prescriptionUpdateDto)
        {
            try
            {
                await _prescriptionService.UpdatePrescriptionAsync(id, prescriptionUpdateDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            try
            {
                await _prescriptionService.DeletePrescriptionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
