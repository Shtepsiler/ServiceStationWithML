using Microsoft.Extensions.DependencyInjection;
using PARTS.BLL.Mapping;
using PARTS.BLL.Services;
using PARTS.BLL.Services.Interaces;

namespace PARTS.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPartsBll(this IServiceCollection services)
        {

            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryImageService, CategoryImageService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMakeService, MakeService>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<IPartImageService, PartImageService>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<ISubModelService, SubModelService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IEngineService, EngineService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            return services;
        }
    }
}
