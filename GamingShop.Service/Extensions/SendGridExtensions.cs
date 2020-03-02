using Microsoft.Extensions.DependencyInjection;

namespace GamingShop.Service.Extensions
{
    public static class SendGridExtensions
    {
        public static IServiceCollection AddSendGrid<TImplementation>(this IServiceCollection services) where TImplementation: class, IEmailSender
        {
            services.AddScoped<IEmailSender, TImplementation>();

            return services;
        }
    }
}
