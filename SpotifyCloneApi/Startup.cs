using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SpotifyClone.Core.Interfaces;
using SpotifyClone.Core.Services;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

[assembly: ApiController]
namespace SpotifyCloneApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(opts =>
            {
                opts.LowercaseUrls = true;
                opts.LowercaseQueryStrings = true;
            });

            var appSettings = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettings);

            services.AddCors(opts => opts.AddPolicy("CorsPolicy", policy => policy.AllowAnyOrigin()
                                                                                  .AllowAnyHeader()
                                                                                  .AllowAnyMethod()));

            services.AddHttpClient("lyrics", c =>
            {
                c.BaseAddress = new Uri("https://www.azlyrics.com/lyrics/");
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            RegisterServices(services);
            AddSwagger(services);

            services.AddControllers()
                    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("CorsPolicy");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpotifyCloneApi v1"));
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(opts =>
            {
                opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Version 1");
                opts.RoutePrefix = "swagger"; // Endpoint: /swagger/index.html
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton(SpotifyClientConfig.CreateDefault());

            services.AddScoped<IAccountService, AccountService>();
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo() { Title = "SpotifyClone API", Version = "v1" });

                var bearerScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Name = "Authorization",
                    Scheme = "bearer",
                    Description = "Enter JWT token bellow",
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };

                opts.AddSecurityDefinition("Bearer", bearerScheme);
                opts.AddSecurityRequirement(new OpenApiSecurityRequirement() { { bearerScheme, Array.Empty<string>() } });
            });
        }
    }
}
