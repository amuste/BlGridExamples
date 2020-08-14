using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlGrid.Shared.Infrastructure.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlGrid.Hosted.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("app");

            var apiurl = builder.Configuration["APIURL"];

            builder.RootComponents.Add<App>("app");

            if (builder.Services.All(x => x.ServiceType != typeof(HttpClient)))
            {
                builder.Services.AddScoped(s => new HttpClient
                {
                    BaseAddress = new Uri(apiurl)
                });
            }

            builder.Services.AddHttpClient("WebHostURL", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddBlGridComponent();

            await builder.Build().RunAsync();
        }
    }
}
