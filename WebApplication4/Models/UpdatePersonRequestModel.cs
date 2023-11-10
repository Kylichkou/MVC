using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class UpdatePersonRequestModel
    {
        public string Name { get; set; }
        public Rolee Role { get; set; } 
        public Team Team { get; set; } 
        public int Age { get; set; }

    }
}