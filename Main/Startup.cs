using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Main.Models;
using System.Threading.Tasks;
using System;
using Main.Handlers;

namespace Main
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddDbContext<MainContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MainContext")));

            services.AddTransient<BirthdayHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime, IServiceProvider provider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Workers}/{action=Index}/{id?}");
            });
            
            SeedData.Initialize(app.ApplicationServices);
            lifetime.ApplicationStarted.Register(() => {
                
                
                while (true)
                {
                    DateTime currentHour = DateTime.Now;
                    var t = Task.Run(async delegate
                    {
                        if(currentHour.Hour == 9)provider.GetService(typeof(BirthdayHandler));
                        await Task.Delay(TimeSpan.FromSeconds(60));
                    });
                    t.Wait();
                }
            });
        }
    }
}
