using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using MostAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using MostAPI.Context;

namespace MostAPI.Controllers
{
    [Route("api/form")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private static readonly string TelegramBotToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");


        private readonly TelegramBotClient _botClient;
        private readonly PostgresDbContext _context;

        public FormController(PostgresDbContext context)
        {
            _context = context;

            _botClient = new TelegramBotClient(TelegramBotToken);
        }


        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] PersonalityUser data)
        {
            if (data == null || string.IsNullOrEmpty(data.Name) || string.IsNullOrEmpty(data.Phone))
            {
                return BadRequest("Invalid data");
            }

            string message = $"Новый пользователь:\nИмя: {data.Name}\nТелефон: {data.Phone}";

            try
            {
                // Получаем список администраторов из базы данных
                var adminChatIds = await _context.admins
                    .Select(a => a.ChatId)
                    .ToListAsync();

                // Отправляем сообщение всем администраторам
                foreach (var chatId in adminChatIds)
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: message,
                        parseMode: ParseMode.Markdown
                    );
                }

                return Ok("Message sent successfully to all admins.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending message: {ex.Message}");
            }

        }
    }
}
