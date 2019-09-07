using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetPress.Models
{
    public class UserAccountsContext : DbContext
    {
        public UserAccountsContext (DbContextOptions<UserAccountsContext> options)
            : base(options)
        {
        }

        public DbSet<NetPress.Models.UserAccounts> UserAccounts { get; set; }
        
        public DbSet<Roles> Roles { get; set; }
    }
}
