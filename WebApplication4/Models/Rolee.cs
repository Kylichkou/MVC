using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models
{
    
    public class Rolee
    {
       
        public string Name { get; set; }
        public int RegisteredUserId { get; set; }

    }
}
