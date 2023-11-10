using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models
{
   
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegisteredUserId { get; set; }
    }
}
