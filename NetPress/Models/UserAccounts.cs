using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace NetPress.Models
{
    public class UserAccounts
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string UserPassword { get; set; }

        public string RolesType {get; set;}

        
        public Roles Role { get; set; }
    }
}
