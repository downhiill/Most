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

        [HttpGet("random")]
        public async Task<ActionResult<List<Faq>>> GetRandom()
        {
            var faqs = await _faqService.GetRandomAsync();
            return Ok(faqs);
        }

        // Получить вопрос по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> GetById(int id)
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
        public async Task<ActionResult> Update(int id, [FromBody] Faq faq)
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
        public async Task<ActionResult> Delete(int id)
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
