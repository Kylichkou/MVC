using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models
{
   
    public class PersonModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        [JsonIgnore]
        public TeamModel Team { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        [JsonIgnore]
        public Role Role { get; set; }

        public int RegisteredUserId { get; set; }
        [ForeignKey("RegisteredUserId")]
        public RegisteredUserModel RegisteredUser { get; set; }
    }
}