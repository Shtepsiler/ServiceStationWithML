using Bogus;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data.Configurations;
using PARTS.DAL.Entities;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Entities.Vehicle;

namespace PARTS.DAL.Data
{
    public class PartsDBContext : DbContext
    {
        public PartsDBContext()
        {
        }

        public PartsDBContext(DbContextOptions contextOptions) : base(contextOptions)
        {
 
                Database.EnsureCreated();
//              var scr =   Database.GenerateCreateScript();

        }


        public DbSet<Brand> Brands { get; set; }
        public DbSet<CategoryImage> CategoryImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<PartImage> PartImages { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<SubModel> SubModels { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Order> Orders { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryImageConfiguration());
            modelBuilder.ApplyConfiguration(new CategotyConfiguration());
            modelBuilder.ApplyConfiguration(new EngineConfiguration());
            modelBuilder.ApplyConfiguration(new MakeConfiguration());
            modelBuilder.ApplyConfiguration(new ModelConfiguration());
            modelBuilder.ApplyConfiguration(new PartConfiguration());
            modelBuilder.ApplyConfiguration(new PartImageConfiguration());
            modelBuilder.ApplyConfiguration(new SubModelConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());

        }

    }
}
