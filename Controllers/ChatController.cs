using Microsoft.AspNetCore.Mvc;

namespace ChatBotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] ChatMessage message)
        {
            var userMsg = message.Text?.ToLower() ?? "";
            string response;

            if (userMsg.Contains("hola"))
                response = "¡Hola! ¿En qué puedo ayudarte?";
            else if (userMsg.Contains("portfolio"))
                response = "Este es mi portfolio estilo VS Code. ¿Quieres ver mis proyectos?";
            else if (userMsg.Contains("cv"))
                response = "Puedes descargar mi CV desde la barra lateral del portfolio.";
            else
                response = "Lo siento, aún estoy aprendiendo. Pregúntame otra cosa.";

            return Ok(new { reply = response });
        }
    }

    public class ChatMessage
    {
        public string Text { get; set; }
    }
}
