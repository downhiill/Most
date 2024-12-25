using Microsoft.AspNetCore.Mvc;
using MostAPI.Data;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using System.Collections.Generic;

namespace MostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly string TelegramBotToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");

        // Список Chat ID всех администраторов
        private static readonly List<string> AdminChatIds = new List<string>
        {
            "935118337",  // Первый администратор
            "821942555",  // Второй администратор
            "592057109",  // Третий администратор
            "448145168"   // Четвертый администратор
        };

        private readonly TelegramBotClient _botClient;

        public ValuesController()
        {
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
                // Отправляем сообщение всем администраторам
                foreach (var chatId in AdminChatIds)
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
