using BlGrid.Api.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlGrid.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(
                options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder
                            .WithOrigins("https://localhost:44368/", "https://localhost:44363", "https://localhost:44332")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .Build());
                });

            services.AddHttpContextAccessor();

            services.AddScoped<IBlGridDbContext>(provider => provider.GetService<BlGridDbContext>());

            services.AddTransient(typeof(IPersonRepository), typeof(PersonRepository));

            var connectionString = Configuration.GetConnectionString("BlGrid");

            services.AddDbContext<BlGridDbContext>(options => options.UseSqlServer(connectionString, x => x.MigrationsAssembly("BlGrid.Api")));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
