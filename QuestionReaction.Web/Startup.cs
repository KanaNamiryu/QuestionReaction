using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestionReaction.Data;
using QuestionReaction.Data.Interfaces;
using QuestionReaction.Services;
using QuestionReaction.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionReaction.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                // cn = bdd locale
                var cn = Configuration.GetConnectionString("cn");
                options.UseSqlServer(cn)
#if DEBUG
                .EnableSensitiveDataLogging();
#endif
            });

            // configurer l'authentification par cookies (un "AddAuthentication" pour toutes les méthodes de connexions (un . par méthode apres))
            services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    //config de l'authentification
                    options.LoginPath = "/home/login";
                    options.AccessDeniedPath = "/home/accesDenied";
                    options.ReturnUrlParameter = "returnUrl";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                    // config du cookie
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });



            // ajout des services au conteneur de DI (Dependence Injection)
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<IRegisterService, RegisterService>();


            // permet l'acces au IHttpContextAccessor (contexte http)
            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // charger le service de routage en mémoire (ne l'utilise pas encore)
            app.UseRouting();

            // authentification
            app.UseAuthentication();

            // si utilisateur authentifié → donne l'autorisation
            app.UseAuthorization();

            // redirection de l'utilisateur selon autorisation
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
