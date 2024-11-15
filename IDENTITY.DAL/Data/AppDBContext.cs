using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IDENTITY.DAL.Data.Configurations;
using IDENTITY.DAL.Entities;
using Microsoft.Data.SqlClient;

namespace IDENTITY.DAL.Data
{
    public class AppDBContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDBContext(DbContextOptions contextOptions) : base(contextOptions)
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (SqlException e)
            {
                Task.Delay(1000);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

        }

    }
}
