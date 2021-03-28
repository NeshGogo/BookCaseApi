using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bookcaseApi.Contexts;
using bookcaseApi.helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace bookcaseApi
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
            // Agregando el custom filter
            services.AddScoped<CustomFilterToAction>();
            //Agregando servicio de cache.
            services.AddResponseCaching();
            // Agregando servicio de JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
            services.AddDbContext<BookCaseDbContext>( options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));
            services.AddControllers( options => {
                options.Filters.Add(new MyExceptionFilter());
                // Si hubiese inyeccion de dependencias en el filtro.
                // options.Filters.Add(typeof(MyExceptionFilter));
            })
                .AddNewtonsoftJson( options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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
            app.UseAuthentication();
            // Agregamos el middleware the cache.
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllers();
            });
        }
    }
}
