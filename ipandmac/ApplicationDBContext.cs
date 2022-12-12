using ipandmac;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EmployeeClass.Model
{


    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }


    }
}
