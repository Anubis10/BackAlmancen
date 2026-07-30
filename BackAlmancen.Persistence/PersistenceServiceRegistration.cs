
using BackAlmancen.Persistence.Context;
using BackAlmancen.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackAlmancen.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ContextDB>(options =>
            options.UseInMemoryDatabase("JuguetesBD"));


            services.AddScoped<IRepository<Producto>, ProductoRepository>();



            return services;

        }
    }
}
