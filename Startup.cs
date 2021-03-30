using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bookcaseApi.Contexts;
using bookcaseApi.Entities;
using bookcaseApi.helpers;
using bookcaseApi.Models;
using bookcaseApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
            
            // Agregando AutoMapper
            services.AddAutoMapper(configuration =>
            {
                configuration.CreateMap<Author, AuthorDTO>();
                configuration.CreateMap<AuthorCreateDTO, Author>().ReverseMap();// Permite mapear en ambas direcciones;
                configuration.CreateMap<Book, BookDTO>();
            }, typeof(Startup));

            // Agregando IHostedService
            services.AddTransient<Microsoft.Extensions.Hosting.IHostedService, WriteToFileHostedService>();
            services.AddTransient<Microsoft.Extensions.Hosting.IHostedService, WriteToFile2HostedService>();

            // Agregando el custom filter
            services.AddScoped<CustomFilterToAction>();

            // Agregando servicio de cache.
            services.AddResponseCaching();

            // Agregando servicio de JWT. Asi es como se configura para la uthorizacion.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer( options => 
                options.TokenValidationParameters = new TokenValidationParameters { 
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                });

            // Agregando Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BookCaseDbContext>()
                .AddDefaultTokenProviders();

            // Agregando el context de la DB
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
            //Agregamos nuestro middleware de autenticacion
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
