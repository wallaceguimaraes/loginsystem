using LoginSystem.Models.Interfaces;
using LoginSystem.Models.ServiceModel;

namespace LoginSystem.Extensions.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTransientServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthentication, Authentication>();
        }
    }
}
