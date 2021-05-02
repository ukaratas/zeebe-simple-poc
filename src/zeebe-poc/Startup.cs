using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using zeebe_poc.Services;
using BlazorWebAssemblySignalRApp.Server.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using System.Text.Json.Serialization;

namespace zeebe_poc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<IBusinessService, BusinessService>();
            services.AddSingleton<IZeebeService, ZeebeService>();
            services.AddSingleton<IServerEventService, ServerEventService>();
            services.AddSingleton<IClientEventService, ClientEventService>();

            services.AddRazorPages();
            services.AddServerSideBlazor();


            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<EventHub>("/eventhub");
                endpoints.MapFallbackToPage("/_Host");
            });

            var zeebeService = app.ApplicationServices.GetService<IZeebeService>();
            //zeebeService.Deploy("sample-process.bpmn");
            zeebeService.StartWorkers();
        }
    }
}
