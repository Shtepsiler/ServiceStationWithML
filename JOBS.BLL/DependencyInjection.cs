using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace JOBS.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBuisnesLogicLayer(this IServiceCollection services)
        {

    


            services.AddAutoMapper(assemblies: Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
           

            return services;
        }



    }
}
