    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
    using WebApplication4.Models;

    namespace WebApplication4.Controllers
    {
        [ApiController]
        public class RegisterController : Controller
        {
            private readonly ApplicationContext _db;

            public RegisterController(ApplicationContext db)
            {
                _db = db;
            }

            [HttpGet("/")]
            public ViewResult register()
            {
                return View();
            }
            [HttpPost]
            [Route("/api/register/register")]
            public async Task<ActionResult<RegisteredUserModel>> Register([FromBody]RegisteredUserModel user)
            {
                var existingUsername = await _db.RegisteredUsers.FirstOrDefaultAsync(u => u.Username == user.Username);
                if (existingUsername != null)
            {
                using (FileStream fstream = new FileStream("logFile.txt", FileMode.Append))
                {
                    byte[] buffer = Encoding.Default.GetBytes($"{user.Username} данное имя пользователя уже используется" + '\n');
                    await fstream.WriteAsync(buffer, 0, buffer.Length);
                }
                return BadRequest(new { message = "Имя пользователя уже занято" });
                }

                var existingEmail = await _db.RegisteredUsers.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingEmail != null)
                {
                using (FileStream fstream = new FileStream("logFile.txt", FileMode.Append))
                {
                    byte[] buffer = Encoding.Default.GetBytes($"{user.Email} данная электронная почта уже используется" + '\n');
                    await fstream.WriteAsync(buffer, 0, buffer.Length);
                }
                return BadRequest(new { message = "Электронная почта уже используется" });
                }

                await _db.RegisteredUsers.AddAsync(user);
                await _db.SaveChangesAsync();
            using (FileStream fstream = new FileStream("logFile.txt", FileMode.Append))
            {
                byte[] buffer = Encoding.Default.GetBytes($"{user.Username} успешно зарегистрирован" + '\n');
                await fstream.WriteAsync(buffer, 0, buffer.Length);
            }
            return Ok(user);
            }
        }
    }