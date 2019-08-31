using FibonacciTestWebservice.OutputFormatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

[assembly: ApiController]

namespace FibonacciTestWebservice
{
    public class Startup
    {
        public static string Baseaddress = "http://localhost:5000";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvc(options => { options.OutputFormatters.Insert(0, new FibonacciOutputFormatter()); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"});
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseMvcWithDefaultRoute();
            app.UseStatusCodePages();
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }

    public class OpenApiInfo : Info
    {
    }
}