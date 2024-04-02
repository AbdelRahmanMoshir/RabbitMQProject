using Microsoft.AspNetCore.Mvc;
using RabbitMQProducer.Models;
using RabbitMQProducer.Services;

namespace RabbitMQProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageProducer _messageProducer;
        public MessagesController(
            ILogger<MessagesController> logger,
            IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }
        // In Memory DB
        public static readonly List<Messages> _Messages = new();

        [HttpPost(Name = "SendMessage")]

        public IActionResult SendMessage(Messages newMessage)
        {
            if (!ModelState.IsValid) return BadRequest();

            _logger.LogInformation($"Request is :- {newMessage}");

            _Messages.Add(newMessage);

            _messageProducer.SendMessage(newMessage);

            return Ok();
        }

    }
}
