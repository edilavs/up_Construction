using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpConstruction.DAL;
using UpConstruction.Models;

namespace UpConstruction
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=VALIDAPC;Database=UpConstruction;Trusted_Connection=TRUE"));

            services.AddIdentity<AppUser, IdentityRole>(

                options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.User.RequireUniqueEmail = false;
                }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "area",
                    "{area:exists}/{controller=dashboard}/{action=index}/{id?}");

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
