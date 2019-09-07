using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetPress.Models
{
    public class Roles
    {
        public int Id { get; set; }

        public string RoleType { get; set; }

       public ICollection<UserAccounts> UserAccounts { get; set; }
    }
}
