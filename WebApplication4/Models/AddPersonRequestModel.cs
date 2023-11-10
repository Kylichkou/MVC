using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WebApplication4.Models
{
    public class AddPersonRequestModel
    {
        //[Required(ErrorMessage = "Поле Name обязательно для заполнения.")]
        public string Name { get; set; }
        

        public Rolee Role { get; set; } // Add this property
        public Team Team { get; set; } // Add this property
        //[Required(ErrorMessage = "Поле Age обязательно для заполнения.")]
        public int Age { get; set; }

       
    }
}