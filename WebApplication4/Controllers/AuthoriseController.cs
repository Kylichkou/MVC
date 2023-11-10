using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using Serilog;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text;

namespace WebApplication4.Controllers
{

    [ApiController]
    public class AuthoriseController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<AuthoriseController> _logger;

        public AuthoriseController(ApplicationContext db, ILogger<AuthoriseController> logger)
        {
            _db = db;
            _logger = logger;
        }
        [Route("/api/login")]
        [HttpGet]
        public IActionResult login()
        {
            return View("login");
        }
        [HttpPost("/api/login")]
        public async Task<IActionResult> loginAsync(Login userData)
        {
            var existingUser = await _db.RegisteredUsers.FirstOrDefaultAsync(u => u.Username == userData.Username && u.Password == userData.Password);
            if (existingUser == null)
            {
                using (FileStream fstream = new FileStream("logFile.txt", FileMode.Append))
                {
                    byte[] buffer = Encoding.Default.GetBytes("Неверный логин или пароль: введённые данные " + userData.Username + " " + userData.Password + '\n');
                    await fstream.WriteAsync(buffer, 0, buffer.Length);
                }
                return NotFound(new { message = "Неверный логин или пароль" });
            }
            using (FileStream fstream = new FileStream("logFile.txt", FileMode.Append))
            {
                byte[] buffer = Encoding.Default.GetBytes("Пользователь автоизирован " + userData.Username +'\n');
                await fstream.WriteAsync(buffer, 0, buffer.Length);
            }
            return Ok(new { userId = existingUser.Id });

        }

    }

}
