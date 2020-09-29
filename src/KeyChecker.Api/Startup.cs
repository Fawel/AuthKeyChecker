using KeyChecker.Application.Infrastructure;
using KeyChecker.Infrastructure.Stubs;
using KeyChecker.Infrastructure.TestImplementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using KeyChecker.Application;

namespace KeyChecker.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            if (Env.IsDevelopment())
            {
                services.AddScoped<IApplicationRepository, InMemoryApplicationRepository>()
                    .AddScoped<IKeyRepository, InMemoryKeyRepository>();
            }

            else if (Env.IsProduction())
            {
                services.AddScoped<IApplicationRepository, ApplicationRepositoryStub>()
                    .AddScoped<IKeyRepository, KeyRepositoryStub>();
            }

            services.AddScoped<AuthKeyValidator>();

            services.AddLogging();
        }

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
