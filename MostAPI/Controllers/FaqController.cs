using Microsoft.AspNetCore.Mvc;
using MostAPI.Data;
using MostAPI.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaqController : ControllerBase
    {
        private readonly FaqService _faqService;

        public FaqController(FaqService faqService)
        {
            _faqService = faqService;
        }

        // Получить все вопросы и ответы
        [HttpGet("all")]
        public async Task<ActionResult<List<Faq>>> GetAll()
        {
            var faqs = await _faqService.GetAllAsync();
            return Ok(faqs);
        }

        // Получить вопросы и ответы с пагинацией (случайные данные)
        [HttpGet("page")]
        public async Task<ActionResult<List<Faq>>> GetPage([FromQuery] int page = 1, [FromQuery] int pageSize = 4)
        {
            var faqs = await _faqService.GetPageAsync(page, pageSize);
            return Ok(faqs);
        }

        // Получить вопрос по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> GetById(string id)
        {
            var faq = await _faqService.GetByIdAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return Ok(faq);
        }

        // Создание нового вопроса и ответа
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Faq faq)
        {
            await _faqService.CreateAsync(faq);
            return CreatedAtAction(nameof(GetById), new { id = faq.Id }, faq);
        }

        // Обновить существующий вопрос
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] Faq faq)
        {
            var existingFaq = await _faqService.GetByIdAsync(id);
            if (existingFaq == null)
            {
                return NotFound();
            }
            await _faqService.UpdateAsync(id, faq);
            return NoContent();
        }

        // Удалить вопрос
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var faq = await _faqService.GetByIdAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            await _faqService.DeleteAsync(id);
            return NoContent();
        }
    }
}
