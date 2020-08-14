using System;
using System.Linq;
using System.Net.Http;
using BlGrid.Shared.Infrastructure.Services;
using DnetSpinnerComponent.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlGrid.ServerSide
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
            if (services.All(x => x.ServiceType != typeof(HttpClient)))
            {
                services.AddScoped(s => new HttpClient
                {
                    BaseAddress = new Uri(Configuration["APIURL"])
                });
            }

            services.AddHttpClient();

            services.AddHttpClient("WebHostURL", client => client.BaseAddress = new Uri(Configuration["WebHostURL"]));

            services.AddRazorPages();

            services.AddServerSideBlazor();

            services.AddBlGridComponent();
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
                app.UseExceptionHandler("/Error");
              
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
