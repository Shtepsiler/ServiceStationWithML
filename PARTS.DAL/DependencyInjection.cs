using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using PARTS.DAL.Interfaces;
using PARTS.DAL.Repositories;

namespace PARTS.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPartsDal(this IServiceCollection services)
        {
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICategoryImageRepository, CategoryImageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IEngineRepository, EngineRepository>();
            services.AddScoped<IMakeRepository, MakeRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IPartImageRepository, PartImageRepository>();
            services.AddScoped<IPartRepository, PartRepository>();
            services.AddScoped<ISubModelRepository, SubModelRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();


            return services;
        }
    }
}
