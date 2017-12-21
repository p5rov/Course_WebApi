using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace SecretSanta.WebApi.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string DisplayName { get; set; }
        
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType((DataType.Password))]
        [Display(Name="Password")]
        public string Password { get; set; }
    }
}