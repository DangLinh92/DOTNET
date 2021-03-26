using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstEmptyWebApp.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FirstEmptyWebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("<div>Hello World1!</div>");
                await next.Invoke();
                await context.Response.WriteAsync("<div>Hello World1-back!</div>");
            });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("<div>Hello World2!</div>");
                await next.Invoke();
                await context.Response.WriteAsync("<div>Hello World2-back!</div>");
            });

            app.UseMiddleware<SimpleMiddleware>();

            app.Use(async (context,next) =>
            {
                await context.Response.WriteAsync("Hello World 3!");
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(Configuration.GetSection("productionString").Value);
                    await context.Response.WriteAsync(Configuration.GetSection("Message").Value);
                    await context.Response.WriteAsync(Configuration.GetSection("ConnectionStrings:connection2").Value);
                    await context.Response.WriteAsync(Configuration.GetConnectionString("connection1"));
                    await context.Response.WriteAsync(Configuration.GetSection("connectionString12:db1").Value);    
                });
            });
        }
    }
}
