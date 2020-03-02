using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace GamingShop.Service.Extensions
{
    public static class IServiceCollectionExtensions
    {


        public static IServiceCollection SetUpApplicationServices(this IServiceCollection services, Action<IServiceCollection> configuration = null)
        {
            configuration?.Invoke(services);

            services.AddScoped<IGame, GameService>();
            services.AddScoped<ICart, CartService>();
            services.AddScoped<IApplicationUser, ApplicationUserService>();
            services.AddScoped<IOrder, OrderService>();

            return services;
        }

        public static IServiceCollection SetUpJWT(this IServiceCollection services, Action<JWTConfiguration> configuration)
        {
            var conf = new JWTConfiguration();

            configuration.Invoke(conf);

            var key = Encoding.UTF8.GetBytes(conf.Key);

            var tokentValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = conf.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = conf.ValidateIssuer,
                ValidateAudience = conf.ValidateAudience,
                ClockSkew = conf.ClockSkew
            };


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = tokentValidationParams;
            });

            return services;
        }

    }
}
