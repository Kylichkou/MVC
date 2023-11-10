using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models
{
    [Table("Teams")]
    public class TeamModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int RegisteredUserId { get; set; }
        [ForeignKey("RegisteredUserId")]
        public RegisteredUserModel RegisteredUser { get; set; }

        public ICollection<PersonModel> Persons { get; set; }
    }
}
