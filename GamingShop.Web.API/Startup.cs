using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Service.Extensions;
using GamingShop.Service.Implementation;
using GamingShop.Service.Services;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GamingShop.Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationOptions>(config => 
            {
                config.Secret_Key = Configuration["JWT_Config:Secret_Key"].ToString();
                config.ClientURL = Configuration["JWT_Config:ClientURL"];
                config.SendGridAPIKey = Configuration["SendGird_Config:APIKey"];
                config.JWTSecretKey = Configuration["JWT_Config:Secret_Key"];
                config.ImagesPath = @"C:\Users\adria\Desktop\AngularApp\GamingShop_Frontend\GamingShop-Frontend\src\assets\img";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(setup =>
            {
                setup.UseMemberCasing();
            });

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(conf =>
            {
                conf.Password.RequiredLength = 6;
                conf.Password.RequireNonAlphanumeric = false;
            });


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(cors => cors.AddPolicy("DevCorsPolicy", options => 
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            }));

            services.AddSendGrid<SendGridEmailSender>();

            services.SetUpApplicationServices(conf => 
            {
                conf.AddScoped<IImage, ImageService>();
            });

            services.AddSingleton<JWTToken>();

            services.SetUpJWT(conf => 
            {
                conf.ValidateIssuerSigningKey = true;
                conf.ValidateIssuer = false;
                conf.ValidateAudience = false;
                conf.Key = Configuration["JWT_Config:Secret_Key"];
            });

        }



        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DevCorsPolicy");
            }

            app.UseMvc(routes => routes.MapRoute(
                name:"default",
                template: "api/{controller}/{action?}/{id?}/{params?}"
                ));
        }
    }
}
