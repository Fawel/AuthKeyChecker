using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyChecker.Application.Infrastructure;
using KeyChecker.Infrastructure.Stubs;
using KeyChecker.Infrastructure.TestImplementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace KeyChecker.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            if(Env.IsDevelopment())
            {
                services.AddScoped<IApplicationRepository, InMemoryApplicationRepository>()
                    .AddScoped<IKeyRepository, InMemoryKeyRepository>();
            }

            else if(Env.IsProduction())
            {
                services.AddScoped<IApplicationRepository, ApplicationRepositoryStub>()
                    .AddScoped<IKeyRepository, KeyRepositoryStub>();
            }

            services.AddLogging();
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
        }
    }
}
