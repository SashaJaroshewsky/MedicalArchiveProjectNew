using MedicalArchive.API.Application.DTOs.ReferralDTOs;
using MedicalArchive.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalArchive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReferralsController : ControllerBase
    {
        private readonly IReferralService _referralService;

        public ReferralsController(IReferralService referralService)
        {
            _referralService = referralService;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<ReferralDto>>> GetAllReferrals()
        {
            var referrals = await _referralService.GetAllReferralsAsync();
            return Ok(referrals);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ReferralDto>>> GetReferralsByUserId(int userId)
        {
            var referrals = await _referralService.GetReferralsByUserIdAsync(userId);
            return Ok(referrals);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReferralDto>> GetReferralById(int id)
        {
            var referral = await _referralService.GetReferralByIdAsync(id);
            if (referral == null)
            {
                return NotFound($"Направлення з ID {id} не знайдено");
            }

            return Ok(referral);
        }

        [HttpPost]
        public async Task<ActionResult<ReferralDto>> CreateReferral([FromBody] ReferralCreateDto referralCreateDto)
        {
            try
            {
                var referral = await _referralService.CreateReferralAsync(referralCreateDto);
                return CreatedAtAction(nameof(GetReferralById), new { id = referral.Id }, referral);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReferral(int id, [FromBody] ReferralCreateDto referralUpdateDto)
        {
            try
            {
                await _referralService.UpdateReferralAsync(id, referralUpdateDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReferral(int id)
        {
            try
            {
                await _referralService.DeleteReferralAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
