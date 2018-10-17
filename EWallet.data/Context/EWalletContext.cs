using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.data
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual IEnumerable<Operation> Operations { get; set; }
        public virtual IEnumerable<Category> Categories { get; set; }
    }

    public class EWalletContext : IdentityDbContext<ApplicationUser>
    {
        public EWalletContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public EWalletContext(string connectionString)
            : base(connectionString, throwIfV1Schema: false)
        {
        }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Category> Categories { get; set; }

        public static EWalletContext Create()
        {
            return new EWalletContext("DefaultConnection");
        }
    }
}
