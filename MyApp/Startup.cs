using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Funq;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using ServiceStack.Configuration;
using MyApp.Class;
using MyApp.Models;

namespace MyApp
{
    public class Startup : ModularStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public new void ConfigureServices(IServiceCollection services)
        {
            Pullenti.Sdk.InitializeAll();
            //services.AddDbContext<AppContext>(op =>op.UseNpgsql("Host=localhost;Port=2345;Database=postgres;Username=postgres;Password=praj"));

#if DEBUG
            services.AddMvc(options => options.EnableEndpointRouting = false).AddRazorRuntimeCompilation();
#else
            services.AddMvc(options => options.EnableEndpointRouting = false);
#endif
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
            }

            //app.UseMiddleware<ChangeMaxAge>();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}
