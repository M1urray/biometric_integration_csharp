using biometrics_app.Data;
using biometrics_app.Interfaces;
using biometrics_app.Services;
using Microsoft.EntityFrameworkCore;

namespace biometrics_app.Extensions
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {

            services.AddDbContext<DataContext>(opt =>
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            services.AddScoped<IFingerprintService, FingerprintService>();
            services.AddScoped<IUserRepository,UserRepository>();
            return services;
        }
    }
}
