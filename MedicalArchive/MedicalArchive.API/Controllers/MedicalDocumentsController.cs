using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Application.Services;
using MedicalArchive.API.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalArchive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MedicalDocumentsController : ControllerBase
    {
        private readonly IMedicalDocumentService _documentService;

        public MedicalDocumentsController(IMedicalDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<MedicalDocumentDto>>> GetAllDocuments()
        {
            var documents = await _documentService.GetAllDocumentsAsync();
            return Ok(documents);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<MedicalDocumentDto>>> GetDocumentsByUserId(int userId)
        {
            var documents = await _documentService.GetDocumentsByUserIdAsync(userId);
            return Ok(documents);
        }

        [HttpGet("user/{userId}/type/{documentType}")]
        public async Task<ActionResult<IEnumerable<MedicalDocumentDto>>> GetDocumentsByType(int userId, DocumentType documentType)
        {
            var documents = await _documentService.GetDocumentsByTypeAsync(userId, documentType);
            return Ok(documents);
        }

        [HttpGet("entity/{entityId}/type/{documentType}")]
        public async Task<ActionResult<IEnumerable<MedicalDocumentDto>>> GetDocumentsByRelatedEntity(int entityId, DocumentType documentType)
        {
            var documents = await _documentService.GetDocumentsByRelatedEntityAsync(entityId, documentType);
            return Ok(documents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalDocumentDto>> GetDocumentById(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound($"Документ з ID {id} не знайдено");
            }

            return Ok(document);
        }

        [HttpPost]
        public async Task<ActionResult<MedicalDocumentDto>> CreateDocument([FromBody] MedicalDocumentCreateDto documentCreateDto)
        {
            try
            {
                var document = await _documentService.CreateDocumentAsync(documentCreateDto);
                return CreatedAtAction(nameof(GetDocumentById), new { id = document.Id }, document);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] MedicalDocumentCreateDto documentUpdateDto)
        {
            try
            {
                await _documentService.UpdateDocumentAsync(id, documentUpdateDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            try
            {
                await _documentService.DeleteDocumentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult<string>> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Файл не завантажено або він порожній");
                }

                var filePath = await _documentService.UploadDocumentFileAsync(file);
                return Ok(new { filePath });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Сталася помилка при завантаженні файлу");
            }
        }
    }
}
