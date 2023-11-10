using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    [Table("RegisteredUser")]
    public class RegisteredUserModel
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public ICollection<PersonModel>? Persons { get; set; }
        [JsonIgnore]
        public ICollection<Role>? Roles { get; set; }
        [JsonIgnore]
        public ICollection<TeamModel>? Teams { get; set; }
    }
}