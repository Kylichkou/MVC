using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using WebApplication4.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace WebApplication4.Controllers
{
    [ApiController]
    public class PersonsWorkController : Controller
    {
        private readonly ApplicationContext _db;
        public PersonsWorkController(ApplicationContext db)
        {
            _db = db;
        }

        [Route("/api/persons")]
        [HttpGet]
        public IActionResult work()
        {
            return View("work");
        }
        [HttpGet]
        [Route("/api/persons/{userId:int}")]
       async public Task<IActionResult> personGet(int? userId) 
        {
            var user = await _db.RegisteredUsers.FindAsync(userId);
            if (user == null) return (IActionResult)Results.NotFound(new { message = "Пользователь не найден" });

            var persons = await _db.Persons
                .Include(p => p.Role)
                    .Include(p => p.Team)
                    .Where(p => p.RegisteredUserId == userId)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Age,
                        Role = new
                        {
                            p.Role.Id,
                            p.Role.Name
                        },
                        Team = new
                        {
                            p.Team.Id,
                            p.Team.Name
                        }
                    })
                    .ToListAsync();
            return Ok(persons);
        }

        [HttpPost]
        [Route("/api/persons/{id:int}")]
        public async Task<IActionResult> CreatePerson(int id, AddPersonRequestModel request)
        {
            var registeredUserId = id;

            var role = _db.Roles.FirstOrDefault(r => r.Name == request.Role.Name && r.RegisteredUserId == registeredUserId);
            if (role == null)
            {
                role = new Role
                {
                    Name = request.Role.Name,
                    RegisteredUserId = registeredUserId
                };
                _db.Roles.Add(role);
                await _db.SaveChangesAsync();
            }

            var team = _db.Teams.FirstOrDefault(t => t.Name == request.Team.Name && t.RegisteredUserId == registeredUserId);
            if (team == null)
            {
                team = new TeamModel
                {
                    Name = request.Team.Name,
                    RegisteredUserId = registeredUserId
                };
                _db.Teams.Add(team);
                await _db.SaveChangesAsync();
            }

            var person = new PersonModel
            {
                Name = request.Name,
                Age = request.Age,
                RoleId = role.Id,
                TeamId = team.Id,
                RegisteredUserId = registeredUserId
            };
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            using (FileStream fstream = new FileStream("logFile.txt", FileMode.Append))
            {
                byte[] buffer = Encoding.Default.GetBytes($"Пользователь с id {registeredUserId} добавил запись " +  '\n');
                await fstream.WriteAsync(buffer, 0, buffer.Length);
            }
            var jsonString = JsonSerializer.Serialize(person, options);

            return Created($"/api/persons/{person.Id}", jsonString);
        }

        [Route("/api/persons/{userId:int}/{personId:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdatePerson(int userId, int personId, UpdatePersonRequestModel requestData)
        {
            var user = await _db.RegisteredUsers.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            var existingPerson = await _db.Persons.FirstOrDefaultAsync(p => p.Id == personId && p.RegisteredUserId == userId);
            if (existingPerson == null)
                return NotFound(new { message = "Человек не найден" });

            var existingRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name == requestData.Role.Name && r.RegisteredUserId == userId);
            if (existingRole == null)
            {
                existingRole = new Role
                {
                    Name = requestData.Role.Name,
                    RegisteredUserId = userId
                };
                _db.Roles.Add(existingRole);
                await _db.SaveChangesAsync();
            }

            var existingTeam = await _db.Teams.FirstOrDefaultAsync(t => t.Name == requestData.Team.Name && t.RegisteredUserId == userId);
            if (existingTeam == null)
            {
                existingTeam = new TeamModel
                {
                    Name = requestData.Team.Name,
                    RegisteredUserId = userId
                };
                _db.Teams.Add(existingTeam);
                await _db.SaveChangesAsync();
            }

            existingPerson.Name = requestData.Name;
            existingPerson.Age = requestData.Age;
            existingPerson.Role = existingRole; 
            existingPerson.Team = existingTeam; 
            _db.Persons.Update(existingPerson);
            await _db.SaveChangesAsync();
            using (FileStream fstream = new FileStream("logFile.txt", FileMode.Append))
            {
                byte[] buffer = Encoding.Default.GetBytes($"Пользователь с id {userId} изменил запись " + '\n');
                await fstream.WriteAsync(buffer, 0, buffer.Length);
            }
            return Ok(new
            {
                existingPerson.Id,
                existingPerson.Name,
                existingPerson.Age,
                Role = new
                {
                    existingRole.Id,
                    existingRole.Name
                },
                Team = new
                {
                    existingTeam.Id,
                    existingTeam.Name
                }
            });
        }
        [Route("/api/persons/{userId:int}/{personId:int}")]
        [HttpGet]
        public async Task<IActionResult> GetPerson(int userId, int personId)
        {
            var user = await _db.RegisteredUsers.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            var person = await _db.Persons
                .Include(p => p.Role)
                .Include(p => p.Team)
                .Where(p => p.RegisteredUserId == userId && p.Id == personId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Age,
                    Role = new
                    {
                        p.Role.Id,
                        p.Role.Name
                    },
                    Team = new
                    {
                        p.Team.Id,
                        p.Team.Name
                    }
                })
                .FirstOrDefaultAsync();

            if (person == null)
                return NotFound(new { message = "Человек не найден" });

            return Ok(person);
        }

        [HttpDelete]
        [Route("/api/persons/{userId:int}/{personId:int}")]
        public async Task<IActionResult> DeletePerson(int userId, int personId)
        {
            var user = await _db.RegisteredUsers.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            var existingPerson = await _db.Persons.FirstOrDefaultAsync(p => p.Id == personId && p.RegisteredUserId == userId);
            if (existingPerson == null)
                return NotFound(new { message = "Человек не найден" });

            _db.Persons.Remove(existingPerson);
            await _db.SaveChangesAsync();
            using (FileStream fstream = new FileStream("logFile.txt", FileMode.Append))
            {
                byte[] buffer = Encoding.Default.GetBytes($"Пользователь с id {userId} удалил запись " + '\n');
                await fstream.WriteAsync(buffer, 0, buffer.Length);
            }
            return Ok(new { message = "Человек успешно удален" });
        }
    }
}

