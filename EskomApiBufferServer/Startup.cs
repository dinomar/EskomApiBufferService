using EskomApiBufferService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Extensions.Logging;

namespace EskomApiBufferServer
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
            int EskomBufferServiceRetries;
            int EskomBufferServiceStatusMinRange;
            int EskomBufferServiceStatusMaxRange;
            int EskomBufferServiceTimeInMinutes;
            int EskomBufferServiceMaxLogs;

            services.AddSingleton<EskomBufferService>(sp =>
                new EskomBufferService(sp.GetRequiredService<ILogger<EskomBufferService>>(),
                new EskomBufferServiceConfiguration
                {
                    EskomApiWrapper = new EskomApiWrapper(),
                    Retries = (Int32.TryParse(Configuration["EskomBufferService:Retries"]?.ToString(), out EskomBufferServiceRetries) ? EskomBufferServiceRetries : 3),
                    StatusMinRange = (Int32.TryParse(Configuration["EskomBufferService:StatusMinRange"]?.ToString(), out EskomBufferServiceStatusMinRange) ? EskomBufferServiceStatusMinRange : 0),
                    StatusMaxRange = (Int32.TryParse(Configuration["EskomBufferService:StatusMaxRange"]?.ToString(), out EskomBufferServiceStatusMaxRange) ? EskomBufferServiceStatusMaxRange : 10),
                    DelayInMinutes = (Int32.TryParse(Configuration["EskomBufferService:DelayInMinutes"]?.ToString(), out EskomBufferServiceTimeInMinutes) ? EskomBufferServiceTimeInMinutes : 60),
                    MaxLogs = (Int32.TryParse(Configuration["EskomBufferService:EskomBufferServiceMaxLogs"]?.ToString(), out EskomBufferServiceMaxLogs) ? EskomBufferServiceMaxLogs : 1000)
                }));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // GetService
            EskomBufferService bufferService = app.ApplicationServices.GetRequiredService<EskomBufferService>();
            bufferService.Start();
        }
    }
}
